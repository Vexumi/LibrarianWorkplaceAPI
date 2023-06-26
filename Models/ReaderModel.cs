using System.ComponentModel.DataAnnotations;

public class ReaderModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FullName { get; set; }

    public DateTime DateOfBirth { get; set; }
}

