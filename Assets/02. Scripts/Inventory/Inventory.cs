using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }
}
