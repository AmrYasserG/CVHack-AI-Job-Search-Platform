using CVHack.Common;
using CVHack.DAL;
using FluentValidation;

namespace CVHack.BLL
{
    public class SupportTicketManager : ISupportTicketManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SupportTicketCreateDto> _createValidator;
        private readonly IValidator<UpdateTicketStatusDto> _updateValidator;

        public SupportTicketManager(
            IUnitOfWork unitOfWork,
            IValidator<SupportTicketCreateDto> createValidator,
            IValidator<UpdateTicketStatusDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /////////////   USER

        public async Task<Result<SupportTicketReadDto>> CreateAsync(SupportTicketCreateDto dto, string userId)
        {
            #region Validation

            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Result<SupportTicketReadDto>.Failure(
                    errors,
                    "One or more validation errors occurred.",
                    400);
            }

            #endregion

            var ticket = new SupportTicket
            {
                UserId = userId,
                Subject = dto.Subject,
                Category = dto.Category,
                Description = dto.Description,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.SupportTicketRepository.Insert(ticket);

            await _unitOfWork.SaveChangesAsync();

            var result = new SupportTicketReadDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Category = ticket.Category,
                Description = ticket.Description,
                Status = ticket.Status,
                Reply = ticket.Reply,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };

            return Result<SupportTicketReadDto>.Success(
                result,
                "Ticket created successfully.",
                201);
        }

        public async Task<Result<IEnumerable<SupportTicketListDto>>> GetUserTicketsAsync(string userId)
        {
            var tickets = await _unitOfWork.SupportTicketRepository.GetUserTicketsAsync(userId);

            var result = tickets.Select(t => new SupportTicketListDto
            {
                Id = t.Id,
                Subject = t.Subject,
                Category = t.Category,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            });

            return Result<IEnumerable<SupportTicketListDto>>.Success(
                    result,
                    "Tickets retrieved successfully.",
                    200);
        }

        public async Task<Result<SupportTicketReadDto>> GetTicketByIdAsync(int id, string userId)
        {
            var ticket = await _unitOfWork.SupportTicketRepository.GetByIdAsync(id);

            if (ticket is null || ticket.UserId != userId)
            {
                return Result<SupportTicketReadDto>.Failure(
                    "Ticket not found.",
                    "The requested ticket does not exist.",
                    404);
            }

            var result = new SupportTicketReadDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Category = ticket.Category,
                Description = ticket.Description,
                Status = ticket.Status,
                Reply = ticket.Reply,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };

            return Result<SupportTicketReadDto>.Success(
                result,
                "Ticket retrieved successfully.",
                200);
        }


        /////////////   ADMIN

        public async Task<Result<IEnumerable<AdminSupportTicketListDto>>> GetAllTicketsAsync()
        {
            var tickets = await _unitOfWork.SupportTicketRepository.GetAllTicketsAsync();

            var result = tickets.Select(t => new AdminSupportTicketListDto
            {
                Id = t.Id,
                UserName = t.User.FirstName!,
                Email = t.User.Email!,
                Subject = t.Subject,
                Category = t.Category,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            });

            return Result<IEnumerable<AdminSupportTicketListDto>>.Success(
                    result,
                    "Support tickets retrieved successfully.",
                    200);
        }

        public async Task<Result<AdminSupportTicketReadDto>> GetTicketDetailsAsync(int id)
        {
            var ticket = await _unitOfWork.SupportTicketRepository.GetTicketWithUserAsync(id);

            if (ticket is null)
            {
                return Result<AdminSupportTicketReadDto>.Failure(
                    "Ticket not found.",
                    "The requested ticket does not exist.",
                    404);
            }

            var result = new AdminSupportTicketReadDto
            {
                Id = ticket.Id,
                UserName = ticket.User.FirstName!,
                Email = ticket.User.Email!,
                Subject = ticket.Subject,
                Category = ticket.Category,
                Description = ticket.Description,
                Status = ticket.Status,
                Reply = ticket.Reply,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };

            return Result<AdminSupportTicketReadDto>.Success(
                result,
                "Ticket retrieved successfully.",
                200);
        }

        public async Task<Result<AdminSupportTicketReadDto>> UpdateTicketStatusAsync( int id, UpdateTicketStatusDto dto)
        {
            #region Validation

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Result<AdminSupportTicketReadDto>.Failure(
                    errors,
                    "One or more validation errors occurred.",
                    400);
            }

            #endregion

            var ticket = await _unitOfWork.SupportTicketRepository.GetTicketWithUserAsync(id);

            if (ticket is null)
            {
                return Result<AdminSupportTicketReadDto>.Failure(
                    "Ticket not found.",
                    "The requested ticket does not exist.",
                    404);
            }

            if (ticket.Status == dto.Status && ticket.Reply == dto.Reply)
            {
                return Result<AdminSupportTicketReadDto>.Failure(
                    "No changes detected.",
                    "Ticket is already up to date.",
                    400);
            }

            ticket.Status = dto.Status;
            ticket.Reply = dto.Reply;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            var result = new AdminSupportTicketReadDto
            {
                Id = ticket.Id,
                UserName = ticket.User.FirstName!,
                Email = ticket.User.Email!,
                Subject = ticket.Subject,
                Category = ticket.Category,
                Description = ticket.Description,
                Status = ticket.Status,
                Reply = ticket.Reply,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };

            return Result<AdminSupportTicketReadDto>.Success(
                result,
                "Ticket updated successfully.",
                200);
        }

    }
}