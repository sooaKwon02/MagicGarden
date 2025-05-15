using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using static Login;

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
        //string url = "http://127.0.0.1/MagicGarden/RegisterUser.php";
        string url = "http://127.0.0.1/MagicGarden/Join.php";

        WWWForm form = new WWWForm();
        //form.AddField("loginUser", username);
        //form.AddField("loginPass", password);
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            { 
                Debug.LogError("서버 연결 실패 : " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(json);
                Debug.Log(response.message);
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
                Debug.LogError("서버 연결 실패 : " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log("서버 응답 JSON: " + json);
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

                if (response.success)
                {
                    Debug.Log($"로그인성공, 유저이름 {response.user_name}");
                    SceneManager.LoadScene(1);
                }
                else
                {
                    Debug.Log(response.message);
                }
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
}
