using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class DB_Connect : MonoBehaviour
{
    // Node.js 서버 주소와 엔드포인트
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

                // 회원가입 성공 여부 확인
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
        // 폼 데이터 생성
        WWWForm form = new WWWForm();
        form.AddField("username", ID);
        form.AddField("password", password);

        string loginUrl = serverUrl + "/login";

        // Post 요청 생성
        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            // 요청 보내기
            yield return www.SendWebRequest();

            // 요청 완료 후 응답 처리
            if(www.result == UnityWebRequest.Result.Success)
            {
                // Json 응답을 파싱하여 처리
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Response : " + jsonResponse);

                // Json 응답을 객체로 반환
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);

                // 성공 여부 확인
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
                // 요청 실패 처리
                Debug.LogError("Request failed: " + www.error);
            }
        }
    }

    // 로그인 응답을 처리할 클래스
    [Serializable]
    public class LoginResponse
    {
        public bool success;
    }

    // 회원가입 응답을 처리한 클래스
    public class RegisterResponse
    {
        public bool success;
        public string message;
    }
}
