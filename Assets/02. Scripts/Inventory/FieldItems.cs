using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    
    public void SetItem(Item _item)
    {
        item = _item;
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemImage = _item.itemImage;
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
