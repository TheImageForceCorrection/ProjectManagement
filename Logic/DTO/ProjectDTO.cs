using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Logic.DTO
{
    public class ProjectDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CustomerCompanyName { get; set; }
        public string ExecutorCompanyName { get; set; }
        public int? ProjectManagerId { get; set; }
        public string ProjectManagerFullName { get; set; }
        public virtual ICollection<EmployeeDTO> Developers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Priority { get; set; }
    }

    public class ProjectSummaryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
