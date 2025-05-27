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

    public GameObject panel;
    public TMP_Text panelText;

    private void Start()
    {
        createPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (idInput.isFocused)
            {
                passwordInput.Select();
            }
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

    public void ErrorMessage(string message)
    {
        panel.SetActive(true);
        panelText.text = message;
        StartCoroutine(ClosePanel());
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
            panel.SetActive(true);
            panelText.text = "아이디와 비밀번호를 모두 입력하세요.";
            StartCoroutine(ClosePanel());
        }
        else
        {
            panel.SetActive(true);
            panelText.text = "회원가입 성공!";
            StartCoroutine(ClosePanel());
            StartCoroutine(Main.Instance.Web.RegisterUser(userId, password));
        }
    }

    public void OnXButtonClick()
    {
        crIdInput.text = "";
        crPasswordInput.text = "";
        createPanel.gameObject.SetActive(false);
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }
}
