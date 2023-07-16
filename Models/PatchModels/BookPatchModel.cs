using LibrarianWorkplaceAPI.Models.PatchModels;
using System.ComponentModel.DataAnnotations;

public class BookPatchModel : PatchBaseModel
{
    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(50)]
    public string Author { get; set; }

    public DateTime ReleaseDate { get; set; }

    public int NumberOfCopies { get; set; }
}

