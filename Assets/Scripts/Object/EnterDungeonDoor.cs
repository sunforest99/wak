using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 던전 맵으로 이동
public class EnterDungeonDoor : MonoBehaviour
{
    [SerializeField] ROOM_CODE moveTo = ROOM_CODE.DUNGEON_0;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!NetworkMng.I.v_party.Count.Equals(0))
            {
                // TODO : 파티 해제 요청. 파티 상태면 못들어가는 던전임
                
            }
            else {
                // NetworkMng.I.changeRoom(ROOM_CODE.TEMPLE);
                NetworkMng.I.changeRoom(moveTo);

                // SceneManager.LoadScene("BossWakguiScene");
            }
        }
    }
}
