using System.ComponentModel.DataAnnotations;

namespace ProjectFinally.Models.DTOs.Auth;

public class OAuthLoginRequestDto
{
    [Required(ErrorMessage = "El token de OAuth es requerido")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "El proveedor es requerido")]
    public string Provider { get; set; } = string.Empty; // "google" o "facebook"

    public string? Username { get; set; } // Opcional: el usuario puede elegir un username después
}
