using System.ComponentModel.DataAnnotations;

public class ReaderModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FullName { get; set; }

    public DateTime DateOfBirth { get; set; }
    // Список из id книг, выданных пользователю
    public List<int>? Books { get; set; }
}

