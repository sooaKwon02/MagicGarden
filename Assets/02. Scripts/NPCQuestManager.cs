using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestManager : MonoBehaviour
{
    public TMP_Text text;

    private List<string> introLines = new List<string>();
    private List<QuestStep> questLines = new List<QuestStep>();
   
    private int introIndex = 0;
    private int questIndex = 0;

    private System.Action onComplete;
    private Coroutine typingText;

    private bool isTyping = false;
    private bool isIntro;

    void Start()
    { 
        StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(1));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingText);
                isTyping = false;

                if (isIntro)
                    text.text = introLines[introIndex - 1];
                else
                    text.text = questLines[questIndex - 1].dialogue;

                return;
            }

            if (isIntro)
            {
                ShowNextIntroLine();
            }
            else
            {
                ShowNextQuestLine();
            }
        }
    }

    public void SetQuestData(List<string> intro, List<QuestStep> quests)
    {
        introLines = intro;
        questLines = quests;

        introIndex = 0;
        questIndex = 0;
        isIntro = true;

        // 처음에는 아무것도 출력 안 함. 사용자가 Space 누르면 첫 줄 출력됨
    }

    private void ShowNextIntroLine()
    {
        if (introIndex < introLines.Count)
        {
            typingText = StartCoroutine(TypeLine(introLines[introIndex]));
            introIndex++;
        }
        else
        {
            isIntro = false; // 인트로 끝, 퀘스트로 넘어감
            text.text = "";
            Debug.Log("인트로 끝, 다음 Space부터 퀘스트 시작");
        }
    }

    private void ShowNextQuestLine()
    {
        if (questIndex < questLines.Count)
        {
            typingText = StartCoroutine(TypeLine(questLines[questIndex].dialogue));
            questIndex++;
        }
        else
        {
            Debug.Log("퀘스트 완료!");
            text.text = "";
        }
    }

    IEnumerator TypeLine(string line)
    {
        text.text = "";
        isTyping = true;

        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }
}
