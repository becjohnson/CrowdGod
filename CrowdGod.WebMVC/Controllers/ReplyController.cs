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
    public class ReplyController : Controller
    {
        private readonly CrowdGodDbContext _context;

        public ReplyController(CrowdGodDbContext context)
        {
            _context = context;
        }

        // GET: Reply
        public async Task<IActionResult> Index()
        {
            var crowdGodDbContext = _context.Replies.Include(r => r.Answer);
            return View(await crowdGodDbContext.ToListAsync());
        }

        // GET: Reply/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies
                .Include(r => r.Answer)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // GET: Reply/Create
        public IActionResult Create()
        {
            ViewData["AnswerId"] = new SelectList(_context.Answers, "AnswerId", "Content");
            return View();
        }

        // POST: Reply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Reply/Create/{id:int}")]
        public async Task<IActionResult> Create(int id, [Bind("ReplyId,Content")] Reply reply)
        {
            var answer = _context.Answers.SingleOrDefault(a => a.AnswerId == id);
            answer.QuestionId = reply.QuestionId;
            var question = _context.Questions.SingleOrDefault(q => q.QuestionId == reply.QuestionId);
            reply.QuestionId = question.QuestionId;
            reply.AnswerId = id;
            reply.Answer = answer;
            reply.CreatedUtc = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                _context.Replies.Add(reply);
                answer.Replies.Add(reply);
                question.Replies.Add(reply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Question", new { @id = answer.QuestionId });
            }
            return View(reply);
        }

        // GET: Reply/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies.FindAsync(id);
            if (reply == null)
            {
                return NotFound();
            }
            ViewData["AnswerId"] = new SelectList(_context.Answers, "AnswerId", "Content", reply.AnswerId);
            return View(reply);
        }

        // POST: Reply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReplyId,Content,CreatedUtc,ModifiedUtc,AnswerId")] Reply reply)
        {
            if (id != reply.ReplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReplyExists(reply.ReplyId))
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
            ViewData["AnswerId"] = new SelectList(_context.Answers, "AnswerId", "Content", reply.AnswerId);
            return View(reply);
        }

        // GET: Reply/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies
                .Include(r => r.Answer)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // POST: Reply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reply = await _context.Replies.FindAsync(id);
            _context.Replies.Remove(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReplyExists(int id)
        {
            return _context.Replies.Any(e => e.ReplyId == id);
        }
    }
}
