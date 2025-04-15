using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Web : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(RegisterUser("testuser3", "2222"));
        //StartCoroutine(Login("testuser3", "2222"));
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        //string url = "http://192.168.45.232/MagicGarden/RegisterUser.php";
        string url = "http://127.0.0.1/MagicGarden/RegisterUser.php";

        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            { 
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
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
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SceneManager.LoadScene(1);
            }
        }
    }
}
