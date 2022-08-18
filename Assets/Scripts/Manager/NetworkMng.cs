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
    public ROOM_CODE myRoom = ROOM_CODE.RAID_0_REPAIR;                  // 현재 내 위치
    public string uniqueNumber = "";        // 나 자신을 가리키는 고유 숫자
    public Dictionary<string, Character> v_users = new Dictionary<string, Character>();        // 맵에 같이 있는 유저들
    public Dictionary<string, PartyData> v_party = new Dictionary<string, PartyData>();        // 파티원들  (v_users안에도 파티원들이 있긴함)
    public bool roomOwner = false;
    int voteAgree = 0, voteRefuse = 0;

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
        
        GameMng.I.noticeMessage.text = msg;

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

            if (txt.Equals("IN_USER"))       // 유저데이터가 없다면 맵에 나밖에 없음 현재
                return;
            
            for (int i = 1; i < txt.Length; i += 5)
            {
                v_users.Add(
                    txt[i], 
                    GameMng.I.createPlayer(txt[i], int.Parse(txt[i + 1]), txt[i + 2], float.Parse(txt[i + 3]), float.Parse(txt[i + 4]))
                );
            }
        }
        else if (txt[0].Equals("LOGOUT"))
        {
            // 내 파티에 속한 사람이 나갔다면
            if (v_party.ContainsKey(txt[1]))
            {
                v_party.Remove(txt[1]);
                SomeoneExitParty(txt[1]);
            }

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
            InviteParty(txt[1], txt[2]);
        }
        else if (txt[0].Equals("AGREE_PARTY"))
        {
            // 파티 수락누른 후 정상적으로 파티에 참가됨
            // txt[1 + n0] 파티원 uniqueNumber
            // txt[1 + n1] 파티원 닉네임
            // txt[1 + n2] 파티원 직업
            AgreeParty(txt);
        }
        else if (txt[0].Equals("ADD_PARTY"))
        {
            // 누군가 파티에 들어옴
            // txt[1] 파티원 uniqueNumber
            // txt[2] 파티원 닉네임
            // txt[3] 파티원 직업
            AddParty(txt[1], txt[2], txt[3]);
        }
        else if (txt[0].Equals("EXIT_PARTY"))
        {
            // 누군가 파티에서 나감
            // txt[1] 나간 파티원 uniqueNumber
            SomeoneExitParty(txt[1]);
        }
        else if (txt[0].Equals("REQ_VOTE_ROOM_CHANGE"))
        {
            // 방 변경하기로 파티원이 투표함
            // txt[1] 이동할 방 idx
            voteAgree = 1;
            voteRefuse = 0;

            // 투표 alert창 띄우기
            GameMng.I.alertMapChange(txt[1]);
        }
        else if (txt[0].Equals("VOTE_ROOM_CHANGE"))
        {
            // 방 변경 누가 찬성/반대함
            if (txt[1].Equals("1"))
                voteAgree++;
            else
                voteRefuse++;
            
            GameMng.I.alertMessage.text = string.Format("파티원이 맵 이동을 권유합니다. \n 수락 : {0}  거절 : {1}", voteAgree, voteRefuse);
            
            // 모두 투표
            if (voteAgree + voteRefuse == v_party.Count + 1)
            {
                GameMng.I.alertMessage.transform.parent.gameObject.SetActive(false);
                GameMng.I.agreeBT.interactable = true;
                GameMng.I.refuseBT.interactable = true;
                // 모두 찬성
                if (voteAgree == v_party.Count + 1)
                {
                    changeRoom((ROOM_CODE)Enum.Parse(typeof(ROOM_CODE), GameMng.I.alertMessage.name));
                }
                else
                {
                    // 반대가 있음 -> 거절됨
                    GameMng.I.showNotice("맵 이동을 모두 찬성해야 합니다.");
                }
            }
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
        else if (txt[0].Equals("BOSS_PATTERN"))
        {
            // txt[1] 보스 패턴
            // txt[2~] { 패턴에 필요한 데이터 }
        }
        else if (txt[0].Equals("NOTICE"))
        {
            GameMng.I.showNotice(txt[1]);
        }
        else if (txt[0].Equals("YOUR_OWNER"))
        {
            // 파티 오너 (보스 패턴을 관리함)
            roomOwner = true;

            if (IsPartyScene((ROOM_CODE)myRoom))
            {
                // 파티원 전용맵에서 파티 오너가 들어왔다는 것은 방의 메세지를 관리하던 파장이 튕긴것이므로 내가 이제 대신 메시지를 관리해야함
                /// 보스맵이라면 보스 패턴들 활성화 시켜서 보내게..
            }
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
            // if (myRoom != 0)
            //     SendMsg(string.Format("OUT_ROOM:{0}", myRoom));
            SendMsg("DISCONNECT");
            Thread.Sleep(500);
            socket.Close();
        }
        StopCoroutine("PacketProc");
    }

    /**
     * @brief 방을 바뀌게 하였을때 호출할것 ( 바뀔 방의 정보를 가져와 새로고침하게 함 )
     */ 
    public void changeRoom(ROOM_CODE changeToRoom)
    {
        // (내가 현재 있는맵)이 파티 전용맵이라면 기존 방 유저들에게 내가 사라지는 메세지를 보낼 필요가없음.
        //  파티 전용맵인지 구분해서 메세지 간소화
        // SendMsg(string.Format("{0}:{1}", 
        //     IsPartyScene(myRoom) ? "CHANGE_ROOM_PARTY" : "CHANGE_ROOM",
        //     (int)changeToRoom
        // ));

        v_users.Clear();

        myRoom = changeToRoom;  

        // 씬 변경되는 과정에서 기타 데이터들이 들어올 수도 있음을 방지하기 위해 제일 먼저 보냄
        SendMsg(string.Format("CHANGE_ROOM:{0}", (int)changeToRoom));

        ChangeScene(changeToRoom);

        // 이후 변경된 새 방의 LoadManager 의 Start() 에서 관리
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

    /**
     * @brief 씬 변경
     * @param roomCode 코드에 알맞는 씬으로 이동
     */
    public void ChangeScene(ROOM_CODE roomCode)
    {
        switch (roomCode)
        {
            case ROOM_CODE.RAID_0:
                SceneManager.LoadScene("BossWakguiScene");
                break;
            case ROOM_CODE.RAID_0_REPAIR:
                break;
            case ROOM_CODE.RAID_1:
                break;
        }
    }

    /**
     * @brief 파티 전용 씬(맵)인지 확인
     * @param roomCode 확인할 맵
     * @return 파티 전용이라면 true
     */
    public bool IsPartyScene(ROOM_CODE roomCode)
    {
        return ROOM_CODE._PARTY_MAP_ < roomCode;
    }

    /**
     * @brief 씬을 변경하고자 할때 파티원들이 있다면 투표하기
     * @param roomCode 이동하고자 하는 씬
     */
    public void VoteChangeScene(ROOM_CODE roomCode)
    {
        if (GameMng.I.alertMessage.transform.parent.gameObject.activeSelf) {
            GameMng.I.showNotice("방 변경을 투표하지 못하는 상황입니다.");
        }
        SendMsg(string.Format("REQ_VOTE_ROOM_CHANGE:{0}", (int)roomCode));
        
        voteAgree = 1;
        voteRefuse = 0;
        GameMng.I.alertMessage.name = (int)roomCode + "";       // 변경할 방 코드 임시 저장
        GameMng.I.alertMessage.text = "파티원에게 맵 이동 권유중... \n 수락 : 1  거절 : 0";
        GameMng.I.alertMessage.transform.parent.gameObject.SetActive(true);
        GameMng.I.agreeBT.interactable = false;
        GameMng.I.refuseBT.interactable = false;
    }

    /**
     * @brief 누군가 파티초대를 보냈을떄
     * @param sendUserNickname 파티초대를 보낸 유저의 닉네임
     * @param sendUserUniqueNumber 파티초대를 보낸 유저의 uniqueNumber
     */
    void InviteParty(string sendUserNickname, string sendUserUniqueNumber)
    {
        GameMng.I.alertMessage.text = sendUserNickname + "님이 당신을 파티에 초대했습니다.";
        GameMng.I.alertMessage.transform.parent.gameObject.SetActive(true);
        GameMng.I.agreeBT.onClick.RemoveAllListeners();
        GameMng.I.agreeBT.onClick.AddListener(() => {
            SendMsg(string.Format("RESPONSE_PARTY:1:{0}", sendUserUniqueNumber));
            GameMng.I.alertMessage.transform.parent.gameObject.SetActive(false);
        });
        GameMng.I.refuseBT.onClick.RemoveAllListeners();
        GameMng.I.refuseBT.onClick.AddListener(() => {
            SendMsg(string.Format("RESPONSE_PARTY:0:{0}", sendUserUniqueNumber));
            GameMng.I.alertMessage.transform.parent.gameObject.SetActive(false);
        });
    }

    /**
     * @brief 파티 수락후 정상적으로 파티 가입이 될때 호출됨
     * @param txt[1 + n0] 파티원 uniqueNumber
     * @param txt[1 + n1] 파티원 닉네임
     * @param txt[1 + n2] 파티원 직업
     */
    void AgreeParty(string[] txt)
    {
        GameMng.I.stateMng.PartyHPImg[0].transform.parent.gameObject.SetActive(true);   // 나 추가
        GameMng.I.stateMng.PartyName[0].text = GameMng.I.userData.user_nickname;
        for (int i = 1; i < txt.Length; i += 3)
        {
            v_party.Add(
                txt[i],
                new PartyData(txt[i + 1], (JOB)Enum.Parse(typeof(JOB), txt[i + 2]))
            );
            GameMng.I.stateMng.PartyHPImg[v_party.Count].transform.parent.localPosition = new Vector3(0, 60 - 40 * i, 0);
            GameMng.I.stateMng.PartyName[v_party.Count].text = txt[i + 1];
            GameMng.I.stateMng.PartyName[v_party.Count].name = txt[i];      // 오브젝트 이름에 uniqueNumber 담기
            GameMng.I.stateMng.PartyHPImg[v_party.Count].transform.parent.gameObject.SetActive(true);   // 파티원 추가
        }
    }

    /**
     * @brief 파티원이 한명 더 들어옴
     * @param newUniqueNumber 새로 들어온 유저의 uniqueNumber
     * @param newNickname 새로 들어온 유저의 닉네임
     * @param newJob 새로 들어온 유저의 직업
     */
    void AddParty(string newUniqueNumber, string newNickname, string newJob)
    {
        v_party.Add(
            newUniqueNumber,
            new PartyData(newNickname, (JOB)Enum.Parse(typeof(JOB), newJob))
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
                GameMng.I.stateMng.PartyName[i].text = newNickname;
                GameMng.I.stateMng.PartyName[i].name = newUniqueNumber;
                GameMng.I.stateMng.PartyHPImg[i].transform.parent.gameObject.SetActive(true);   // 파티원 추가
                break;
            }
        }
    }

    /**
     * @brief 누군가 파티에서 나갔을때 사용
     * @param exitUserUniqueNumber 나간 유저의 uniqueNumber
     */
    void SomeoneExitParty(string exitUserUniqueNumber)
    {
        // index 찾아서 그 뒤에 있는 유저들은 위로 끌어올리기
        for (int i = 1; i < v_party.Count + 1; i++) // 나는 항상 첫번째니까 제외
        {
            if (GameMng.I.stateMng.PartyName[i].name.Equals(exitUserUniqueNumber))
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
        v_party.Remove(exitUserUniqueNumber);
    }
}
