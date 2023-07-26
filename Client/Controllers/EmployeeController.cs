using API.DTOs.Employees;
using API.Models;
using API.Utilities;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        [Authorize(Roles = $"{nameof(RoleLevel.user)},{nameof(RoleLevel.admin)}")]
        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListEmployee = new List<GetEmployeeDto>();

            if (result.Data != null)
            {
                ListEmployee = result.Data.ToList();
            }
            return View(ListEmployee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GetEmployeeDto newEmploye)
        {

            var result = await repository.Post(newEmploye);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil masuk";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "409")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await repository.Delete(guid);

            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil dihapus";
            }
            else
            {
                TempData["Error"] = "Gagal menghapus data";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var result = await repository.Get(guid);

            if (result.Data?.Guid is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var employee = new GetEmployeeDto
            {
                Guid = result.Data.Guid,
                NIK = result.Data.NIK,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                BirthDate = result.Data.BirthDate,
                Gender = result.Data.Gender,
                HiringDate = result.Data.HiringDate,
                Email = result.Data.Email,
                PhoneNumber = result.Data.PhoneNumber
            };

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GetEmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            var result = await repository.Put(employee.Guid, employee);

            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil diubah";
            }
            else
            {
                TempData["Error"] = "Gagal mengubah data";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
