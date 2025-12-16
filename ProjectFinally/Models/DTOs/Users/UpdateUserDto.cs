using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.Users;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Email es requerido")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string? Password { get; set; } // Opcional al actualizar

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Role ID es requerido")]
    public int RoleId { get; set; }
}
