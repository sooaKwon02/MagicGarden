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

    void OnEnable()
    {
        loginButton.onClick.AddListener(() =>
        {
            string username = idInput.text;
            string password = passwordInput.text;
            StartCoroutine(Main.Instance.Web.Login(username, password));
            Debug.Log("�Էµ� ��������: " + username);
            Debug.Log("�Էµ� ��й�ȣ: " + password);
        });

        createButton.onClick.AddListener(() =>
        {
            string username = crIdInput.text;
            string password = crPasswordInput.text;
            StartCoroutine(Main.Instance.Web.RegisterUser(username, password));
        });
    }
}
