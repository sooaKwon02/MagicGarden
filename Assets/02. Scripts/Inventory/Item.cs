using System.Collections;
using UnityEngine;

[System.Serializable]

public class Item
{
    public string itemName;
    public int itemID;
    public string itemDes;
    public Texture2D itemIcon;
    //public ItemType itemType;

    //public enum ItemType
    //{
    //    Seed
    //}

    public Item()
    {

    }

    public Item(string name, int id, string desc)// ItemType type){
    {
        itemName = name;
        itemID = id;
        itemDes = desc;

        //itemIcon = Resources.Load<Texture2D>(name);

        //itemType = type;
    }
}

