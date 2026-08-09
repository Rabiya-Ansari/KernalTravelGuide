using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FeedbacksController : Controller
{
    private readonly AppDbContext _context;

    public FeedbacksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Feedbacks
    public async Task<IActionResult> Index()
    {
        var feedbacks = await _context.Feedbacks
            .OrderByDescending(x => x.FeedbackDate)
            .ToListAsync();

        return View(feedbacks);
    }

    // GET: Feedbacks/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedbacks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // GET: Feedbacks/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedbacks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // POST: Feedbacks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var feedback = await _context.Feedbacks
            .FindAsync(id);

        if (feedback != null)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}