using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NPCQuestManager : MonoBehaviour
{
    public static NPCQuestManager Instance; 

    public GameObject npcPanel;
    public TMP_Text text;

    private List<string> introLines = new List<string>();
    private List<QuestStep> questLines = new List<QuestStep>();

    private int introIndex = 0;
    private int questIndex = 0;

    private bool isTyping = false; //대사 타이핑 중
    private bool isQuest = false;
    private Coroutine typingText;

    private bool isIntro = true;
    private bool isAccept = false;

    private int currentQuestId = 1;

    AudioSource audioSource;
    public AudioClip questCompleteSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        npcPanel.SetActive(false);
        StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(currentQuestId));
    }

    void Update()
    {
        if (CheckPlayer.isColl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (isTyping)
                {
                    StopCoroutine(typingText);
                    isTyping = false;

                    if (isIntro)
                    {
                        int safeIndex = Mathf.Clamp(introIndex - 1, 0, introLines.Count - 1);
                        text.text = introLines[safeIndex];
                    }
                    else
                    {
                        int safeIndex = Mathf.Clamp(questIndex, 0, questLines.Count - 1);
                        text.text = questLines[safeIndex].dialogue;
                    }

                    return;
                }
                    
                if (isQuest) return; // 중복 실행 방지
                if (isAccept) return;

                if (isIntro)
                    StartCoroutine(ShowNextIntroLine());
                else
                    StartCoroutine(ShowNextQuestLine());
            }

            if (isAccept)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                    AcceptQuest();
                else if (Input.GetKeyDown(KeyCode.N))
                    StartCoroutine(RejectQuest());
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isIntro && !isAccept && !isTyping)
                    npcPanel.SetActive(false);
            }
        }
    }

    public void SetQuestData(List<string> intro, List<QuestStep> quests)
    {
        introLines = intro;
        questLines = quests;

        introIndex = 0;
        questIndex = 0;

        if (introLines.Count > 0 || questLines.Count > 0)
        {
            isIntro = true;
            isAccept = false;
            text.text = "";
        }
        else
        {
            Debug.Log("[NPCQuestManager] 모든 퀘스트 완료! Intro 진입 안함");
            isIntro = false;
            isAccept = false;
            text.text = "모든 퀘스트 완료!";
        }
    }

    private IEnumerator ShowNextIntroLine()
    {
        if (!npcPanel.activeSelf)
            npcPanel.SetActive(true);

        isQuest = true;

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

        yield return new WaitUntil(() => !isTyping);
        isQuest = false;
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

        typingText = StartCoroutine(TypeLine("아쉽다.. 다음에 부탁해!"));
        yield return new WaitUntil(() => !isTyping);

        yield return new WaitForSeconds(1f);

        text.text = "";
        introIndex = 0;
        questIndex = 0;
        isIntro = true;

        npcPanel.SetActive(false);
    }

    private IEnumerator ShowNextQuestLine()
    {
        if (isQuest) yield break;
        isQuest = true;

        if (!npcPanel.activeSelf)
        {
            npcPanel.SetActive(true);
            yield return null;
        }

        if (questIndex >= questLines.Count)
        {
            Debug.Log($"퀘스트 {currentQuestId} 완료!");
            text.text = "";

            isIntro = false;
            isAccept = false;
            isQuest = false;

            currentQuestId++;
            StartCoroutine(Main.Instance.Web.GetQuestStepsFromServer(currentQuestId));

            yield break;
        }

        QuestStep step = questLines[questIndex];
        typingText = StartCoroutine(TypeLine(step.dialogue));

        if (step.condition_type == "custom")
        {
            questIndex++;
            yield return new WaitUntil(() => !isTyping);
            isQuest = false;
            StartCoroutine(ShowNextQuestLine());
        }
        else if (step.condition_type == "collect")
        {
            TryAutoAdvanceCollectQuest();
            yield return new WaitUntil(() => !isTyping);
            isQuest = false;
        }
        else
        {
            yield return new WaitUntil(() => !isTyping);
            isQuest = false;
        }
    }

    public void TryAutoAdvanceCollectQuest()
    {
        if (questIndex >= questLines.Count) return;

        QuestStep step = questLines[questIndex];
        if (step.condition_type != "collect") return;

        int count = Inventory.instance.items.Count(item => item.itemName == step.condition_target);
        if (count >= step.condition_value)
        {
            Debug.Log($"'{step.condition_target}' 수량 {count}개: 조건 충족!");
            questIndex++;
            if (audioSource != null && questCompleteSFX != null)
            {
                audioSource.PlayOneShot(questCompleteSFX);
            }
            StartCoroutine(ShowNextQuestLine());
        }
        else
        {
            Debug.Log($"인벤토리에 '{step.condition_target}' 없음 또는 수량 부족");
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
            if (audioSource != null && questCompleteSFX != null)
            {
                audioSource.PlayOneShot(questCompleteSFX);
            }
            questIndex++;
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

    public QuestStep GetCurrentQuestStep()
    {
        if (questIndex < questLines.Count)
            return questLines[questIndex];
        return null;
    }
}
