using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Employee Employee { get; set; }
        public EmployeesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Update or Create Get Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Upsert(int? id)
        {
            Employee = new Employee();
            if (id == null)
            {
                //create request
                return View(Employee);
            }
            //update
            Employee = _db.Employees.FirstOrDefault(u => u.Id == id);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Employees.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeFromDb = await _db.Employees.FirstOrDefaultAsync(u => u.Id == id);
            if (employeeFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Employees.Remove(employeeFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}