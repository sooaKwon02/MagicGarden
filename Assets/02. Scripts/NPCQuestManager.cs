using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NPCQuestManager : MonoBehaviour
{
    public GameObject npcPanel;
    public TMP_Text text;

    private List<string> introLines = new List<string>();
    private List<QuestStep> questLines = new List<QuestStep>();

    private int introIndex = 0;
    private int questIndex = 0;

    private bool isTyping = false;
    private Coroutine typingText;

    private bool isIntro = true;
    private bool isAccept = false;

    private int currentQuestId = 1;

    void Start()
    {
        npcPanel.SetActive(false);
        StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(currentQuestId));
    }

    void Update()
    {
        if (FindSeed.isColl)
        {
            if (isTyping)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!npcPanel.activeSelf)
                    {
                        npcPanel.SetActive(true);
                    }

                    StopCoroutine(typingText);
                    isTyping = false;

                    if (isIntro)
                    {
                        int safeIndex = Mathf.Clamp(introIndex - 1, 0, introLines.Count - 1);
                        text.text = introLines[safeIndex];
                    }
                    else
                    {
                        int safeIndex = Mathf.Clamp(questIndex - 1, 0, questLines.Count - 1);
                        text.text = questLines[safeIndex].dialogue;
                    }
                    return;
                }
            }

            if (isAccept)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    AcceptQuest();
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    StartCoroutine(RejectQuest());
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isIntro)
                {
                    StartCoroutine(ShowNextIntroLine());
                }
                else
                {
                    StartCoroutine(ShowNextQuestLine());
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(!isIntro && !isAccept && !isTyping)
                {
                    npcPanel.SetActive(false);
                }
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
        isAccept = false;

        text.text = "";
    }

    private IEnumerator ShowNextIntroLine()
    {
        if (!npcPanel.activeSelf)
            npcPanel.SetActive(true);

        yield return null;

        if (introIndex < introLines.Count)
        {
            typingText = StartCoroutine(TypeLine(introLines[introIndex]));
            introIndex++;
        }
        else
        {
            isIntro = false;
            isAccept = true;
            text.text = "퀘스트를 수락하시겠습니까?\n(Y: 수락 / N: 거절)";
        }
    }

    private void AcceptQuest()
    {
        Debug.Log("퀘스트 수락");
        isAccept = false;
        isIntro = false;

        StartCoroutine(ShowNextQuestLine());
    }

    private IEnumerator RejectQuest()
    {
        Debug.Log("퀘스트 거절");
        isAccept = false;
        isIntro = false;

        yield return StartCoroutine(TypeLine("아쉽다.. 다음에 부탁해!"));

        yield return new WaitForSeconds(1f);

        text.text = "";
        introIndex = 0;
        questIndex = 0;
        isIntro = true;

        npcPanel.SetActive(false);
    }

    private IEnumerator ShowNextQuestLine()
    {
        if (!npcPanel.activeSelf)
        {
            npcPanel.SetActive(true);

            yield return null;
        }

        if (questIndex >= questLines.Count)
        {
            Debug.Log($"퀘스트 {currentQuestId} 완료!");
            text.text = "";
            currentQuestId++;
            StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(currentQuestId));
            yield break;
        }

        QuestStep step = questLines[questIndex];
        typingText = StartCoroutine(TypeLine(step.dialogue));

        if (step.condition_type == "custom")
        {
            questIndex++;
        }
        else if (step.condition_type == "collect")
        {
            TryAutoAdvanceCollectQuest();
        }
    }

    public void TryAutoAdvanceCollectQuest()
    {
        if (questIndex >= questLines.Count) return;

        QuestStep step = questLines[questIndex];

        if (step.condition_type != "collect")
            return;

        string targetItemName = step.condition_target;
        int requiredAmount = step.condition_value;

        int count = Inventory.instance.items
            .Count(item => item.itemName == targetItemName);

        if (count >= requiredAmount)
        {
            Debug.Log($"인벤토리 내 '{targetItemName}' 수량 {count}개 == 조건 충족");
            questIndex++;
            //ShowNextQuestLine();
        }
        else
        {
            Debug.Log($"인벤토리에 '{targetItemName}' 없음 또는 수량 부족");
        }
    }

    public void NotifyAction(string type, string target, int value)
    {
        if (questIndex >= questLines.Count) return;

        QuestStep step = questLines[questIndex];

        if (step.condition_type == type &&
            step.condition_target == target &&
            value >= step.condition_value)
        {
            Debug.Log($"퀘스트 조건 충족: {type} {target} {value}");
            questIndex++;
            ShowNextQuestLine();
        }
    }

    private IEnumerator TypeLine(string line)
    {
        text.text = "";
        isTyping = true;
        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }
}
