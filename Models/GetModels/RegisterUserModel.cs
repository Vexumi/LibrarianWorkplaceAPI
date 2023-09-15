using System.ComponentModel.DataAnnotations;

namespace LibrarianWorkplaceAPI.Models.GetModels
{
    public class RegisterUserModel
    {
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
}
