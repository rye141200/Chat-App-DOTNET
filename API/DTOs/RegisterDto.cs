using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [Length(minimumLength: 4, maximumLength: 8, ErrorMessage = "Password must be at least 4 characters long and at most 8 characters")]
    public required string Password { get; set; }
}
