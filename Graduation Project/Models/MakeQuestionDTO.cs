namespace Graduation_Project.Models
{
    public class MakeQuestionDTO
    {
        public string subject {  get; set; }

        public string grade { get; set; }

        public string chapter { get; set; }

        public List<string> hints {  get; set; }

        public string answer { get; set; }

        public string summary { get; set; }
    }
}
