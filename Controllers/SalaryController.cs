using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeInfo.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new List<Salary>());
        }

        [HttpGet]
        public async Task<IActionResult> GetSalaries(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            try 
            {
                var result = await _salaryService.GetSalariesPagedAsync(page, pageSize, searchTerm);
                return Json(new
                {
                    data = result.Data,
                    totalCount = result.TotalCount
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salary salary)
        {
            if (ModelState.IsValid)
            {
                await _salaryService.CreateSalaryAsync(salary);
                return RedirectToAction(nameof(Index));
            }
            return View(salary);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            return View(salary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _salaryService.UpdateSalaryAsync(salary);
                return RedirectToAction(nameof(Index));
            }
            return View(salary);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            return View(salary);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _salaryService.DeleteSalaryAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Cannot delete this salary record because it is assigned to one or more employees.";
            }
            else
            {
                TempData["SuccessMessage"] = "Salary record deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
