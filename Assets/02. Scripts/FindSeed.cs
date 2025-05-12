using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindSeed : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Main.Instance.npcQuestManager.TryAutoAdvanceCollectQuest();
        }
    }
}
