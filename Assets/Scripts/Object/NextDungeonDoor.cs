using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDungeonDoor : MonoBehaviour
{
    [SerializeField] GameObject _mapUI;             // 맵 UI

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            _mapUI.SetActive(true);

            GameMng.I.character.transform.parent.position = Vector3.zero;

            GameMng.I._keyMode = KEY_MODE.UI_MODE;
        }
    }
}
