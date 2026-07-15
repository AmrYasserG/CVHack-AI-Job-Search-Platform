using System;
using System.Collections.Generic;

namespace CVHack.BLL.DTOs.Auth;

public class AuthResultDto
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<string> Roles { get; set; } = new();
}
