using System.Collections;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]

public class Item
{
    public ToolboxItemFilterType itemType;
    public string itemName;
    public MeshFilter itemMesh;

    public bool Use()
    {
        return false;
    }
}

