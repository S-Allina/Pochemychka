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

    public class QuestionsController : Controller
    {
        private readonly PochemychkaContext _context;

        public QuestionsController(PochemychkaContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index(int questionId, int testId)
        {
            if (questionId == 0 && _context.Questions.Where(q => q.IdTest == testId).FirstOrDefault() != null)
            {
                ViewBag.TestId = testId;
                string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
                AnswersUser answersUser = new AnswersUser
                {
                    IdUser = userId,
                    CountFullPoints = 0,
                    Time = DateTime.Now
                };
                _context.Add(answersUser);
                await _context.SaveChangesAsync();
                int firstQuestionId = _context.Questions.FirstOrDefault(q => q.IdTest == testId).IdQuestion;
                var questions = _context.Questions.Where(q => q.IdTest == testId && q.IdQuestion == firstQuestionId);
                ViewBag.Button = "Далее";
                return View(await questions.ToListAsync());
            }
            else
            {
                ViewBag.TestId = testId;
                var questions = _context.Questions.Where(q => q.IdTest == testId);
                return View(await questions.ToListAsync());
            }
        }

        public async Task<IActionResult> Dalee(int questionId, int testId, int pointsCount, string? answer)
        {
            var lastQuestionInTest = _context.Questions.OrderBy(q => q.IdQuestion).LastOrDefault(q => q.IdTest == testId && q.IdQuestion > questionId);
            var nextQuestion = _context.Questions.FirstOrDefault(q => q.IdQuestion > questionId && q.IdTest == testId);
            var nextToNextQuestion = nextQuestion != null ? _context.Questions.FirstOrDefault(q => q.IdQuestion > nextQuestion.IdQuestion && q.IdTest == testId) : null;

            if (nextToNextQuestion != null && nextQuestion.IdQuestion < lastQuestionInTest.IdQuestion)
            {
                ViewBag.TestId = testId;
                int nextQuestionId = nextQuestion.IdQuestion;
                var question = _context.Questions.FirstOrDefault(q => q.IdTest == testId && q.IdQuestion == nextQuestionId);
                Answer answerForDb = new Answer
                {
                    IdAnswersUser = _context.AnswersUsers.OrderBy(a => a.IdAnswersUser).Last().IdAnswersUser,
                    IdQuestion = questionId,
                    Answer1 = answer,
                    CountPoints = pointsCount
                };
                await _context.AddAsync(answerForDb);
                await _context.SaveChangesAsync();
                return View(nameof(Index), await _context.Questions.Where(q => q.IdTest == testId && q.IdQuestion == nextQuestionId).ToListAsync());
            }
            else if (lastQuestionInTest != null)
            {
                ViewBag.TestId = testId;
                var question = _context.Questions.FirstOrDefault(q => q.IdTest == testId && q.IdQuestion == questionId);
                Answer answerForDb = new Answer
                {
                    IdAnswersUser = _context.AnswersUsers.OrderBy(a => a.IdAnswersUser).Last().IdAnswersUser,
                    IdQuestion = questionId,
                    Answer1 = answer,
                    CountPoints = pointsCount
                };
                await _context.AddAsync(answerForDb);
                await _context.SaveChangesAsync();
                int nextQuestionId = nextQuestion.IdQuestion;
                ViewBag.Button = "Готovo";
                return View(nameof(Index), await _context.Questions.Where(q => q.IdTest == testId && q.IdQuestion == nextQuestionId).ToListAsync());
            }
            else
            {
                ViewBag.TestId = testId;
                var question = _context.Questions.FirstOrDefault(q => q.IdTest == testId && q.IdQuestion == questionId);
                Answer answerForDb = new Answer
                {
                    IdAnswersUser = _context.AnswersUsers.OrderBy(a => a.IdAnswersUser).Last().IdAnswersUser,
                    IdQuestion = questionId,
                    Answer1 = answer,
                    CountPoints = pointsCount
                };
                await _context.AddAsync(answerForDb);


                await _context.SaveChangesAsync();
                var userAnswer = _context.AnswersUsers.OrderBy(a => a.IdAnswersUser).Last();
                int totalPoints = _context.Answers.Where(a => a.IdAnswersUser == userAnswer.IdAnswersUser).Sum(a => a.CountPoints);
                userAnswer.CountFullPoints = totalPoints;
                _context.Entry(userAnswer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(End), new { count = totalPoints, testId = testId });
            }

            return View();
        }

        public async Task<IActionResult> End(int count, int testId)
        {
            var result = await _context.ResultDiapason.FirstOrDefaultAsync(r => r.IdTest == testId && r.StartDiapason <= count && r.EndDiapason >= count);
            ViewBag.Count = count;
            return View(result);
        }


        // GET: Questions/Create
        public IActionResult Create(int idT)
        {
            ViewBag.idT = idT;
            ViewData["IdTest"] = new SelectList(_context.Tests, "IdTest", "IdTest");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question, int idT, string answer)
        {


            
                question.IdTest = idT;
                ViewBag.idT = idT;
            _context.Questions.Add(question);
            _context.SaveChanges();
                return RedirectToAction(nameof(Index), new {idT=idT});
            
            return View(question);
        }
       
        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id, string? email, string name, bool? isAdmin,int idUse)
        {
            ViewBag.UserEmail = email;
            ViewBag.UserName = name;
            ViewBag.isAdmin = isAdmin;
            ViewBag.idUse = idUse;
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["IdTest"] = new SelectList(_context.Tests, "IdTest", "IdTest", question.IdTest);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdQuestion,IdTest,TextQuetion,Option1,Option2,Option3,Option4,Option5,CorrectAnswer,WhoAnsweredQuestion")] Question question, string? email, string name, bool? isAdmin,int idUse)
        {
            if (id != question.IdQuestion)
            {
                return NotFound();
            }
            ViewBag.UserEmail = email;
            ViewBag.UserName = name;
            ViewBag.isAdmin = isAdmin;
            ViewBag.idUse = idUse;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.IdQuestion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {name,email,isAdmin, idUse });
            }
            ViewData["IdTest"] = new SelectList(_context.Tests, "IdTest", "IdTest", question.IdTest);
            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int idT, string? email, string name, bool? isAdmin, int idUse)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'SerovaContext.Questions'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idT = question.IdTest,name,email,isAdmin, idUse });

        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.IdQuestion == id)).GetValueOrDefault();
        }
    }
}
