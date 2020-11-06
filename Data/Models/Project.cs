using System;
using System.Collections.Generic;

namespace ProjectManagement.Data.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CustomerCompanyName { get; set; }
        public string ExecutorCompanyName { get; set; }
        public int? ProjectManagerId { get; set; }
        public virtual Manager ProjectManager { get; set; }
        public virtual ICollection<Developer> Developers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Priority { get; set; }

        public Project()
        {
            Developers = new List<Developer>();
        }
    }
}
