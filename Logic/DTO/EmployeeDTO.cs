using System.Collections.Generic;
using ProjectManagement.Logic.Common;

namespace ProjectManagement.Logic.DTO
{
    class EmployeeFullDTOComparer : EqualityComparer<EmployeeFullDTO>
    {
        public override bool Equals(EmployeeFullDTO evm1, EmployeeFullDTO evm2)
        {
            if (evm1 == null && evm2 == null)
                return true;
            else if (evm1 == null || evm2 == null)
                return false;

            return (evm1.ID == evm2.ID);
        }

        public override int GetHashCode(EmployeeFullDTO evm)
        {
            return 0;
        }
    }

    public class EmployeeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
    }

    public class EmployeeFullDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public ICollection<ProjectSummaryDTO> Projects { get; set; }
    }
}
