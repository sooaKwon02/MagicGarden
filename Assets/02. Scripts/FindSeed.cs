using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindSeed : MonoBehaviour
{
    public static bool isColl = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            isColl = true; Debug.Log(isColl);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isColl = true; Debug.Log(isColl);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isColl = false; Debug.Log(isColl);
    }
}
