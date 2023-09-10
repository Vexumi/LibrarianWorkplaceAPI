using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string UserName { get; set; }
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(50)]
    public string LibraryName { get; set; }
    [StringLength(50)]
    public string Address { get; set; }
}

