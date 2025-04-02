using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Start()
    {
        items.Add(new Item("Spicy Seed", 1001, "This seed is spicy seed"));
        items.Add(new Item("Sweet Seed", 1002, "This seed is sweet seed"));
        items.Add(new Item("Cold Seed", 1003, "This seed is cold seed"));
        items.Add(new Item("Beer Seed", 1004, "This seed is beer seed"));
    }
}
