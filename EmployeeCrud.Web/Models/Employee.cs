using System.ComponentModel.DataAnnotations;

namespace EmployeeCrud.Web.Models;

public class Employee : IValidatableObject
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Employee Name")]
    public string EmployeeName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Department")]
    public string EmployeeDepartment { get; set; } = string.Empty;

    [Range(18, 70, ErrorMessage = "Age 18 se 70 ke darmiyan honi chahiye.")]
    [Display(Name = "Age")]
    public int EmployeeAge { get; set; }

    [Required, EmailAddress, StringLength(150)]
    [Display(Name = "Email")]
    public string EmployeeEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    [Display(Name = "Check In Time")]
    public DateTime CheckInTime { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Check Out Time")]
    public DateTime? CheckOutTime { get; set; }

    // Check-out, check-in se pehle nahi ho sakta
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckOutTime.HasValue && CheckOutTime.Value < CheckInTime)
        {
            yield return new ValidationResult(
                "Check Out time, Check In time se pehle nahi ho sakta.",
                new[] { nameof(CheckOutTime) });
        }
    }
}
