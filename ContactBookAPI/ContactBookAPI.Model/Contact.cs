using System.ComponentModel.DataAnnotations;

namespace ContactBookAPI.Model
{
    public class Contact
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$", 
        ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 3, 
        ErrorMessage = "First name must be between 3 and 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 3, 
        ErrorMessage = "Last name must be between 3 and 50 characters")]
        public string LastName { get; set; }

        public string PhotoURl { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public Address Address { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
//using System.ComponentModel.DataAnnotations;

//namespace ContactBookAPI.Model
//{
//    public class Contact
//    {
//        public int Id { get; set; }
//        [Required]
//        public string Email { get; set; }
//        [Required]
//        public string FirstName { get; set; }
//        [Required]
//        public string LastName { get; set; }
//        public string PhotoURl { get; set; }
//        [Required]
//        public Address Address { get; set; }
//        public string UserId { get; set; } 
//        public AppUser AppUser { get; set; }
//    }
//}
