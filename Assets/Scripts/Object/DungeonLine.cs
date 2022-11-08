using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLine : MonoBehaviour
{
    [SerializeField] GameObject prevWall;
    [SerializeField] GameObject lockEff;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (prevWall)
                prevWall.SetActive(true);
            if (DungeonMng._leftMonster > 0)
                lockEff.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            lockEff.SetActive(false);
        }
    }
}
