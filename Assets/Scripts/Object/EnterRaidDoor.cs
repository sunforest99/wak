using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRaidDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!NetworkMng.I.v_party.Count.Equals(0))
                NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_0);
            else {
                // NetworkMng.I.changeRoom(ROOM_CODE.TEMPLE);
                NetworkMng.I.changeRoom(ROOM_CODE.RAID_0);

                // SceneManager.LoadScene("BossWakguiScene");
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
