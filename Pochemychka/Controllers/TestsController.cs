using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Pochemychka.Models;
using Pochemychka.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Pochemychka.Controllers
{
    public class TestsController : Controller
    {
        private readonly PochemychkaContext _context;

        public TestsController(PochemychkaContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index(string? TextSearch, string? message)
        {
            var o = User.IsInRole("admin");
            var o1 = User.IsInRole("user");
            ViewBag.message = message;
            if (TextSearch == null)
            {
                return View(await _context.Tests.ToListAsync());
            }
            return View(await _context.Tests.Where(t=>t.NameTest.ToLower().Contains(TextSearch.ToLower())).ToListAsync());
            
        }


        // GET: Tests/Create
        public IActionResult Create(string? message, bool? isCreatedResult, bool? isCreated)
        {
            ViewBag.isCreated = isCreated;
            ViewBag.isCreatedResult = isCreatedResult;
            ViewBag.message=message;
            if (isCreated==true) {
              var   test = _context.Tests.OrderBy(t => t.IdTest).Last();
                TestViewModel testViewModel = new TestViewModel
                {
                    IdTest = test.IdTest,
                    NameTest = test.NameTest,
                    ResultDiapasons = _context.ResultDiapason.Where(r=>r.IdTest==test.IdTest).ToList()
                };
                //test.ResultDiapasons=_context.ResultDiapason.Where(r=>r.IdTest==test.IdTest).ToList();
                return View(testViewModel);
            }
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameTest")] TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                Test test = new Test
                {
                    NameTest = testViewModel.NameTest
                };
                _context.Add(test);
                await _context.SaveChangesAsync();
               
                return RedirectToAction("Create",new { isCreated = true});
            }
            return RedirectToAction("Create", new { isCreated = true, message = "некорректное название теста. Название должно содержать от 4 до 50 симвотов." });
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int idT,string? message, bool? isCreatedResult, bool? isCreated)
        {
            if (idT == null || _context.Tests == null)
            {
                return NotFound();
            }

            ViewBag.isCreated = isCreated;
            ViewBag.isCreatedResult = isCreatedResult;
            ViewBag.message = message;
                var test = _context.Tests.FirstOrDefault(t=>t.IdTest==idT);
                TestViewModel testViewModel = new TestViewModel
                {
                    IdTest = test.IdTest,
                    NameTest = test.NameTest,
                    ResultDiapasons = _context.ResultDiapason.Where(r => r.IdTest == test.IdTest).ToList()
                };
                //test.ResultDiapasons=_context.ResultDiapason.Where(r=>r.IdTest==test.IdTest).ToList();
                return View(testViewModel);
            
            return View();
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTest,NameTest")] TestViewModel testViewModel)
        {
          
            if (ModelState.IsValid)
            {
                Test test = new Test
                {
                    NameTest = testViewModel.NameTest,
                    IdTest = testViewModel.IdTest,
                };
                try
                {
                  
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.IdTest))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", new {idT=test.IdTest, isCreated = true });
            }
            return RedirectToAction("Edit", new { idT = testViewModel.IdTest, isCreated = true, message = "некорректное название теста. Название должно содержать от 4 до 50 симвотов." });
        }

        // GET: Tests/Delete/5
        //public async Task<IActionResult> Delete(int? id, string? email, string name, bool? isAdmin, int idUse)
        //{
        //    if (id == null || _context.Tests == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.UserEmail = email;
        //    ViewBag.UserName = name;
        //    ViewBag.isAdmin = isAdmin;
        //    ViewBag.idUse = idUse;
        //    var test = await _context.Tests
        //        .FirstOrDefaultAsync(m => m.IdTest == id);
        //    if (test == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(test);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Tests == null)
            {
                return Problem("Тестов нет");
            }
            var test = await _context.Tests.FindAsync(id);
            if (test != null)
            {
                _context.Tests.Remove(test);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
          return (_context.Tests?.Any(e => e.IdTest == id)).GetValueOrDefault();
        }
    }
}
