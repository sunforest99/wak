using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDungeonDoor : MonoBehaviour
{
    [SerializeField] GameObject _mapUI;             // 맵 UI
    [SerializeField] GameObject _prevWall;                              // 이전 벽

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            _prevWall.SetActive(false);
            _mapUI.SetActive(true);

            GameMng.I.character.transform.parent.position = Vector3.zero;
            GameMng.I.character.setStopAndReset();

            GameMng.I._keyMode = KEY_MODE.UI_MODE;
            // GameMng.I.npcUI._startLoad.SetActive(true);
        }
    }
}
