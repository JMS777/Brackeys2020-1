﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action ItemsChanged;

    public string Name;
    public int size = 10;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (items.Count >= size)
        {
            Debug.Log("Not enough space.");
            return false;
        }

        items.Add(item);
        ItemsChanged?.Invoke();
        return true;
    }

    public bool Remove(Item item)
    {
        var success = items.Remove(item);
        ItemsChanged?.Invoke();
        
        return success;
    }
}
