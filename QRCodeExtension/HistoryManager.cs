using System;
using System.Collections.Generic;
using System.Linq;

namespace QRCodeExtension;

public class HistoryManager
{
    private readonly List<string> _history;
    private const int MaxCapacity = 50;

    public HistoryManager()
    {
        _history = [];
    }

    /// <summary>
    /// Initializes a new instance with existing history items for persistence
    /// </summary>
    /// <param name="initialHistory">Initial history items to load</param>
    public HistoryManager(IEnumerable<string> initialHistory)
    {
        _history = [];

        if (initialHistory != null)
        {
            // Add items in reverse order to maintain correct order (newest first)
            var items = initialHistory.Where(item => !string.IsNullOrWhiteSpace(item))
                                    .Take(MaxCapacity)
                                    .ToList();

            _history.AddRange(items);
        }
    }

    /// <summary>
    /// Adds a new history item. If the item already exists, moves it to the front; otherwise adds it to the front.
    /// </summary>
    /// <param name="item">The string to add</param>
    public void AddItem(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
            return;

        // Remove existing item if present
        _history.Remove(item);

        // Insert item at the front
        _history.Insert(0, item);

        // Remove last item if exceeding max capacity
        if (_history.Count > MaxCapacity)
        {
            _history.RemoveAt(_history.Count - 1);
        }
    }

    /// <summary>
    /// Gets all history items in order from newest to oldest
    /// </summary>
    /// <returns>Read-only list of history items</returns>
    public IReadOnlyList<string> GetHistory()
    {
        return _history.AsReadOnly();
    }

    /// <summary>
    /// Gets the number of history items
    /// </summary>
    public int Count => _history.Count;

    /// <summary>
    /// Clears all history items
    /// </summary>
    public void Clear()
    {
        _history.Clear();
    }

    /// <summary>
    /// Checks if the specified item is contained in the history
    /// </summary>
    /// <param name="item">The string to check</param>
    /// <returns>True if contained, otherwise false</returns>
    public bool Contains(string item)
    {
        return !string.IsNullOrWhiteSpace(item) && _history.Contains(item);
    }

    /// <summary>
    /// Removes the specified history item
    /// </summary>
    /// <param name="item">The string to remove</param>
    /// <returns>True if successfully removed, otherwise false</returns>
    public bool RemoveItem(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
            return false;

        return _history.Remove(item);
    }

    /// <summary>
    /// Gets the most recent n history items
    /// </summary>
    /// <param name="count">Number of items to retrieve</param>
    /// <returns>Most recent history items</returns>
    public IEnumerable<string> GetRecentItems(int count)
    {
        if (count <= 0)
            return Enumerable.Empty<string>();

        return _history.Take(count);
    }
}
