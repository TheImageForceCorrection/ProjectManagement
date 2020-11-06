using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using AutoMapper;
using ProjectManagement.Presentation.Models;
using ProjectManagement.Logic.Interfaces;
using ProjectManagement.Logic.Services;
using ProjectManagement.Logic.DTO;
using ProjectManagement.Data.Wrappers;

namespace ProjectManagement.Presentation.Controllers
{
    [Route("[controller]")]
    public class ProjectsController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private IProjectService projectService;

        public ProjectsController()
        {
            this.unitOfWork = new UnitOfWork();
            projectService = new ProjectService(unitOfWork);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var projectDtos = projectService.GetProjects();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, ProjectSummaryViewModel>()
                ).CreateMapper();

            return View(mapper.Map<IEnumerable<ProjectDTO>, List<ProjectSummaryViewModel>>(projectDtos));
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Project(int id)
        {
            try
            {
                var projectDto = projectService.GetProject(id);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, ProjectDetailsViewModel>()
                    .ForMember("Developers",opt=>opt.Ignore())
                    ).CreateMapper();

                var project = mapper.Map<ProjectDTO, ProjectDetailsViewModel>(projectDto);
                project.Developers = projectDto.Developers
                    .Select(dev => new ProjectEmployeeViewModel()
                    {
                        ID = dev.ID,
                        FullName = dev.Surname + " " + dev.Name + " " + dev.Patronymic
                    });

                ViewBag.Developers = EmployeeSelectListCreation(projectService.GetDevelopersSummary().Except(projectDto.Developers));

                return View(project);
            }
            catch(ValidationException)
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
                var projectDto = projectService.GetProject(id);
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProjectDTO, EditProjectViewModel>()
                    .ForMember("OldName", opt => opt.MapFrom(src=>src.Name))
                    ).CreateMapper();

                var project = mapper.Map<ProjectDTO, EditProjectViewModel>(projectDto);

                ViewBag.Managers = EmployeeSelectListCreation(projectService.GetManagersSummary());
                return View(project);
            }
            catch(ValidationException)
            {
                return NotFound();
            }
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(EditProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditProjectViewModel, ProjectDTO>()
                    ).CreateMapper();

                var projectDto = mapper.Map<EditProjectViewModel, ProjectDTO>(project);

                projectService.EditProject(projectDto);

                return RedirectToAction("Project", new { id = project.ID });
            }

            ViewBag.Managers = EmployeeSelectListCreation(projectService.GetManagersSummary());
            return View(project);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var projectDto = projectService.GetProject(id);
                return View(new DeleteProjectViewModel()
                {
                    ID = id,
                    Name = projectDto.Name
                });
            }
            catch(ValidationException)
            {
                return NotFound();
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(DeleteProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                projectService.DeleteProject(project.ID);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        private SelectList EmployeeSelectListCreation(IEnumerable<EmployeeDTO> employeeDTOs)
        {
            return new SelectList( employeeDTOs
                .Select(
                manager => new
                {
                    manager.ID,
                    FullName = manager.Surname + " " + manager.Name + " " + manager.Patronymic
                })
                , "ID"
                , "FullName");
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Managers = EmployeeSelectListCreation(projectService.GetManagersSummary());
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(CreateProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CreateProjectViewModel, ProjectDTO>()).CreateMapper();
                var projectDto = mapper.Map<CreateProjectViewModel, ProjectDTO>(project);

                projectService.CreateProject(projectDto);
                return RedirectToAction("Index");
            }

            ViewBag.Managers = EmployeeSelectListCreation(projectService.GetManagersSummary());
            return View(project);
        }
    }
}
