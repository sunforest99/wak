using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 정비소. 레이드 입장전 맵으로 이동
public class EnterRepairDoor : MonoBehaviour
{
    [SerializeField] ROOM_CODE moveTo = ROOM_CODE.RAID_0_REPAIR;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!NetworkMng.I.v_party.Count.Equals(0))
                NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_0_REPAIR);
            else {
                // NetworkMng.I.changeRoom(ROOM_CODE.TEMPLE);
                NetworkMng.I.changeRoom(moveTo);

                // SceneManager.LoadScene("BossWakguiScene");
            }
        }
    }
}
