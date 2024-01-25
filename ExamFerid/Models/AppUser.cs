using Microsoft.AspNetCore.Identity;

namespace ExamFerid.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
