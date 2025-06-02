using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo_application___Base_code.Controllers;

/// <summary>
/// Manage the user login session 
/// </summary>
public class SessionController : Controller
{
    public IActionResult Index()
    {
        var userId = GetUserId();

        if (userId is not null)
        {
            List<SessionStoreEntry> entries = AdvancedSessionStore.GetEntriesByUserId(userId);
            return View(entries);
        }

        return View(new List<SessionStoreEntry>());
    }

    /// <summary>
    /// Delete the specific session in the session store
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult DeleteSession(int id)
    {
        AdvancedSessionStore.RemoveEntry(id);

        return RedirectToAction("Index");
    }

    /// <summary>
    /// Delete all sessions for the current user
    /// </summary>
    /// <returns></returns>
    public IActionResult DeleteAllSessions()
    {
        var userId = GetUserId();

        if (userId is not null)
        {
            AdvancedSessionStore.RemoveAllEntriesForUserId(userId);
        }

        return RedirectToAction("Index");
    }

    private string GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            // Try to get the sub claim
            userId = User.FindFirst("sub")?.Value;
        }

        return userId;
    }
}

