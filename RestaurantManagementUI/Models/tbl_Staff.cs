using System.ComponentModel.DataAnnotations;

public class tbl_Staff
{
    public int StaffID { get; set; }

    [Required(ErrorMessage = "Staff Name is required.")]
    [StringLength(100, ErrorMessage = "Staff Name cannot exceed 100 characters.")]
    public string? StaffName { get; set; }

    [Required(ErrorMessage = "Staff Phone is required.")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Enter a valid phone number (10–15 digits).")]
    public string? StaffPhone { get; set; }

    [Required(ErrorMessage = "Role ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role.")]
    public int RoleID { get; set; }

    [Required(ErrorMessage = "Staff Role is required.")]
    [StringLength(50, ErrorMessage = "Staff Role cannot exceed 50 characters.")]
    public string? StaffRole { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(200, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, and a number.")]
    public string? UserPassword { get; set; }
}
