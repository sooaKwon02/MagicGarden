using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public static bool isColl = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            isColl = true; 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isColl = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isColl = false; 
    }
}
