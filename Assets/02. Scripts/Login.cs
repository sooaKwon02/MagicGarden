using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField passwordInput;

    public TMP_InputField crIdInput;
    public TMP_InputField crPasswordInput;

    public Button loginButton;
    public Button createButton;

    public GameObject createPanel;

    private void Start()
    {
        createPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (idInput.isFocused)
            {
                passwordInput.Select();
            }
            // Create 창 입력 처리도 해주면 좋음
            else if (crIdInput.isFocused)
            {
                crPasswordInput.Select();
            }
        }
    }

    public void OnStartButtonClick()
    {
        string username = idInput.text;
        string password = passwordInput.text;
        StartCoroutine(Main.Instance.Web.Login(username, password));
        Debug.Log("입력된 유저네임: " + username);
        Debug.Log("입력된 비밀번호: " + password);
    }

    public void IsCreatePanel()
    {
        createPanel.gameObject.SetActive(true);
    }

    public void OnCreateButtonClick()
    {
        string userId = crIdInput.text.Trim();
        string password = crPasswordInput.text;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
        {
            Debug.Log("아이디와 비밀번호를 모두 입력하세요.");
        }
        else
            StartCoroutine(Main.Instance.Web.RegisterUser(userId, password));
    }

    public void OnXButtonClick()
    {
        createPanel.gameObject.SetActive(false);
    }
}
