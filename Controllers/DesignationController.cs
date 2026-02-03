using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInfo.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new List<EmployeeDesignation>());
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var result = await _designationService.GetDesignationsPagedAsync(page, pageSize, searchTerm);
            return Json(new
            {
                data = result.Data,
                totalCount = result.TotalCount
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDesignation designation)
        {
            if (ModelState.IsValid)
            {
                await _designationService.CreateDesignationAsync(designation);
                return RedirectToAction(nameof(Index));
            }
            return View(designation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var designation = await _designationService.GetDesignationByIdAsync(id);
            if (designation == null)
            {
                return NotFound();
            }
            return View(designation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDesignation designation)
        {
            if (id != designation.DesignationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _designationService.UpdateDesignationAsync(designation);
                return RedirectToAction(nameof(Index));
            }
            return View(designation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _designationService.DeleteDesignationAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Cannot delete this designation because it is assigned to one or more employees.";
            }
            else
            {
                TempData["SuccessMessage"] = "Designation deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
