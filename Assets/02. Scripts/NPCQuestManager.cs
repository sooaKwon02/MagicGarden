using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestManager : MonoBehaviour
{
    public TMP_Text text;
    public Button nextButton;
    //public Button acceptButton;
    //public Button refuseButton;

    private List<string> textLines = new List<string>();
    private int index = 0;
    private System.Action onComplete;

    void Start()
    { 
        StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(1));
    }

    public void StartText(List<string> lines, System.Action onLineComplete = null)
    {
        textLines = lines;
        index = 0;
        onComplete = onLineComplete;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextText);
        NextText();
    }

    private void NextText()
    {
        if (index < textLines.Count)
        {
            StartCoroutine(TypeLine(textLines[index]));
            index++;
        }
        else
        {
            text.text = "";
            nextButton.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
    }

    IEnumerator TypeLine(string line)
    {
        text.text = "";
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
