using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string playerID;
    public int level = 0;
    public bool isInventoryLoad = false;

    public GameObject endPanelPrefab; 
    private GameObject endPanelInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        AttachEndPanel();
    }

    private void OnEnable()
    {
        AttachEndPanel();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AttachEndPanel();
    }

    private void AttachEndPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            if (endPanelInstance == null)
            {
                endPanelInstance = Instantiate(endPanelPrefab, canvas.transform);
                endPanelInstance.SetActive(false);
            }
            else
            {
                endPanelInstance.transform.SetParent(canvas.transform, false);
            }
        }
        else
        {
            Debug.LogWarning("Canvas를 찾을 수 없습니다!");
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (endPanelInstance == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            endPanelInstance.SetActive(true);
        }

        if (endPanelInstance.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartCoroutine(HandleLogout());
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                endPanelInstance.SetActive(false);
            }
        }
    }

    private IEnumerator HandleLogout()
    {
        yield return StartCoroutine(Main.Instance.Web.SaveInventory());

        yield return new WaitForSeconds(1.0f);

        playerID = null;
        level = 0;

        QuitGame();
    }

    public void SetPlayerInfo(string id, int lv)
    {
        playerID = id;
        level = lv;

        StartCoroutine(Main.Instance.Web.LoadInventory(playerID));
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
