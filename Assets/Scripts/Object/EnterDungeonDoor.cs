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
                GameMng.I.showNotice("파티를 해제해주세요. 싱글던전입니다.");
            }
            else {
                NetworkMng.I.changeRoom(moveTo);
            }
        }
    }
}
