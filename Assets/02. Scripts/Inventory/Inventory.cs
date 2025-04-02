using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    private ItemDatabase db;

    void Start()
    {
        db = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        inventory.Add(db.items[0]);
    }

    void OnGUI()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
        }
    }
}
