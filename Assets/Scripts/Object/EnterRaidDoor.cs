using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRaidDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            NetworkMng.I.VoteChangeScene(ROOM_CODE.RAID_0);
           // SceneManager.LoadScene("BossWakguiScene");
        }
    }
}
