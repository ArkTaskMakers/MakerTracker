using System.ComponentModel.DataAnnotations;

namespace MakerTracker.Models
{
    public class UserProfileViewModel
    {
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public string ProfileImage { get; set; }
    }
}