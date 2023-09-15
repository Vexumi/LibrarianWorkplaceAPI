using System.ComponentModel.DataAnnotations;

public class UserCredential
    {
    [Required]
    [StringLength(50)]
    public string UserName { get; set; }
    [StringLength(50)]
    public string Password { get; set; }
    }