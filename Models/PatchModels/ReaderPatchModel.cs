using LibrarianWorkplaceAPI.Models.PatchModels;
using System.ComponentModel.DataAnnotations;

public class ReaderPatchModel: PatchBaseModel
{
    [StringLength(50)]
    public string FullName { get; set; }

    public DateTime DateOfBirth { get; set; }
}

