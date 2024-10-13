using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class DB_Connect : MonoBehaviour
{
    // Node.js ���� �ּҿ� ��������Ʈ
    private string serverUrl = "http://localhost:3000";

    private void Start()
    {
        //Login("test", "kjm@7940718");
        //Register("King2", "aaa@222", "emailTest2");
        Login("King2", "aaa@222");
    }

    public void Register(string ID, string password, string email)
    {
        StartCoroutine(RegisterCoroutine(ID, password, email));
    }

    public void Login(string ID, string password)
    {
        StartCoroutine(LoginCoroutine(ID, password));
    }

    private IEnumerator RegisterCoroutine(string ID, string password, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", ID);
        form.AddField("password", password);
        form.AddField("email", email);

        string registerUrl = serverUrl + "/register";

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response:" + jsonResponse);

                RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(jsonResponse);

                // ȸ������ ���� ���� Ȯ��
                if (response.success)
                {
                    Debug.Log("Registration successful: " + response.message);
                }
                else
                {
                    Debug.Log("Registration failed: " + response.message);
                }
            }
            else
            {
                Debug.LogError("Request failed: " + www.error);
            }
        }
    }

    private IEnumerator LoginCoroutine(string ID, string password)
    {
        // �� ������ ����
        WWWForm form = new WWWForm();
        form.AddField("username", ID);
        form.AddField("password", password);

        string loginUrl = serverUrl + "/login";

        // Post ��û ����
        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            // ��û ������
            yield return www.SendWebRequest();

            // ��û �Ϸ� �� ���� ó��
            if(www.result == UnityWebRequest.Result.Success)
            {
                // Json ������ �Ľ��Ͽ� ó��
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response : " + jsonResponse);

                // Json ������ ��ü�� ��ȯ
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                // ���� ���� Ȯ��
                if (response.success)
                {
                    Debug.Log("Login successful!");
                }
                else
                {
                    Debug.Log("Login failed: Incorrect username or password.");
                }
            }
            else
            {
                // ��û ���� ó��
                Debug.LogError("Request failed: " + www.error);
            }
        }
    }

    // �α��� ������ ó���� Ŭ����
    [Serializable]
    public class LoginResponse
    {
        public bool success;
    }

    // ȸ������ ������ ó���� Ŭ����
    public class RegisterResponse
    {
        public bool success;
        public string message;
    }
}
