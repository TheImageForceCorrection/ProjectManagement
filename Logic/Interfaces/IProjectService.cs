using System.Collections.Generic;
using ProjectManagement.Logic.DTO;

namespace ProjectManagement.Logic.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectDTO> GetProjects();
        ProjectDTO GetProject(int id);
        void EditProject(ProjectDTO project);
        void DeleteProject(int? id);
        void CreateProject(ProjectDTO project);
        IEnumerable<EmployeeDTO> GetManagersSummary();
        public IEnumerable<EmployeeDTO> GetDevelopersSummary();
        IEnumerable<EmployeeFullDTO> GetEmployees();
        EmployeeFullDTO GetEmployee(int id);
        void EditEmployee(EmployeeFullDTO employeeDto);
        void DeleteEmployee(int id);
        void CreateEmployee(EmployeeFullDTO employeeDto);
        void AssignEmployeeOnProject(int employeeID, int projectID);
        void RemoveEmployeeFromProject(int employeeID, int projectID);
        public IEnumerable<ProjectSummaryDTO> GetProjectsSummary();
        public IEnumerable<ProjectSummaryDTO> GetLinkedProjects(int managerId);
    }
}
