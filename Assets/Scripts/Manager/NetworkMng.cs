using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct PartyData
{
    public string nickName;
    public JOB job;
    // 장착 아이템들 (머리, 무기..)
    public PartyData(string nickName, JOB job)
    {
        this.nickName = nickName;
        this.job = job;
    }
}

public class NetworkMng : MonoBehaviour
{
    // 네트워크 기본 데이터 ==================================================================================================
    static Socket socket = null;
    public string address = "127.0.0.1";    // 주소, 서버 주소와 같게 할 것
    int port = 10000;                       // 포트 번호, 서버포트와 같게 할 것
    byte[] buf = new byte[4096];
    int recvLen = 0;

    // 유저 데이터 =========================================================================================================
    public int myRoom = 0;                  // 현재 내 위치
    public string uniqueNumber = "";        // 나 자신을 가리키는 고유 숫자
    Dictionary<string, Character> v_users = new Dictionary<string, Character>();        // 맵에 같이 있는 유저들
    Dictionary<string, PartyData> v_party = new Dictionary<string, PartyData>();        // 파티원들  (v_users안에도 파티원들이 있긴함)
    public bool roomOwner = false;


    static NetworkMng _instance;
    public static NetworkMng I
    {
        get
        {
            if (_instance.Equals(null))
            {
                Debug.LogError("Instance is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;

    }

    void Start() {
        Login();
    }

    /**
     * @brief 서버에 접속 
     */
    public void Login()
    {
        if (checkNetwork())
        {
            Logout();       // 이중 접속 방지

            IPAddress serverIP = IPAddress.Parse(address);
            int serverPort = Convert.ToInt32(port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);      // 송신 제한시간 10초
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);   // 수신 제한시간 10초

            // 서버가 닫혀 있을것을 대비하여 예외처리
            try
            {
                socket.Connect(new IPEndPoint(serverIP, serverPort));
                StartCoroutine("PacketProc");

                // 정상
                // SceneManager.LoadScene("Main");
            }
            catch (SocketException err)
            {
                Debug.Log("서버가 닫혀있습니다. : " + err.ToString());
                Logout();
            }
            catch (Exception ex)
            {
                Debug.Log("ERROR 개반자에게 문의 : " + ex.ToString());
                Logout();
            }
        }
        else
        {
            // 로그인 메세지 대기
        }
    }

    /**
     * @brief 접속 종료 
     */
    public void Logout()
    {
        if (socket != null && socket.Connected)
            socket.Close();
        StopCoroutine("PacketProc");
    }

    /**
     * @brief 채팅
     * @param input 내용
     */
    public void Chat(InputField input)
    {
        SendMsg(string.Format("CHAT:{0}", input.text));
    }

    /**
     * @brief 접속 종료 
     * @param nickName 이름
     * @param pos 생성 위치
     * @param isPlayer 나 인가 아닌가
     */
    public void CreateUser(string nickName, Vector3 pos, int job, string uniqueNumber)
    {
        GameObject obj = Instantiate(GameMng.I.characterPrefab[job], pos, Quaternion.identity) as GameObject;
        Character player = obj.GetComponent<Character>();
        player._job = (JOB)job;

        v_users.Add(uniqueNumber, player);
    }

    /**
     * @brief 서버에게 패킷 전달
     * @param txt 패킷 내용
     */
    public void SendMsg(string txt)
    {
        try
        {
            if (socket != null && socket.Connected)
            {
                byte[] buf = new byte[4096];

                Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(txt).Length + 2), 0, buf, 0, 2);

                Buffer.BlockCopy(Encoding.UTF8.GetBytes(txt), 0, buf, 2, Encoding.UTF8.GetBytes(txt).Length);

                socket.Send(buf, Encoding.UTF8.GetBytes(txt).Length + 2, 0);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    /**
     * @brief 패킷 처리 업데이트
     */
    IEnumerator PacketProc()
    {
        while (true)
        {
            if (socket.Connected)
                if (socket.Available > 0)
                {
                    byte[] buf = new byte[4096];
                    int nRead = socket.Receive(buf, socket.Available, 0);

                    if (nRead > 0)
                    {
                        Buffer.BlockCopy(buf, 0, this.buf, recvLen, nRead);
                        recvLen += nRead;

                        while (true)
                        {
                            int len = BitConverter.ToInt16(this.buf, 0);

                            if (len > 0 && recvLen >= len)
                            {
                                ParsePacket(len);
                                recvLen -= len;
                                Buffer.BlockCopy(this.buf, len, this.buf, 0, recvLen);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            yield return null;
        }
    }

    /**
     * @brief 패킷 분석
     */
    public void ParsePacket(int len)
    {
        string msg = Encoding.UTF8.GetString(this.buf, 2, len - 2);
        string[] txt = msg.Split(':');
        
        if (txt[0].Equals("ADD_USER"))
        {
            // 방에 새로 들어온 유저
            // ADD_USER : 새로온유저uniqueNumber : 직업 : 닉네임
            v_users.Add(
                txt[1], 
                GameMng.I.createPlayer(txt[1], int.Parse(txt[2]), txt[3])
            );
        }
        else if (txt[0].Equals("SKILL"))
        {
            // txt[2] 스킬코드
            // txt[3,4] 스킬사용방향
            v_users[txt[1]].useSkill(
                (SKILL_CODE)Enum.Parse(typeof(SKILL_CODE), txt[2]),
                new Vector2(float.Parse(txt[3]), float.Parse(txt[4]))
            );
        }
        else if (txt[0].Equals("CHAT"))
        {
            // 채팅 UI 에 추가
            GameMng.I.chatMng.newMessage(txt[1], msg.Substring(6 + txt[1].Length));     // 닉네임 뒤는 모두 문자로 취급 (: 있는 메세지 방지)
        }
        else if (txt[0].Equals("DAMAGE"))
        {
            // DAMAGE : 데미지를_입힌_타겟 : 데미지_수치

            // if (보스맵인가?)
            //     보스한테 데미지 입힘
            // else if (필드맵인가?)
            //     필드맵의 몬스터 누구에게 데미지 입힘
        }
        else if (txt[0].Equals("MOVE_START"))
        {
            // MOVE_START : 유저uniqueNumber : 방향x좌표 : 방향y좌표
            v_users[txt[1]].setMoveDir(int.Parse(txt[2]), int.Parse(txt[3]));
            v_users[txt[1]].startMove();
        }
        else if (txt[0].Equals("MOVE"))
        {
            // MOVE : 유저uniqueNumber : 방향x좌표 : 방향y좌표 : 캐릭터x좌표 : 캐릭터y좌표
            v_users[txt[1]].setMoveDir(int.Parse(txt[2]), int.Parse(txt[3]));
        }
        else if (txt[0].Equals("MOVE_STOP"))
        {
            // MOVE_STOP : 유저uniqueNumber : 캐릭터x좌표 : 캐릭터y좌표
            v_users[txt[1]].setMoveDir(0, 0);
            v_users[txt[1]].stopMove();
            v_users[txt[1]].transform.position = new Vector3(float.Parse(txt[2]), float.Parse(txt[3]));
        }
        else if (txt[0].Equals("IN_USER"))  // 기존 맵에 있는 유저들 데이터
        {
            // TODO : 이 메세지를 받으려면 서버에 "CHANGE_ROOM"을 호출해야 하는데 씬을 이동한 후(LoadManager)에 받을 것. 이 메세지를 받았다면 로딩창을 내려도 됨

            // 고유uniqueNumber : 직업 : 닉네임

            Debug.Log(" " + txt.Length);
            if (txt.Length.Equals(2))       // 유저가 아무도 없으면 2가 됨
                return;
            
            for (int i = 1; i < txt.Length; i += 5)
            {
                v_users.Add(
                    txt[i], 
                    GameMng.I.createPlayer(txt[1], int.Parse(txt[i + 1]), txt[i + 2], float.Parse(txt[i + 3]), float.Parse(txt[i + 4]))
                );
            }
        }
        else if (txt[0].Equals("LOGOUT"))
        {
            Destroy(v_users[txt[1]].gameObject);
            v_users.Remove(txt[1]);
        }
        else if (txt[0].Equals("CHANGE_HP"))
        {
            // 파티원 HP가 변경됨
            // txt[1] 변경된 파티원 uniqueNumber
            // txt[2] 변경된 체력 퍼센트
        }
        else if (txt[0].Equals("INVITE_PARTY"))
        {
            // 파티 초대 왔음
            // txt[1] 초대한 사람 닉네임
            // txt[2] 초대한 사람 uniqueNumber
            GameMng.I.alertMessage.text = txt[1] + "님이 당신을 파티에 초대했습니다.";
            GameMng.I.alertMessage.transform.parent.gameObject.SetActive(true);
            GameMng.I.agreeBT.onClick.RemoveAllListeners();
            GameMng.I.agreeBT.onClick.AddListener(() => {
                NetworkMng.I.SendMsg(string.Format("RESPONSE_PARTY:1:{0}", txt[2]));
                GameMng.I.alertMessage.transform.parent.gameObject.SetActive(false);
            });
            GameMng.I.refuseBT.onClick.RemoveAllListeners();
            GameMng.I.refuseBT.onClick.AddListener(() => {
                NetworkMng.I.SendMsg(string.Format("RESPONSE_PARTY:0:{0}", txt[2]));
                GameMng.I.alertMessage.transform.parent.gameObject.SetActive(false);
            });
        }
        else if (txt[0].Equals("AGREE_PARTY"))
        {
            // 파티 수락누른 후 정상적으로 파티에 참가됨
            // txt[1 + n0] 파티원 uniqueNumber
            // txt[1 + n1] 파티원 닉네임
            // txt[1 + n2] 파티원 직업
            GameMng.I.stateMng.PartyHPImg[0].transform.parent.gameObject.SetActive(true);   // 나 추가
            GameMng.I.stateMng.PartyName[0].text = GameMng.I.userData.user_nickname;
            for (int i = 1; i < txt.Length; i += 3)
            {
                v_party.Add(
                    txt[i],
                    new PartyData(txt[i + 1], (JOB)Enum.Parse(typeof(JOB), txt[i + 2]))
                );
                GameMng.I.stateMng.PartyHPImg[v_party.Count].transform.parent.gameObject.SetActive(true);   // 파티원 추가
                GameMng.I.stateMng.PartyName[v_party.Count].text = txt[i + 1];
                GameMng.I.stateMng.PartyName[v_party.Count].name = txt[i];      // 오브젝트 이름에 uniqueNumber 담기
            }
        }
        else if (txt[0].Equals("ADD_PARTY"))
        {
            // 누군가 파티에 들어옴
            // txt[1] 파티원 uniqueNumber
            // txt[2] 파티원 닉네임
            // txt[3] 파티원 직업
            v_party.Add(
                txt[1],
                new PartyData(txt[2], (JOB)Enum.Parse(typeof(JOB), txt[3]))
            );

            if (v_party.Count.Equals(1))    // 파티원잉 처음 한명 들어온 것이기에 내 UI도 추가로 켜줘야함
            {
                GameMng.I.stateMng.PartyHPImg[0].transform.parent.gameObject.SetActive(true);   // 나 추가
                GameMng.I.stateMng.PartyName[0].text = GameMng.I.userData.user_nickname;
            }

            for (int i = 1; i < 4; i++)
            {
                if (!GameMng.I.stateMng.PartyHPImg[i].transform.parent.gameObject.activeSelf)
                {
                    GameMng.I.stateMng.PartyHPImg[i].transform.parent.localPosition = new Vector3(0, 60 - 40 * i, 0);
                    GameMng.I.stateMng.PartyName[i].text = txt[2];
                    GameMng.I.stateMng.PartyName[i].name = txt[1];
                    GameMng.I.stateMng.PartyHPImg[i].transform.parent.gameObject.SetActive(true);   // 파티원 추가
                    break;
                }
            }
        }
        else if (txt[0].Equals("EXIT_PARTY"))
        {
            // 누군가 파티에서 나감
            // txt[1] 나간 파티원 uniqueNumber
            
            // index 찾아서 그 뒤에 있는 유저들은 위로 끌어올리기
            
            for (int i = 1; i < v_party.Count + 1; i++) // 나는 항상 첫번째니까 제외
            {
                if (GameMng.I.stateMng.PartyName[i].name.Equals(txt[1]))
                {
                    GameMng.I.stateMng.PartyHPImg[i].transform.parent.gameObject.SetActive(false);   // 파티원 끄기
                    // 나간 파티원들 뒤에 있는 순번 파티원들을 모두 앞으로 당기기
                    for (int j = i; j < v_party.Count; j++)
                    {
                        GameMng.I.stateMng.PartyHPImg[j + 1].transform.parent.position = new Vector3(0, 60 - 40 * j, 0);    // 뒤에 있는 파티원들 앞으로
                    }
                    break;
                }
            }
            GameMng.I.stateMng.PartyHPImg[v_party.Count].transform.parent.gameObject.SetActive(false);   // 파티원 제거
            v_party.Remove(txt[1]);
        }
        else if (txt[0].Equals("REQ_VOTE_ROOM_CHANGE"))
        {
            // 방 변경하기로 파티원이 투표함
            // txt[1] 이동할 방 idx
        }
        else if (txt[0].Equals("VOTE_ROOM_CHANGE"))
        {
            // 방 변경 누가 찬성/반대함
            // txt[1] == "1" 찬성, "0" 반대
            
            // if 모두 찬성
            //!! 일반적으로 방 변경할때는 CHANGE_ROOM이나, 레이드 정비소 및 보스재입장 시에는 CHANGE_ROOM_PARTY 을 호출해야함
        }
        else if (txt[0].Equals("ESTHER"))
        {
            // 에스더 사용
            // txt[1] 에스더 번호
            // txt[2] 에스더 소환 자
            // txt[3] 에스더 소환
            GameMng.I.estherManager.useEsther(
                int.Parse(txt[1]),
                v_users[txt[2]].transform.position
            );
        }
        else if (txt[0].Equals("ESTHER_DAMAGE"))
        {
            // DAMAGE와 다르게 데미지 표시도 해줌
        }
        else if (txt[0].Equals("CHANGE_ROOM"))
        {
            // 방 변경 (파티 없이 혼자일때 들어옴 아마)
            v_users.Clear();

            // 이후 변경된 새 방의 LoadManager 의 Start() 에서 관리
        }
        else if (txt[0].Equals("BOSS_PATTERN"))
        {
            // txt[1] 보스 패턴
            // txt[2~] { 패턴에 필요한 데이터 }
        }
        else if (txt[0].Equals("YOUR_OWNER"))
        {
            // 파티 오너 (보스 패턴을 관리함)
            roomOwner = true;
        }
        else if (txt[0].Equals("CONNECT"))
        {
            // TODO : 기타 다른 정보까지 알려줄일 있으면 알려줘야함
            SendMsg(string.Format("LOGIN:{0}:{1}", GameMng.I.userData.user_nickname, GameMng.I.userData.job));
        }
        else if (txt[0].Equals("UNIQUE"))
        {
            this.uniqueNumber = txt[1];
        }
    }

    /**
     * @brief 기기에서 접속을 끊었을때 
     */
    void OnDestroy()
    {
        if (socket != null && socket.Connected)
        {
            // 광장이 아니였을 때
            if (myRoom != 0)
                SendMsg(string.Format("OUT_ROOM:{0}", myRoom));
            SendMsg("DISCONNECT");
            Thread.Sleep(500);
            socket.Close();
        }
        StopCoroutine("PacketProc");
    }

    /**
     * @brief 방을 바뀌게 하였을때 호출할것 ( 바뀔 방의 정보를 가져와 새로고침하게 함 )
     */
    public void changeRoom(int changeToRoom)
    {
        v_users.Clear();

        myRoom = changeToRoom;  

        SendMsg(string.Format("CHANGE_ROOM:{0}", changeToRoom));

        // 방 변경후 다음 데이터는 서버에서 보내줌
    }

    /**
     * @brief 게임내 로그아웃, 접속 종료
     */
    public void LogOutBT()
    {
        OnDestroy();
        // SceneManager.LoadScene(" ");
    }

    /**
     * @brief 인터넷 연결되어 있는지 확인
     */
    public bool checkNetwork()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText.Equals(""))
        {
            // 연결 실패
            Debug.Log("인터넷 연결 실패");
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            // 비정상적인 루트일때
            Debug.Log("인터넷 연결 실패");
        }
        else
        {
            // 성공적인 연결
            Debug.Log("인터넷 연결 되있음");
            return true;
        }

        return false;
    }

    /**
     * @brief html 받아오기
     * @param resource url
     */
    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    /**
     * @brief int 를 2바이트 데이터로 변환
     * @param val 변경할 변수
     */
    public static byte[] ShortToByte(int val)
    {
        byte[] temp = new byte[2];
        temp[1] = (byte)((val & 0x0000ff00) >> 8);
        temp[0] = (byte)((val & 0x000000ff));
        return temp;
    }

    /**
     * @brief 스킬 사용
     * @param skillCode 0~4스킬, 5대쉬, 6기상기, 7평타
     * @param skillDirection 스킬 사용방향,  사용방향이 필요없는 스킬은 00, 평타는 좌우, 나머지는 진짜 방향
     */
    public void UseSkill(SKILL_CODE skillCode, Vector2 skillDirection = new Vector2())
    {
        SendMsg(string.Format("SKILL:{0}:{1}:{2}", skillCode, skillDirection.x, skillDirection.y));
    }
}
