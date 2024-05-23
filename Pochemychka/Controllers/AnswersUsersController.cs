using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pochemychka.Models;
using Pochemychka.ViewModels;

namespace Pochemychka.Controllers
{
    public class AnswersUsersController : Controller
    {
        private readonly PochemychkaContext _context;

        public AnswersUsersController(PochemychkaContext context)
        {
            _context = context;
        }


        // GET: AnswersUsers
        public async Task<IActionResult> Index(string? user, int? test, int? startPoints, int? endPoints)
        {
          
            var i = User.Identity.Name;
            var idUse = _context.Users.FirstOrDefault(u => u.UserName == i).Id;
            if (User.IsInRole("admin"))
            {

                var tests = await _context.Tests.ToListAsync();

                // Создаем список объектов SelectListItem для заполнения выпадающего списка
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "-1", Text = "все" });
               selectListItems.AddRange(tests.Select(c => new SelectListItem
                {
                    Value = c.IdTest.ToString(), // Здесь должно быть строковое значение идентификатора категории
                    Text = c.NameTest // Здесь должно быть название категории
                }).ToList());
                ViewBag.Tests = selectListItems;
                var users = await _context.Users.ToListAsync();

                List<SelectListItem> selectListItems2 = new List<SelectListItem>();
                selectListItems2.Add(new SelectListItem { Value = "-1", Text = "все" });
                // Создаем список объектов SelectListItem для заполнения выпадающего списка
                selectListItems2.AddRange(users.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(), // Здесь должно быть строковое значение идентификатора категории
                    Text = c.LastName +' '+c.FirstName // Здесь должно быть название категории
                }).ToList());
                ViewBag.users = selectListItems2;

                startPoints = startPoints == null ? 0 : startPoints;
                endPoints = endPoints == null ? 1000000000 : endPoints;
                if (startPoints > endPoints)
                {
                    int y = (int)startPoints;
                    startPoints = endPoints;
                    endPoints = y;
                }
                var a = await _context.AnswerUserView.Where(a=>a.CountFullPoints<= endPoints && a.CountFullPoints>= startPoints)
                 .ToListAsync();
                if (user != null && user != "-1") a = a.Where(a => a.IdUser == user).ToList();
                if (test != null && test!=-1) a = a.Where(a => a.IdTest == test).ToList();
                return View( a);
            }
            else
                return View(await _context.AnswerUserView.Where(u=>u.IdUser==idUse).ToListAsync());

        }
        
        public async Task<FileResult> ExportInExcel()
        {
            var result = new List<AnswersUserViewModel>();
            result = _context.AnswerUserView.ToList();
            var fileName = $"Result.xlsx";
            return GenerateExcel(fileName, result);

        }

        private FileResult GenerateExcel(string fileName, List<AnswersUserViewModel> result)
        {
            DataTable dataTable = new DataTable("Result");
            
                dataTable.Columns.Add(new DataColumn("Пользователь"));
            dataTable.Columns.Add(new DataColumn("Тест"));
            dataTable.Columns.Add(new DataColumn("Баллы"));
            dataTable.Columns.Add(new DataColumn("Результат"));
            dataTable.Columns.Add(new DataColumn("Дата и время прохождения"));

            for (int i = 0; i < result.Count(); i++)
            {
                
                var dataRow = dataTable.NewRow();
                dataRow["Пользователь"] = result[i].LastName +" " + result[i].FirstName;
                dataRow["Тест"] = result[i].NameTest;
                dataRow["Баллы"] = result[i].CountFullPoints;
                dataRow["Результат"] = result[i].TextResult;
                dataRow["Дата и время прохождения"] = result[i].Time.ToString("dd.MM.yyyy HH:mm");
                dataTable.Rows.Add(dataRow);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }
        // GET: AnswersUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnswersUsers == null)
            {
                return NotFound();
            }

            var answersUser = await _context.AnswersUsers
                .FirstOrDefaultAsync(m => m.IdAnswersUser == id);
            if (answersUser == null)
            {
                return NotFound();
            }

            return View(answersUser);
        }

        // GET: AnswersUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnswersUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAnswersUser,IdUser,CountCurrent,Time")] AnswersUser answersUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answersUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answersUser);
        }

        // GET: AnswersUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AnswersUsers == null)
            {
                return NotFound();
            }

            var answersUser = await _context.AnswersUsers.FindAsync(id);
            if (answersUser == null)
            {
                return NotFound();
            }
            return View(answersUser);
        }

        // POST: AnswersUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAnswersUser,IdUser,CountCurrent,Time")] AnswersUser answersUser)
        {
            if (id != answersUser.IdAnswersUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answersUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswersUserExists(answersUser.IdAnswersUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(answersUser);
        }

        // GET: AnswersUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AnswersUsers == null)
            {
                return NotFound();
            }

            var answersUser = await _context.AnswersUsers
                .FirstOrDefaultAsync(m => m.IdAnswersUser == id);
            if (answersUser == null)
            {
                return NotFound();
            }

            return View(answersUser);
        }

        // POST: AnswersUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnswersUsers == null)
            {
                return Problem("Entity set 'SerovaContext.AnswersUsers'  is null.");
            }
            var answersUser = await _context.AnswersUsers.FindAsync(id);
            if (answersUser != null)
            {
                _context.AnswersUsers.Remove(answersUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswersUserExists(int id)
        {
            return (_context.AnswersUsers?.Any(e => e.IdAnswersUser == id)).GetValueOrDefault();
        }
    }
}