namespace ExamFerid.Areas.Manage.ViewModels
{
    public class UpdateVM
    {
        public IFormFile? Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
