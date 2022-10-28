using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDungeonDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            GameMng.I.character.setStopAndReset();
            
            NetworkMng.I.changeRoom(ROOM_CODE.HOME);
        }
    }
}
