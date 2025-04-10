using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    void OnEnable()
    {
        loginButton.onClick.AddListener(() =>
        {
            Debug.Log(" 버튼 눌림");

            if (Main.Instance == null)
            {
                Debug.LogError("1. Main.Instance가 null입니다!");
                return;
            }

            if (Main.Instance.Web == null)
            {
                Debug.LogError(" 2Main.Instance.Web이 null입니다!");
                return;
            }
            string username = idInput.text;
            string password = passwordInput.text;
            StartCoroutine(Main.Instance.Web.Login(username, password));
            Debug.Log("입력된 유저네임: " + username);
            Debug.Log("입력된 비밀번호: " + password);
        });
    }
}
