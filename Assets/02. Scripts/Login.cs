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
            Debug.Log(" ��ư ����");

            if (Main.Instance == null)
            {
                Debug.LogError("1. Main.Instance�� null�Դϴ�!");
                return;
            }

            if (Main.Instance.Web == null)
            {
                Debug.LogError(" 2Main.Instance.Web�� null�Դϴ�!");
                return;
            }
            string username = idInput.text;
            string password = passwordInput.text;
            StartCoroutine(Main.Instance.Web.Login(username, password));
            Debug.Log("�Էµ� ��������: " + username);
            Debug.Log("�Էµ� ��й�ȣ: " + password);
        });
    }
}
