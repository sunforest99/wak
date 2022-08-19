using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRaidDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!NetworkMng.I.v_party.Count.Equals(0))
                NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_0);
            else {
                NetworkMng.I.changeRoom(ROOM_CODE.RAID_0);
                // SceneManager.LoadScene("BossWakguiScene");
            }
        }
    }
}
