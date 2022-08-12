using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable] 
public class Character_Schema {
    public int hair;
    public int face;
    public int weapon;
}
[System.Serializable] 
public class Item_Schema {
    public int item_code;
    public int mount;
}

[System.Serializable] public class Skill_Schema {
    public int strength;
    public int rune_code;
}
[System.Serializable] 
public class Quest_Schema {
    public int quest_code;
    public int quest_progress;
}
[System.Serializable] 
public class UserData 
{ 
    public string _id;
    public string user_nickname; 
    public int job; 
    public Character_Schema character;
    public List<Item_Schema> inventory;
    public List<Skill_Schema> skills;
    public List<Quest_Schema> quest;
    public List<int> slot;
    public float level;
    
    public void printData() 
    { 
        Debug.Log("User_OID : " + _id + " , Nickname : " + user_nickname);
        Debug.Log("Hair : " + character.hair + " , weapon : " + character.weapon);
        for (int i = 0; i < inventory.Count; i++) {
            Debug.Log(" _ " + inventory[i].item_code  + " _ " + inventory[i].mount );
        }
    } 
} 

[System.Serializable]
public class MessagePacket
{
    public bool success;
    public string message;
}
[System.Serializable] 
public class UserDataPacket : MessagePacket
{
    public UserData data;
}

public class LoginManager : MonoBehaviour
{
    string BASE_URL = "localhost:3000/";
    public delegate void responseFunc(string res);

    void Start() 
    {
    }

    public void GetUserInfo()
    {
        StartCoroutine(HttpGET(
            "users/data/62e7a0cd2d20d2969d6cf10f", 
            success: (msg) => {
                Debug.Log(msg);
                
                UserDataPacket dataPacket = JsonUtility.FromJson<UserDataPacket>(msg);
                dataPacket.data.printData();
            },
            failure: (msg) => {
                Debug.LogError(msg);
            }
        ));
    }

    public void Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", "wak");
        form.AddField("user_pw", "wak");

        StartCoroutine(HttpPost(
            "users/login",
            form,
            success: (msg) => {
                Debug.Log(msg);
                
                UserDataPacket dataPacket = JsonUtility.FromJson<UserDataPacket>(msg);
                dataPacket.data.printData();
                if (dataPacket.success) {
                    // 로그인 성공
                } else {
                    // 실패
                }
            },
            failure: (msg) => {
                // 실패
            }
        ));
    }

    public void Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", "wak");
        form.AddField("user_pw", "wak");
        form.AddField("user_nickname", "user_nickname");

        StartCoroutine(HttpPost(
            "users/register",
            form,
            success: (msg) => {
                MessagePacket mp = JsonUtility.FromJson<MessagePacket>(msg);
                if (mp.success) {
                    // 회원가입 성공
                } else {
                    // 실패
                }
            },
            failure: (msg) => {
                // 실패
            }
        ));
    }

    IEnumerator HttpGET(string url, responseFunc success, responseFunc failure)
    {
        UnityWebRequest req = new UnityWebRequest();

        using (req = UnityWebRequest.Get(BASE_URL + url)) {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError) {
                failure(req.error);
            } else {
                success(req.downloadHandler.text);
            }
        }
    }

    IEnumerator HttpPost(string url, WWWForm form, responseFunc success, responseFunc failure)
    {
        UnityWebRequest req = new UnityWebRequest();

        using  (req = UnityWebRequest.Post(BASE_URL + url, form)) {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError) {
                failure(req.error);
            } else {
                success(req.downloadHandler.text);
            }
        }
    }
}
