#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdGod.Data;

namespace CrowdGod.WebMVC.Controllers
{
    public class AnswerController : Controller
    {
        private readonly CrowdGodDbContext _context;

        public AnswerController(CrowdGodDbContext context)
        {
            _context = context;
        }

        // GET: Answer
        public async Task<IActionResult> Index()
        {
            var crowdGodDbContext = _context.Answers.Include(a => a.Question);
            return View(await crowdGodDbContext.ToListAsync());
        }

        // GET: Answer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .Include(a => a.Replies)
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answer/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Answer/Create/{id:int}")]
        public async Task<IActionResult> Create(int id, [Bind("AnswerId,Content")] Answer answer)
        {
            var question = _context.Questions.SingleOrDefault(e => e.QuestionId == id);
            answer.QuestionId = id;
            answer.Question = question;
            answer.CreatedUtc = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(answer);
                question.Answers.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Question", new { @id = answer.QuestionId });
            }
            return View(answer);
        }

        [Route("Answer/Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnswerId,Content,QuestionId")] Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return NotFound();
            }
            answer.ModifiedUtc = DateTime.Now;
            answer.CreatedUtc = answer.CreatedUtc;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.AnswerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Question", new { @id = answer.QuestionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Title", answer.QuestionId);
            return View(answer);
        }

        // GET: Answer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { @id = answer.QuestionId });
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.AnswerId == id);
        }
    }
}
