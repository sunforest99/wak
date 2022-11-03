using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRaidDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!NetworkMng.I.v_party.Count.Equals(0)) {
                switch (NetworkMng.I.myRoom)
                {
                    case ROOM_CODE.RAID_0_REPAIR:
                        NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_0);
                        break;
                    case ROOM_CODE.RAID_1_REPAIR:
                        NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_1);
                        break;
                    case ROOM_CODE.RAID_2_REPAIR:
                        NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_2);
                        break;
                }
            }
            else {
                GameMng.I.showNotice("파티원 4명이 모두 준비되어야 합니다.");
                // TODO : 아래는 꼭 삭제하기
                switch (NetworkMng.I.myRoom)
                {
                    case ROOM_CODE.RAID_0_REPAIR:
                        NetworkMng.I.changeRoom(ROOM_CODE.RAID_0);
                        break;
                    case ROOM_CODE.RAID_1_REPAIR:
                        NetworkMng.I.changeRoom(ROOM_CODE.RAID_1);
                        break;
                    case ROOM_CODE.RAID_2_REPAIR:
                        NetworkMng.I.changeRoom(ROOM_CODE.RAID_2);
                        break;
                }
            }
        }
    }


    /**
     * @brief 레이드 도전 중단 UI
     */
    public void stopRaid()
    {
        if (!NetworkMng.I.v_party.Count.Equals(0))
            NetworkMng.I.VoteChangeScene(ROOM_CODE.HOME);
        else
            NetworkMng.I.changeRoom(ROOM_CODE.HOME);
    }
}
