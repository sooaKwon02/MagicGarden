using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance;
   // public GameObject createPanel;

    public Web Web;
    public NPCQuestManager npcQuestManager;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //createPanel.SetActive(false);
        //Web = GetComponent<Web>();
    }

    //public void CreateID()
    //{
    //    createPanel.SetActive(true);
    //}
}
