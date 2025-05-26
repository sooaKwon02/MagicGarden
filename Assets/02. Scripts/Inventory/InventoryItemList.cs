using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string item_name;
    public string item_type;
    public string item_image;
}

[System.Serializable]
public class InventoryItemList
{
    public List<InventoryItem> items;
}