using System.ComponentModel.DataAnnotations;

public class ReaderGetModel
{
    [Required]
    [StringLength(50)]
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string LibraryName { get; set; }

}