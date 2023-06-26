using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookModel
{
    [Key]
    public int VendorCode { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(50)]
    public string Author { get; set; }

    public DateTime ReleaseDate { get; set; }

    public int NumberOfCopies { get; set; }
    // Список id читателей, которым выдали книги
    public List<int>? Readers { get; set; }
}
