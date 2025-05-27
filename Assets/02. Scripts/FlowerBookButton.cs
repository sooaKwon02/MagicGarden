using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBookButton : MonoBehaviour
{
    public GameObject bookPanel;
    bool isBookOn = false;

    public GameObject infoPanel;
    bool isInfoOn = false;

    private void Start()
    {
        bookPanel.SetActive(isBookOn);
        infoPanel.SetActive(isInfoOn);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isBookOn = !isBookOn;
            bookPanel.SetActive(!isBookOn);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            isInfoOn = true;
            infoPanel.SetActive(isInfoOn);
        }
        else
        {
            isInfoOn = false;
            infoPanel.SetActive(isInfoOn);
        }
    }

    public void OnBookClick()
    {
        isBookOn = !isBookOn;
        bookPanel.SetActive(isBookOn);
    }
}
