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

    public IEnumerator RegisterUser(string username, string password)
    {
        //string url = "http://192.168.45.232/MagicGarden/RegisterUser.php";
        //string url = "http://127.0.0.1/MagicGarden/RegisterUser.php";
        string url = "http://127.0.0.1/MagicGarden/Join.php";

        WWWForm form = new WWWForm();

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
                    GameManager.Instance.SetPlayerInfo(response.user_name, response.user_level);
                    SceneManager.LoadScene(1);
                }
                else
                {
                    Login lg = FindObjectOfType<Login>();
                    lg.ErrorMessage(response.message);
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

            NPCQuestManager.Instance.SetQuestData(intro, questSteps);
        }
    }

    public IEnumerator LoadInventory(string playerID)
    {
        string url = "http://127.0.0.1/MagicGarden/LoadInventory.php";

        WWWForm form = new WWWForm();
        form.AddField("player_id", playerID ?? "");

        using(UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("인벤토리 정보 불러오기 실패 : " + www.error);
            }
            else
            {
                string json = "{\"items\":" + www.downloadHandler.text + "}";
                InventoryItemList itemList = JsonUtility.FromJson<InventoryItemList>(json);

                if(itemList.items.Count == 0)
                {
                    Debug.Log("인벤토리 비어있음");
                }
                else
                {
                    Debug.Log("아이템 로드 완료");
                }
                GameManager.Instance.isInventoryLoad = true;

                foreach(var data in itemList.items)
                {
                    Item item = ItemDatabase.instance.itemDB.Find(i => i.itemName == data.item_name);
                    if(item != null)
                    {
                        item.itemImage = Resources.Load<Sprite>(data.item_image);
                        if(item.itemImage == null)
                            Debug.LogWarning($"이미지 로드 실패 : {data.item_image}");
                        
                        Inventory.instance.AddItem(item);
                    }
                    else
                    {
                        Debug.LogWarning($"[경고] '{data.item_name}'을 찾지 못함");
                    }
                }
            }
        }
    }

    public IEnumerator SaveInventory()
    {
        if (string.IsNullOrEmpty(GameManager.Instance.playerID))
        {
            Debug.LogWarning("플레이어 ID가 설정되지 않음. 저장 건너뜀.");
            yield break;
        }

        string url = "http://127.0.0.1/MagicGarden/SaveInventory.php";

        foreach (var item in Inventory.instance.items)
        {
            string itemName = item?.itemName ?? "";
            string itemType = item?.itemType.ToString() ?? "";
            string itemImagePath = item?.itemImage != null ? item.itemImage.name : "";
            
            WWWForm form = new WWWForm();
            form.AddField("player_id", GameManager.Instance.playerID ?? "");
            form.AddField("item_name", itemName);
            form.AddField("item_type", itemType);
            form.AddField("item_image", "Seed/" + itemImagePath);

            Debug.Log($"저장 요청: {itemName}, {itemType}, {itemImagePath}");

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError("저장 실패: " + www.error);
                else
                    Debug.Log("저장 성공: " + www.downloadHandler.text);
            }
        }
    }
}
