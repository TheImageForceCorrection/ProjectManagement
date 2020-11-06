using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using ProjectManagement.Presentation.Models;
using ProjectManagement.Logic.Interfaces;
using ProjectManagement.Logic.Services;
using ProjectManagement.Logic.DTO;
using ProjectManagement.Logic.Common;
using ProjectManagement.Data.Wrappers;

namespace ProjectManagement.Presentation.Utils
{
    public static class UICommons
    {
        public static Dictionary<Role, string> rolePairs;
        static UICommons()
        {
            rolePairs = new Dictionary<Role, string>()
            {
                { Role.Manager, "Руководитель проекта" },
                { Role.Developer, "Разработчик" },
                { Role.Employee, "Сотрудник" }
            };
        }
    };

    class EmployeeViewModelComparer : EqualityComparer<EmployeeViewModel>
    {
        public override bool Equals(EmployeeViewModel evm1, EmployeeViewModel evm2)
        {
            if (evm1 == null && evm2 == null)
                return true;
            else if (evm1 == null || evm2 == null)
                return false;

            return (evm1.ID == evm2.ID);
        }

        public override int GetHashCode(EmployeeViewModel evm)
        {
            return 0;
        }
    }
}

namespace ProjectManagement.Presentation.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private IProjectService projectService;
        private static Dictionary<Role, string> rolePairs;

        static EmployeeController()
        {
            rolePairs = new Dictionary<Role, string>();
            rolePairs.Add(Role.Manager, "Руководитель проекта");
            rolePairs.Add(Role.Developer, "Разработчик");
            rolePairs.Add(Role.Employee, "Сотрудник");
        }

        public EmployeeController()
        {
            unitOfWork = new UnitOfWork();
            projectService = new ProjectService(unitOfWork);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employeeDtos = projectService.GetEmployees();
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<ProjectSummaryDTO, EmployeeProjectViewModel>()
                ).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, EmployeeViewModel>()
                  .ForMember("Projects", opt => opt.MapFrom(src => mapper2.Map<IEnumerable<ProjectSummaryDTO>, List<EmployeeProjectViewModel>>(src.Projects)))
                  .ForMember("FullName", opt => opt.MapFrom(src => src.Surname + " " + src.Name + " " + src.Patronymic))
            ).CreateMapper();

            var employees = mapper.Map<IEnumerable<EmployeeFullDTO>, List<EmployeeViewModel>>(employeeDtos);

            return View(employees);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Employee(int id)
        {
            try
            {
                var employeeDto = projectService.GetEmployee(id);

                var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<ProjectSummaryDTO, EmployeeProjectViewModel>()
                    ).CreateMapper();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, EmployeeDetailsViewModel>()
                    .ForMember("Projects", opt => opt.MapFrom(src => mapper2.Map<IEnumerable<ProjectSummaryDTO>, List<EmployeeProjectViewModel>>(src.Projects)))
                    .ForMember("FullName", opt => opt.MapFrom(src => src.Surname + " " + src.Name + " " + src.Patronymic))
                    ).CreateMapper();

                var employee = mapper.Map<EmployeeFullDTO, EmployeeDetailsViewModel>(employeeDto);

                if (employee.Role == Role.Developer)
                {
                    SelectList projects = new SelectList(
                        projectService.GetProjectsSummary()
                        .Select(project=>new EmployeeProjectViewModel()
                        { 
                            ID=project.ID,
                            Name=project.Name
                        })
                        .Except(employee.Projects)
                        , "ID"
                        , "Name");
                    ViewBag.Projects = projects;
                }

                return View(employee);
            }
            catch (ValidationException)
            {
                return NotFound();
            }
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var employeeDto = projectService.GetEmployee(id);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, EditEmployeeViewModel>()
                    ).CreateMapper();

                var employee = mapper.Map<EmployeeFullDTO, EditEmployeeViewModel>(employeeDto);

                return View(employee);
            }
            catch (ValidationException)
            {
                return NotFound();
            }
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(EditEmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditEmployeeViewModel, EmployeeFullDTO>()
                    ).CreateMapper();

                var employeeDto = mapper.Map<EditEmployeeViewModel, EmployeeFullDTO>(employee);

                projectService.EditEmployee(employeeDto);

                return RedirectToAction("Employee", new { id = employee.ID });
            }

            return View(employee);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var employeeDto = projectService.GetEmployee(id);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeFullDTO, DeleteEmployeeViewModel>()
                    .ForMember("FullName", opt => opt.MapFrom(src => src.Surname + " " + src.Name + " " + src.Patronymic))
                    .ForMember("LinkedProjects", opt=>opt.Ignore())
                    ).CreateMapper();

                var employee = mapper.Map<EmployeeFullDTO, DeleteEmployeeViewModel>(employeeDto);
                if (employeeDto.Role == Role.Manager)
                {
                    var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<ProjectSummaryDTO, EmployeeProjectViewModel>()
                        ).CreateMapper();
                    var linkedProjects = projectService.GetLinkedProjects(id);

                    employee.LinkedProjects = 
                    mapper2.Map<IEnumerable<ProjectSummaryDTO>, List<EmployeeProjectViewModel>>(linkedProjects);        
                }
                else
                    employee.LinkedProjects = new List<EmployeeProjectViewModel>();
                return View(employee);
            }
            catch (ValidationException)
            {
                return NotFound();
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(DeleteEmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                projectService.DeleteEmployee(employee.ID);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            SelectList roles = new SelectList(rolePairs
                .Select(pair => new 
                { 
                    ID = pair.Key, 
                    Name = pair.Value 
                })
                , "ID"
                , "Name");
            ViewBag.Roles = roles;
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(CreateEmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CreateEmployeeViewModel, EmployeeFullDTO>()
                    .ForMember("ID", opt => opt.MapFrom(src=>0))
                    .ForMember("Projects", opt => opt.Ignore())
                    ).CreateMapper();

                var employeeDto = mapper.Map<CreateEmployeeViewModel, EmployeeFullDTO>(employee);

                projectService.CreateEmployee(employeeDto);

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [Route("AssignOnProject")]
        [HttpPost]
        public IActionResult AssignOnProject(EmployeeProjectRelationChange relation)
        {
            if (ModelState.IsValid)
            {
                projectService.AssignEmployeeOnProject((int)relation.employeeID, (int)relation.projectID);

                if ((bool)relation.isFromProject)
                    return RedirectToAction("Project", "Projects", new { id = relation.projectID });

                return RedirectToAction("Employee", new { id = relation.employeeID });
            }

            return RedirectToAction("Index");
        }

        [Route("RemoveFromProject")]
        [HttpPost]
        public IActionResult RemoveFromProject(EmployeeProjectRelationChange relation)
        {
            if (ModelState.IsValid)
            {
                projectService.RemoveEmployeeFromProject((int)relation.employeeID, (int)relation.projectID);

                if ((bool)relation.isFromProject)
                    return RedirectToAction("Project", "Projects", new { id = relation.projectID });

                return RedirectToAction("Employee", new { id = relation.employeeID });
            }
                return RedirectToAction("Index");
        }
    }
}
