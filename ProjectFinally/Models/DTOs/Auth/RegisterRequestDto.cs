using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.Auth;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Nombre del usuario requerido")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email es requerido")]
    [EmailAddress(ErrorMessage = "Formato invalido de correo")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contraseña requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe ser mayor a 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Role ID is required")]
    public int RoleId { get; set; }
}
