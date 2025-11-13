using ShoppingList.Application.Interfaces;
using ShoppingList.Domain.Models;
using System;

namespace ShoppingList.Application.Services;

public class ShoppingListService : IShoppingListService
{
    private ShoppingItem[] _items;
    private int _nextIndex;

    public ShoppingListService()
    {
        _nextIndex = 0;
    }

    public IReadOnlyList<ShoppingItem> GetAll()
    {
        if(_nextIndex == 0)
            return [];
        return _items;  
    }

    public ShoppingItem? GetById(string id)
    {
        foreach (var item in _items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public ShoppingItem? Add(string name, int quantity, string? notes)
    {
       var item = new ShoppingItem()
       {
            Name = name,
            Quantity = quantity,
            Notes = notes,            
        };
        
        _nextIndex++;
        Array.Resize<ShoppingItem>(ref _items, _nextIndex);
        _items[_nextIndex-1] = item;
        return item;
    }

    public ShoppingItem? Update(string id, string name, int quantity, string? notes)
    {
        // TODO: Students - Implement this method
        // Return the updated item, or null if not found
        return null;
    }

    public bool Delete(string id)
    {
        for(int index = 0; index < _items.Length; index++)
        {
            if (_items[index].Id == id)
            {
                for (int j = index+1; j < _items.Length; j++) 
                {
                    _items[j-1] = _items[j];
                }
                _nextIndex--;
                Array.Resize<ShoppingItem>(ref _items, _nextIndex);

                return true;
            }
        }
        return false;
    }

    public IReadOnlyList<ShoppingItem> Search(string query)
    {
        foreach (var item in _items)
        {
            if (item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (item.Notes != null && item.Notes.Contains(query, StringComparison.OrdinalIgnoreCase)))
            {
                return [item];
            }
        }
        return [];
    }

    public int ClearPurchased()
    {
        // TODO: Students - Implement this method
        // Return the count of removed items
        return 0;
    }

    public bool TogglePurchased(string id)
    {
        // TODO: Students - Implement this method
        // Return true if successful, false if item not found
        return false;
    }

    public bool Reorder(IReadOnlyList<string> orderedIds)
    {
        // TODO: Students - Implement this method
        // Return true if successful, false otherwise
        return false;
    }
    public ShoppingItem[] _TEST_items => _items;
    
}