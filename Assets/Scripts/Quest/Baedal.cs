using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baedal : MonoBehaviour
{
    [SerializeField] Sprite baedalSpr;
    [SerializeField] GameObject baedalObj;
    Sprite beforeWeapon;


    [SerializeField] GameObject alertMsg;
    bool isBaedalReady = false;

    void Update() {
        if (isBaedalReady) {
            if (Input.GetKeyDown(KeyCode.Z) && GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE)) {
                EquipBaedal();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            alertMsg.transform.position = GameMng.I.character.transform.parent.position + new Vector3(0, 3, 0);
            alertMsg.SetActive(true);
            isBaedalReady = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            alertMsg.SetActive(false);
            isBaedalReady = false;
        }
    }





    void EquipBaedal()
    {
        GameMng.I.nextSubQuest(QUEST_CODE.BAEDAL);

        baedalObj.SetActive(false);

        beforeWeapon = GameMng.I.character._weapon.sprite;
        
        GameMng.I.character._weapon.sprite = baedalSpr;

        GameMng.I.mainMap.npcdatas[1].checkQuest();
    }

    void OnDestroy()
    {
        GameMng.I.character._weapon.sprite = beforeWeapon;
    }
}
