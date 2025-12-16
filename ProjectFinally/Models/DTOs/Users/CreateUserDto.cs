using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "Nombre de usuario es requerido")]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email es requerido")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Role ID es requerido")]
    public int RoleId { get; set; }
}
