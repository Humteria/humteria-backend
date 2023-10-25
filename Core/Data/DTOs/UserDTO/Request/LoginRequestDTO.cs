﻿using System.ComponentModel.DataAnnotations;

namespace Humteria.Data.DTOs.UserDTO.Request;

public class LoginRequestDTO
{
    [Required]
    public string UsernameOrMail { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
