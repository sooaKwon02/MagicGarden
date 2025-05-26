using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string playerID;
    public int level = 0;
    public bool isInventoryLoad = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        StartCoroutine(Logout());
    }

    public void SetPlayerInfo(string id, int lv)
    {
        playerID = id;
        level = lv;

        StartCoroutine(Main.Instance.Web.LoadInventory(playerID));
    }

    public IEnumerator Logout()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 인벤토리 저장
            StartCoroutine(Main.Instance.Web.SaveInventory());

            yield return new WaitForSeconds(3.0f);
            // 플레이어 정보 초기화
            playerID = null;
            level = 0;
            Debug.Log("로그아웃 완료!");

            // 메인 메뉴로 이동 (원하는 씬 이름)
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");

        }
    }
}
