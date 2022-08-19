using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    // 나중에 맵 변경 될때마다 해당 Text UI에 맵 이름 뜨게 함
    [SerializeField] UnityEngine.UI.Text mapNameTxt;        // !< GameObject가 관리해도 괜찮아 보임
    [SerializeField] string mapName = "";
    [SerializeField] bool isFocusingMap = true;

    void Start()
    {
        // 플레이어 생성
        GameMng.I.createMe();

        Debug.Log("@ 파티원 수 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@ " + NetworkMng.I.v_party.Count);
        

        foreach (var party in NetworkMng.I.v_party)
        {
            NetworkMng.I.v_users.Add(
                party.Key, 
                GameMng.I.createPlayer(party.Key, (int)party.Value.job, party.Value.nickName)
            );
        }
        // TEST용
        GameMng.I.noticeMessage.text = NetworkMng.I.v_users.Count + " : " +  NetworkMng.I.v_party.Count + "";

        // 파티 전용맵이 아닌, 글로벌맵에 왔다면 기존 유저들 데이터와 내가 들어옴을 알림
        // if (!NetworkMng.I.IsPartyScene(NetworkMng.I.myRoom))
        // {
        //     Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + NetworkMng.I.myRoom);
        //     Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + NetworkMng.I.IsPartyScene(NetworkMng.I.myRoom));
        //     Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + (ROOM_CODE._PARTY_MAP_ > ROOM_CODE.RAID_0));
        //     NetworkMng.I.SendMsg("REQ_ROOM_DATA");
        // }

        // 캐릭터에게 포커싱(카메라) 해야할 맵인지
        GameMng.I.isFocusing = isFocusingMap;

        // 로딩  끝

        // mapNameTxt.text = mapName;
    }
}
