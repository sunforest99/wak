using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable] 
public class Character_Schema {
    public int hair;
    public int face;
    public int weapon;
    public int shirts;
    public int pants;
}
[System.Serializable] 
public class Item_Schema {
    public int item_code;
    public int mount;
    public Item_Schema(int itemcode, int mount)
    {
        this.item_code = itemcode;
        this.mount = mount;
    }
}

[System.Serializable]
public class Skill_Schema {
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
    public List<List<Item_Schema>> inventory = new List<List<Item_Schema>>();
    public List<Skill_Schema> skills = new List<Skill_Schema>();
    // public Quest_Schema main_quest;
    public List<Quest_Schema> sub_quest;
    public List<int> quest_done;
    public List<Skill_TREE> upgrade = new List<Skill_TREE>();
    public int love;
    public List<int> npc = new List<int>();
    public int coin;
    
    public void printData() 
    { 
        Debug.Log("User_OID : " + _id + " , Nickname : " + user_nickname);
        Debug.Log("Hair : " + character.hair + " , weapon : " + character.weapon);
        for (int i = 0; i < inventory.Count; i++)
        {
            for (int j = 0; j < inventory[i].Count; j++)
                Debug.Log(" _ " + inventory[i][j].item_code + " _ " + inventory[i][j].mount);
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
    [Space(20)][Header("[  알림 메시지  ]")]  // ==============================================================================================================================
    [SerializeField] TMPro.TextMeshProUGUI alertMsg;
    [SerializeField] GameObject blockAct;               // API 응답 대기중 UI 버튼 재클릭 방지용
    [SerializeField] GameObject customizeCanvas;
    
    [Space(20)][Header("[  로그인  ]")]  // ==============================================================================================================================
    [SerializeField] TMPro.TMP_InputField idInput;
    [SerializeField] TMPro.TMP_InputField pwInput;
    [SerializeField] GameObject loginBlock;
    [SerializeField] GameObject gameStartBT;
    [SerializeField] GameObject loginBT;

    [Space(20)][Header("[  회원가입  ]")]  // ==============================================================================================================================    
    [SerializeField] TMPro.TMP_InputField idInput_r;
    [SerializeField] TMPro.TMP_InputField pwInput_r;
    [SerializeField] TMPro.TMP_InputField nickInput_r;
    [SerializeField] GameObject registerBlock;

    [Space(20)][Header("[  핵심 매니저들  ]")]  // ==============================================================================================================================    
    [SerializeField] GameObject[] managers;

    // string BASE_URL = "localhost:3000/";
    public delegate void responseFunc(string res);

    [SerializeField] GameObject loadStart;          // 게임 시작 후 마을로 갈때 로딩

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
        // UI 재입력 방지
        alertMsg.text = "요청중..";
        blockAct.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("user_id", idInput.text);
        form.AddField("user_pw", pwInput.text);

        StartCoroutine(HttpPost(
            "users/login",
            form,
            success: (msg) => {
                Debug.Log(msg);
                
                UserDataPacket dataPacket = JsonUtility.FromJson<UserDataPacket>(msg);

                if (dataPacket.success) {
                    
                    dataPacket.data.printData();

                    // 로그인 성공
                    for (int i = 0; i < managers.Length; i++) {
                        managers[i].SetActive(true);
                    }
                    
                    GameMng.I.userData = dataPacket.data;
                    
                    loginBlock.SetActive(false);
                    blockAct.SetActive(false);

                    loginBT.SetActive(false);
                    gameStartBT.SetActive(true);
                    
                    Debug.Log(GameMng.I.userData.user_nickname);

                    // if (dataPacket.data.character.hair.Equals(-1))
                    // {
                    //     // 커스터마이징 안되어 있음
                    //     customizeCanvas.SetActive(true);
                    // }
                    // else if (dataPacket.data.job.Equals(0))
                    // {
                    //     // 직업 선택 안되어 있음
                    //     SceneManager.LoadScene("TempleScene");
                    // }
                    // else
                    // {
                    //     // 마을로!
                    // }

                } else {
                    // 실패
                    alertMsg.text = "로그인 실패! (아이디 혹은 비밀번호 틀림)";
                    alertMsg.transform.parent.gameObject.SetActive(true);
                }
            },
            failure: (msg) => {
                // 실패
                alertMsg.text = "로그인 실패! (DB 서버 연결 실패)";
                alertMsg.transform.parent.gameObject.SetActive(true);
            }
        ));
    }

    public void Register()
    {
        alertMsg.text = "요청중..";
        blockAct.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("user_id", idInput_r.text);
        form.AddField("user_pw", pwInput_r.text);
        form.AddField("user_nickname", nickInput_r.text);

        StartCoroutine(HttpPost(
            "users/register",
            form,
            success: (msg) => {
                MessagePacket mp = JsonUtility.FromJson<MessagePacket>(msg);
                if (mp.success) {
                    // 회원가입 성공
                    blockAct.SetActive(false);
                    registerBlock.SetActive(false);
                    loginBlock.SetActive(true);
        
                } else {
                    // 실패
                    alertMsg.text = "로그인 실패! (이미 있는 아이디 혹은 닉네임)";
                    alertMsg.transform.parent.gameObject.SetActive(true);
                }
            },
            failure: (msg) => {
                // 실패
                alertMsg.text = "회원가입 실패! (아이디 혹은 비밀번호 틀림)";
                alertMsg.transform.parent.gameObject.SetActive(true);
            }
        ));
    }

    public void gameStart()
    {
        if (GameMng.I.userData.character.hair.Equals(-1))
        {
            // 커스터마이징 안되어 있음
            customizeCanvas.SetActive(true);
        }
        else if (GameMng.I.userData.job.Equals(0))
        {
            // 직업 선택 안되어 있음
            SceneManager.LoadScene("TutorialCine");
        }
        else
        {
            loadStart.SetActive(true);
            // 마을로!
            // SceneManager.LoadScene("TutorialDungeon");
            NetworkMng.I.changeRoom(ROOM_CODE.HOME);
        }
    }

    IEnumerator HttpGET(string url, responseFunc success, responseFunc failure)
    {
        UnityWebRequest req = new UnityWebRequest();

        using (req = UnityWebRequest.Get(NetworkMng.DB_URL + url)) {
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

        using  (req = UnityWebRequest.Post(NetworkMng.DB_URL + url, form)) {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError) {
                failure(req.error);
            } else {
                success(req.downloadHandler.text);
            }
        }
    }
}
