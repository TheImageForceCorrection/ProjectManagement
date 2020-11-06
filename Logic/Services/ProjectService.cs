using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using ProjectManagement.Logic.Interfaces;
using ProjectManagement.Logic.DTO;
using ProjectManagement.Logic.Common;
using ProjectManagement.Data.Models;
using ProjectManagement.Data.Interfaces;

namespace ProjectManagement.Logic.Services
{
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }

    public class ProjectService : IProjectService
    {
        IUnitOfWork UnitOfWork { get; set; }

        public ProjectService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        public IEnumerable<ProjectDTO> GetProjects()
        {
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeDTO>()
            ).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectDTO>()
                .ForMember("Developers", opt => opt.MapFrom(src => mapper2.Map<IEnumerable<Person>, List<EmployeeDTO>>(src.Developers)))
                .ForMember("ProjectManagerFullName", opt => opt.MapFrom(src => src.ProjectManager.Surname + " " + src.ProjectManager.Name))
                ).CreateMapper();

            var projectDtos = mapper.Map<IEnumerable<Project>, List<ProjectDTO>>(UnitOfWork.Projects.GetAll());

            return projectDtos;
        }
        public ProjectDTO GetProject(int id)
        {
            var project = UnitOfWork.Projects.Get(id);

            if (project == null)
                throw new ValidationException("Project with the current id does not exist", "id");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectDTO>()
                .ForMember("Developers", opt => opt.Ignore())
                ).CreateMapper();
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeDTO>()
                ).CreateMapper();

            var projectDTO = mapper.Map<Project, ProjectDTO>(project);
            projectDTO.Developers = mapper2.Map<IEnumerable<Person>, List<EmployeeDTO>>(project.Developers);
            projectDTO.ProjectManagerFullName = project.ProjectManager.Surname + " " + project.ProjectManager.Name + " " + project.ProjectManager.Patronymic;

            return projectDTO;
        }
        public void EditProject(ProjectDTO projectDto)
        {
            var projectManager = UnitOfWork.Managers.Get((int)projectDto.ProjectManagerId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, Project>()
                ).CreateMapper();

            var project = mapper.Map<ProjectDTO, Project>(projectDto);

            UnitOfWork.Projects.Update(project);
            UnitOfWork.Save();
            project.ProjectManager = null;
            UnitOfWork.Projects.Update(project);
            UnitOfWork.Save();

            project.ProjectManager = projectManager;
            UnitOfWork.Projects.Update(project);
            UnitOfWork.Save();
        }
        public void DeleteProject(int? id)
        {
            if (id == null)
                return;
            UnitOfWork.Projects.Delete((int)id);
            UnitOfWork.Save();
        }
        public void CreateProject(ProjectDTO projectDto)
        {
            if (projectDto.ProjectManagerId == null)
                return;
            var projectManager = UnitOfWork.Managers.Get((int)projectDto.ProjectManagerId);

            if (projectManager == null)
                return;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, Project>()
                ).CreateMapper();
            var createdProject = mapper.Map<ProjectDTO, Project>(projectDto);

            createdProject.ProjectManager = projectManager;

            UnitOfWork.Projects.Create(createdProject);
            UnitOfWork.Save();
        }

        public IEnumerable<EmployeeDTO> GetManagersSummary()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeDTO>()
            ).CreateMapper();
            return mapper.Map<IEnumerable<Person>, List<EmployeeDTO>>(UnitOfWork.Managers.GetAll());
        }

        public IEnumerable<EmployeeDTO> GetDevelopersSummary()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeDTO>()
            ).CreateMapper();
            return mapper.Map<IEnumerable<Person>, List<EmployeeDTO>>(UnitOfWork.Developers.GetAll());
        }

        private IEnumerable<EmployeeFullDTO> GetManagers()
        {
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectSummaryDTO>()
                ).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Manager, EmployeeFullDTO>()
                  .ForMember("Projects", opt => opt.MapFrom(src => mapper2.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(src.Projects)))
                  .ForMember("Role", opt => opt.MapFrom(src => Role.Manager))
                ).CreateMapper();
            return mapper.Map<IEnumerable<Manager>, List<EmployeeFullDTO>>(UnitOfWork.Managers.GetAll());
        }

        private IEnumerable<EmployeeFullDTO> GetDevelopers()
        {
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectSummaryDTO>()
                ).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Developer, EmployeeFullDTO>()
                  .ForMember("Projects", opt => opt.MapFrom(src => mapper2.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(src.Projects)))
                  .ForMember("Role", opt => opt.MapFrom(src => Role.Developer))
                ).CreateMapper();
            return mapper.Map<IEnumerable<Developer>, List<EmployeeFullDTO>>(UnitOfWork.Developers.GetAll());
        }

        private IEnumerable<EmployeeFullDTO> GetRestEmployees()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeFullDTO>()
                .ForMember("Role", opt => opt.MapFrom(src => Role.Employee))
                ).CreateMapper();
            return mapper.Map<IEnumerable<Person>, List<EmployeeFullDTO>>(UnitOfWork.Persons.GetAll());
        }

        public IEnumerable<EmployeeFullDTO> GetEmployees()
        {
            return GetManagers()
                .Concat(GetDevelopers()).
                Concat(GetRestEmployees())
                .Distinct(new EmployeeFullDTOComparer());
        }
        public EmployeeFullDTO GetEmployee(int id)
        {
            var employee = UnitOfWork.Persons.Get(id);

            if (employee == null)
                throw new ValidationException("Employee with the current id does not exist", "id");
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectSummaryDTO>()
                ).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Person, EmployeeFullDTO>()
                ).CreateMapper();
            var employeeDto = mapper.Map<Person, EmployeeFullDTO>(employee);
            if (employee is Manager manager)
            {
                employeeDto.Role = Role.Manager;
                employeeDto.Projects = mapper2.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(manager.Projects);
            }
            else if (employee is Developer developer)
            {
                employeeDto.Role = Role.Developer;
                employeeDto.Projects = mapper2.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(developer.Projects);
            }
            else
                employeeDto.Role = Role.Employee;

            return employeeDto;
        }
        public void EditEmployee(EmployeeFullDTO employeeDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, Person>()
                ).CreateMapper();

            var employee = mapper.Map<EmployeeFullDTO, Person>(employeeDto);

            UnitOfWork.Persons.Update(employee);
            UnitOfWork.Save();

        }
        public void DeleteEmployee(int id)
        {
            UnitOfWork.Persons.Delete(id);
            UnitOfWork.Save();

        }
        public void CreateEmployee(EmployeeFullDTO employee)
        {
            switch (employee.Role)
            {
                case Role.Manager:
                    var managerMapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, Manager>()
                        ).CreateMapper();
                    var createdManager = managerMapper.Map<EmployeeFullDTO, Manager>(employee);
                    UnitOfWork.Managers.Create(createdManager);
                    break;

                case Role.Developer:
                    var developerMapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, Developer>()
                        ).CreateMapper();
                    var createdDeveloper = developerMapper.Map<EmployeeFullDTO, Developer>(employee);
                    UnitOfWork.Developers.Create(createdDeveloper);
                    break;

                case Role.Employee:
                    var employeeMapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, Person>()
                        ).CreateMapper();
                    var createdEmployee = employeeMapper.Map<EmployeeFullDTO, Person>(employee);

                    UnitOfWork.Persons.Create(createdEmployee);
                    break;
            }
            UnitOfWork.Save();
        }
        public void AssignEmployeeOnProject(int employeeID, int projectID)
        {
            var project = UnitOfWork.Projects.Get(projectID);
            var developer = UnitOfWork.Developers.Get(employeeID);
            if (project != null || developer != null)
            {
                developer.Projects.Add(project);  
                UnitOfWork.Save();
            }
        }
        public void RemoveEmployeeFromProject(int employeeID, int projectID)
        {
            var project = UnitOfWork.Projects.Get(projectID);
            var developer = UnitOfWork.Developers.Get(employeeID);
            if (project != null || developer != null)
            {
                developer.Projects.Remove(project);
                UnitOfWork.Save();
            }
        }
        public IEnumerable<ProjectSummaryDTO> GetProjectsSummary()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectSummaryDTO>()
                ).CreateMapper();

            return mapper.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(UnitOfWork.Projects.GetAll());
        }
        public IEnumerable<ProjectSummaryDTO> GetLinkedProjects(int managerId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Project, ProjectSummaryDTO>()
                ).CreateMapper();

            var linkedProjects= UnitOfWork.Projects.GetAll()
                .Where(
                    project => project.ProjectManager == null
                    ? false
                    : project.ProjectManager.ID == managerId
                ).ToList();

            return mapper.Map<IEnumerable<Project>, List<ProjectSummaryDTO>>(linkedProjects);
        }
    }
}
