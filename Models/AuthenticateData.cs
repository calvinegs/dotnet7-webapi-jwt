using System.ComponentModel.DataAnnotations;

namespace dotnet7_webapi_jwt.Data;

public class Response
{
    public string? Status { get; set; }
    public string? Message { get; set; }
}

public class LoginModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Eamil Address is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

public class RegisterModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email Address is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}