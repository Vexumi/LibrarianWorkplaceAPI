using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(50)]
    public string Author { get; set; }

    public uint VendorCode { get; set; }

    public DateTime ReleaseDate { get; set; }

    public int NumberOfCopies { get; set; }
}
