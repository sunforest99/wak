using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMap : MonoBehaviour
{
    [SerializeField] GameObject[] npcs;     // 획득시 생겨날 npc 들 (망령)   // Enum의 NPC 데이터와 같아야함

    void Start()
    {
        for (int i = 0; i < GameMng.I.userData.npc.Count; i++)
        {
            npcs[ GameMng.I.userData.npc[i] ].SetActive(true);
        }
    }
}
