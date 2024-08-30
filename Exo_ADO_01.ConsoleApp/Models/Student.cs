namespace Exo_ADO_01.ConsoleApp.Models
{
    public class Student
    {
        public Student(int id, string firstname, string lastname, DateTime birthDate, int yearResult, int sectionId)
            :this(firstname, lastname, birthDate, yearResult, sectionId)
        {
            Id = id;
        }
        public Student(string firstname, string lastname, DateTime birthDate, int yearResult, int sectionId)
        {
            Id = -1;
            Firstname = firstname;
            Lastname = lastname;
            BirthDate = birthDate;
            YearResult = yearResult;
            SectionId = sectionId;
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public int YearResult { get; set; }
        public int SectionId { get; set; } 
    }
}
