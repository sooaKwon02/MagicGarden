using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    seed
}
[System.Serializable]

public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;

    public bool Use()
    {
        return false;
    }
}

