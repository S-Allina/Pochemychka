using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pochemychka.Models;

namespace Pochemychka.Controllers
{
    public class ResultDiapasonsController : Controller
    {
        private readonly PochemychkaContext _context;

        public ResultDiapasonsController(PochemychkaContext context)
        {
            _context = context;
        }

        // GET: ResultDiapasons
        public async Task<IActionResult> Index()
        {
              return _context.ResultDiapason != null ? 
                          View(await _context.ResultDiapason.ToListAsync()) :
                          Problem("Entity set 'PochemychkaContext.ResultDiapason'  is null.");
        }

        // GET: ResultDiapasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ResultDiapason == null)
            {
                return NotFound();
            }

            var resultDiapason = await _context.ResultDiapason
                .FirstOrDefaultAsync(m => m.IdDiapasonResult == id);
            if (resultDiapason == null)
            {
                return NotFound();
            }

            return View(resultDiapason);
        }

        // GET: ResultDiapasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResultDiapasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? idT,int StartDiapason,int EndDiapason, string TextResult,string? returnAction)
        {

            if (StartDiapason > EndDiapason)
            {
                var t = StartDiapason;
                StartDiapason = EndDiapason;
                EndDiapason = t;
            }
                if (idT == null)
                {
                    idT = _context.Tests.OrderBy(t => t.IdTest).Last().IdTest;
                var isUnique = !_context.ResultDiapason
    .Any(d => d.IdTest == idT &&
        ((d.StartDiapason >= StartDiapason && d.StartDiapason <= EndDiapason) ||
         (d.EndDiapason >= StartDiapason && d.EndDiapason <= EndDiapason) ||
         (StartDiapason >= d.StartDiapason && StartDiapason <= d.EndDiapason) ||
         (EndDiapason >= d.StartDiapason && EndDiapason <= d.EndDiapason)));
                if (!isUnique) return RedirectToAction("Create", "Tests", new { idT = idT, isCreated = true, message = "Диапазоны оценивания пересекаются." });


                ResultDiapason resultDiapason = new ResultDiapason
                    {
                        StartDiapason = StartDiapason,
                        EndDiapason = EndDiapason,
                        TextResult = TextResult,
                        IdTest = (int)idT
                    };
                    _context.Add(resultDiapason);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Create", "Tests", new { isCreatedResult = true, isCreated = true });
                }
                else
                {
                var isUnique = !_context.ResultDiapason
        .Any(d => d.IdTest == idT &&
            ((d.StartDiapason >= StartDiapason && d.StartDiapason <= EndDiapason) ||
             (d.EndDiapason >= StartDiapason && d.EndDiapason <= EndDiapason) ||
             (StartDiapason >= d.StartDiapason && StartDiapason <= d.EndDiapason) ||
             (EndDiapason >= d.StartDiapason && EndDiapason <= d.EndDiapason)));
                if(!isUnique) return RedirectToAction("Edit", "Tests", new { idT = idT, isCreated = true, message = "Диапазоны оценивания пересекаются." });
                ResultDiapason resultDiapason = new ResultDiapason
                    {
                        StartDiapason = StartDiapason,
                        EndDiapason = EndDiapason,
                        TextResult = TextResult,
                        IdTest = (int)idT
                    };
                    _context.Add(resultDiapason);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(returnAction, "Tests", new {idT, isCreatedResult = true, isCreated = true });
                
            }
        }

        // GET: ResultDiapasons/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.ResultDiapason == null)
        //    {
        //        return NotFound();
        //    }

        //    var resultDiapason = await _context.ResultDiapason.FindAsync(id);
        //    if (resultDiapason == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(resultDiapason);
        //}

        // POST: ResultDiapasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDiapasonResult,IdTest,StartDiapason,EndDiapason,TextResult")] ResultDiapason resultDiapason)
        {
            if (id != resultDiapason.IdDiapasonResult)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultDiapason);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultDiapasonExists(resultDiapason.IdDiapasonResult))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Tests", new {idT=resultDiapason.IdTest, isCreated = true });
            }
            return RedirectToAction("Edit", "Tests", new { idT = resultDiapason.IdTest, isCreated = true, message = "некорректное название теста. Название должно содержать от 4 до 50 симвотов." });

        }

        //// GET: ResultDiapasons/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.ResultDiapason == null)
        //    {
        //        return NotFound();
        //    }

        //    var resultDiapason = await _context.ResultDiapason
        //        .FirstOrDefaultAsync(m => m.IdDiapasonResult == id);
        //    if (resultDiapason == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(resultDiapason);
        //}

        // POST: ResultDiapasons/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.ResultDiapason == null)
            {
                return Problem("Entity set 'PochemychkaContext.ResultDiapason'  is null.");
            }
            var resultDiapason = await _context.ResultDiapason.FindAsync(id);
            if (resultDiapason != null)
            {
                _context.ResultDiapason.Remove(resultDiapason);
            }
            
            await _context.SaveChangesAsync();
            bool i = _context.ResultDiapason.Where(r => r.IdTest == resultDiapason.IdTest).Count() > 0;
            return RedirectToAction("Edit", "Tests", new { idT = resultDiapason.IdTest, isCreated = true, isCreatedResult=i });
        }

        private bool ResultDiapasonExists(int id)
        {
          return (_context.ResultDiapason?.Any(e => e.IdDiapasonResult == id)).GetValueOrDefault();
        }
    }
}
