using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    [SerializeField] GameObject fishingGame;
    [SerializeField] GameObject alertMsg;
    bool isFishingReady = false;

    void Update() {
        if (isFishingReady) {
            if (Input.GetKeyDown(KeyCode.Z) && GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE)) {
                fishingGame.SetActive(true);
                GameMng.I._keyMode = KEY_MODE.UI_MODE;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            alertMsg.transform.position = GameMng.I.character.transform.parent.position + new Vector3(0, 3, 0);
            alertMsg.SetActive(true);
            isFishingReady = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            alertMsg.SetActive(false);
            isFishingReady = false;
        }
    }
}
