using System.Collections.Generic;

namespace ProjectManagement.Data.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
 
        public Person()
        {
        }
    }

    public class Developer : Person
    {
        public virtual ICollection<Project> Projects { get; set; }

        public Developer()
        {
            Projects = new List<Project>();
        }
    }

    public class Manager : Person
    {
        public virtual ICollection<Project> Projects { get; set; }

        public Manager()
        {
            Projects = new List<Project>();
        }
    }

}
