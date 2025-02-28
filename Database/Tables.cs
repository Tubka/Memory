using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    public UserSettings Settings { get; set; }
}

public class Company
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<MonthlyCheckbox> MonthlyCheckboxes { get; set; }
}

public class ActiveCompany
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }
}

public class CheckboxTemplate
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string TemplateName { get; set; }

    [Required]
    public int CompanyId { get; set; }

    public int Order { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }
   
    public ICollection<MonthlyCheckbox> MonthlyCheckboxes { get; set; }
}

public class MonthlyCheckbox
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int TemplateId { get; set; }

    public bool IsChecked { get; set; } = false;

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    [ForeignKey("TemplateId")]
    public CheckboxTemplate CheckboxTemplate { get; set; }
}

public class Description
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}

public class UserSettings
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public ICollection<OrderCheckbox> OrderCheckboxes { get; set; } = new List<OrderCheckbox>();
}

public class OrderCheckbox
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int CheckboxTemplateId { get; set; }

    [Required]
    public int Order { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
public class MonthDescription
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}

public class FuelDescription
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}