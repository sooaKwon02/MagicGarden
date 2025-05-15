using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateUser : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCreateButtonClick()
    {
        this.gameObject.SetActive(true);

        string userId = idInputField.text.Trim();
        string password = passwordInputField.text;

        if (!string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
        {
            Debug.Log("아이디와 비밀번호를 모두 입력하세요.");
        }

        Main.Instance.Web.RegisterUser(userId, password);
    }
}
