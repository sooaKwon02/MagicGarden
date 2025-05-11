using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class Web : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        //StartCoroutine(RegisterUser("testuser3", "2222"));
        //StartCoroutine(Login("testuser3", "2222"));
        //StartCoroutine(GetQuestStepsFromServer(1));
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        //string url = "http://192.168.45.232/MagicGarden/RegisterUser.php";
        string url = "http://127.0.0.1/MagicGarden/RegisterUser.php";

        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            { 
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        //string url = "http://192.168.45.232/MagicGarden/Login.php";
        string url = "http://127.0.0.1/MagicGarden/Login.php";

        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        Debug.Log(username + " " + password);   

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SceneManager.LoadScene(1);
            }
        }
    }

    public IEnumerator GetQuestStepsFromServer(int questId)
    {
        string url = "http://127.0.0.1/MagicGarden/GetQuestStep.php";
        WWWForm form = new WWWForm();
        form.AddField("quest_id", questId);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("퀘스트 단계 불러오기 실패: " + www.error);
        }
        else
        {
            //PHP가 배열을 반환하므로 JSON 파싱을 위해 감싸줌
            string json = "{\"steps\":" + www.downloadHandler.text + "}";

            NPCQuestStepList npcStepList = JsonUtility.FromJson<NPCQuestStepList>(json);

            //foreach (QuestStep step in npcStepList.steps)
            //{
            //    Debug.Log($"[단계 {step.step_number}] {step.dialogue} ({step.condition_type})");
            //}
            List<string> intro = npcStepList.steps
                .Where(s => s.step_number == 0)
                .Select(s => s.dialogue)
                .ToList();

            List<QuestStep> questSteps = npcStepList.steps
                .Where(s => s.step_number >= 1)
                .OrderBy(s => s.step_number)
                .ToList();

            //Main.Instance.npcQuestManager.StartText(intro, () =>
            //{
                //Debug.Log("퀘스트 시작");
                Main.Instance.npcQuestManager.SetQuestData(intro, questSteps);
            //});
        }
    }

    //IEnumerator GetDialoguesFromServer(int npcId)
    //{
    //    string url = "http://127.0.0.1/MagicGarden/Login.php";

    //    WWWForm form = new WWWForm();
    //    form.AddField("npc_id", npcId);
    //    form.AddField("type", "normal"); // �Ǵ� "quest_intro", "quest"

    //    UnityWebRequest www = UnityWebRequest.Post("http://192.168.45.232/MagicGarden/GetNPCDialogues.php", form);
    //    yield return www.SendWebRequest();

    //    if (www.result == UnityWebRequest.Result.Success)
    //    {
    //        string json = www.downloadHandler.text;
    //        NPCDialogueList dialogueList = JsonUtility.FromJson<NPCDialogueList>("{\"dialogues\":" + json + "}");
    //        DisplayDialogues(dialogueList.dialogues);
    //    }
    //    else
    //    {
    //        Debug.LogError("NPC 대사 요청 실패: " + www.error);
    //    }
    //}

    //void DisplayDialogues(List<string> lines)
    //{
    //    // TODO: UI 출력 (대사창, 텍스트창 등)
    //    foreach (string line in lines)
    //    {
    //        Debug.Log($"[{npcName}] {line}");
    //    }
    //}
}
