using CVHack.BLL;
using CVHack.DAL;
using CVHack.AI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenAI;
using Scalar.AspNetCore;
using System.ClientModel;
using System.Text;
using CVHack.AI.AIChat;


namespace CVHack.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddBusinessLogic(builder.Configuration);

            // Add JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Policy-Based Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("JobSeekerOnly", policy => policy.RequireRole("JobSeeker"));
            });

            // CORS - allow the Angular dev server to call this API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            builder.Services.AddControllers();

            // --- AI: chat client (Groq, OpenAI-compatible) ---
            builder.Services.AddAiIntegrations(builder.Configuration);
            builder.Services.AddInterviewCoach();


            // Configure built-in .NET OpenAPI with JWT Security & Operation Transformers
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddOperationTransformer<SecurityRequirementsOperationTransformer>();
            });

            var app = builder.Build();

            // Automatic migrations and seeding on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    await context.Database.MigrateAsync();
                    await DbSeeder.SeedDataAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database migration or seeding.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapInterviewCoachEndpoints();



            //-------------------------------------------------------------------------------------------
            // Temporary RAG test endpoints
            //-------------------------------------------------------------------------------------------
            app.MapGet("/rag/documents", async (IDocumentLoader loader) =>
            {
                var docs = await loader.LoadAsync();

                return Results.Ok(docs.Select(d => new
                {
                    d.KnowledgeBase,
                    d.Category,
                    d.FileName
                }));
            });


            app.MapGet("/rag/chunks", async (IDocumentLoader loader, TextChunker chunker) =>
            {
                var docs = await loader.LoadAsync();
                var chunks = docs.SelectMany(chunker.Chunk).ToList();

                return Results.Ok(new
                {
                    TotalDocuments = docs.Count,
                    TotalChunks = chunks.Count,
                    Sample = chunks.Take(2).Select(c => new
                    {
                        c.KnowledgeBase,
                        c.Category,
                        c.SourceFile,
                        c.Metadata,
                        TextPreview = c.Text[..Math.Min(100, c.Text.Length)]
                    })
                });
            });

            app.MapGet("/rag/embed-test", async (IEmbeddingService embedder) =>
            {
                var vector = await embedder.EmbedAsync("What is dependency injection?");
                return Results.Ok(new
                {
                    Dimensions = vector.Length,
                    Sample = vector.Take(5)
                });
            });


            app.MapGet("/rag/search", async (IRagService rag, string query) =>
            {
                var results = await rag.SearchAsync(query, "InterviewQuestions", topK: 3);
                return Results.Ok(results);
            });
            //-------------------------------------------------------------------------------------------

            await app.RunAsync();
        }
    }

    // Transformer to inject JWT Bearer Authentication into Scalar / OpenAPI Document
    public class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer" || authScheme.Name == JwtBearerDefaults.AuthenticationScheme))
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Bearer token only."
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["Bearer"] = securityScheme;
            }
        }
    }

    // Transformer to apply JWT Bearer security requirements ONLY to endpoints requiring authorization
    public class SecurityRequirementsOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var metadata = context.Description.ActionDescriptor.EndpointMetadata;

            var hasAuthorize = metadata.Any(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute);
            var hasAllowAnonymous = metadata.Any(m => m is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute);

            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                };
                operation.Security.Add(securityRequirement);
            }

            return Task.CompletedTask;
        }
    }
}
