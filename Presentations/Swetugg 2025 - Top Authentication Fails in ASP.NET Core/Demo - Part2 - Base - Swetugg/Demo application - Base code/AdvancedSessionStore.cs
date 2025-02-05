using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Concurrent;

namespace Demo_application___Base_code;

/// <summary>
/// Sample demo authentication SessionStore
/// 
/// Written by Tore Nestenius
/// https://nestenius.se
/// </summary>
public class AdvancedSessionStore : ITicketStore
{
    private static readonly ConcurrentDictionary<string, SessionStoreEntry> mytickets = new();


    public static List<SessionStoreEntry> GetEntriesByUserId(string userId)
    {
        return mytickets.Values.Where(entry => entry.UserId == userId).ToList();
    }

    public static void RemoveAllEntriesForUserId(string userId)
    {
        var keysToRemove = mytickets.Where(entry => entry.Value.UserId == userId).Select(entry => entry.Key).ToList();
        foreach (var key in keysToRemove)
        {
            mytickets.TryRemove(key, out _);
        }
    }

    public static void RemoveEntry(int entryId)
    {
        var keyToRemove = mytickets.FirstOrDefault(entry => entry.Value.Id == entryId).Key;
        if (keyToRemove != null)
        {
            mytickets.TryRemove(keyToRemove, out _);
        }
    }

    public static void RemoveAll()
    {
        mytickets.Clear();
    }

    public Task RemoveAsync(string key)
    {
        if (mytickets.ContainsKey(key))
            mytickets.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        mytickets[key] = CreateSessionStoreEntry(ticket);
        return Task.CompletedTask;
    }

    public Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        mytickets.TryGetValue(key, out SessionStoreEntry entry);
        if (entry != null)
            return Task.FromResult(entry.Ticket);
        else
            return Task.FromResult<AuthenticationTicket>(default);
    }

    public Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString();

        var entry = CreateSessionStoreEntry(ticket);


        entry.Id = mytickets.Count + 1;
        if (mytickets.TryAdd(key, entry))
        {
            return Task.FromResult(key);
        }
        else
            throw new InvalidOperationException("Failed to add entry to AdvancedSessionStore");
    }

    private static SessionStoreEntry CreateSessionStoreEntry(AuthenticationTicket ticket)
    {
        var entry = new SessionStoreEntry
        {
            EntryDate = DateTime.Now,
            Ticket = ticket
        };

        entry.UserId = GetUserId(ticket);

        if (ticket.Properties.Items.TryGetValue("IpAddress", out var ipAddress))
        {
            entry.IPAddress = ipAddress;
        }

        if (ticket.Properties.Items.TryGetValue("UserAgent", out var userAgent))
        {
            entry.Browser = userAgent;
        }

        return entry;
    }


    private static string GetUserId(AuthenticationTicket ticket)
    {
        var userId = ticket.Principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            // Try to get the sub claim
            userId = ticket.Principal.FindFirst("sub")?.Value;
        }

        return userId;
    }
}


public class SessionStoreEntry
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Browser { get; set; }
    public string IPAddress { get; set; }
    public DateTime EntryDate { get; set; }
    public AuthenticationTicket Ticket { get; set; }
}