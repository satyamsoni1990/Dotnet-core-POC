using System.ComponentModel.DataAnnotations;
namespace webapi1.Dtos

{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Password length should be between 4 to 8 ")]
        public string Password { get; set; }
    }
}