using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    
    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemMesh = _item.itemMesh;
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
