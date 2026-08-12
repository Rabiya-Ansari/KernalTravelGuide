using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
public class ContactMessagesController : Controller
{
    private readonly AppDbContext _context;

    public ContactMessagesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ContactMessages
    public async Task<IActionResult> Index()
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(x => x.SentOn)
            .ToListAsync();

        return View(messages);
    }

    // GET: ContactMessages/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var message = await _context.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    // GET: ContactMessages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var message = await _context.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    // POST: ContactMessages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var message = await _context.ContactMessages
            .FindAsync(id);

        if (message != null)
        {
            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}