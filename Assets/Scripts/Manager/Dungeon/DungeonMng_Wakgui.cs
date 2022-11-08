using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMng_Wakgui : DungeonMng
{
    [Space(20)][Header("[  몬스터 프리팹  ]")]  // ==========================================================================================================================
    [SerializeField] GameObject monster_seadu;          // 심해두 몬스터 prefab
    [SerializeField] GameObject monster_seaduNew;       // 뉴심해두 몬스터 prefab
    [SerializeField] GameObject monster_octopusdu;      // 문어두 몬스터 prefab
    
    protected override void dungeonMonster()
    {
        switch (Random.Range(0, 8))
        {
            case 0:         // 뉴심해두 2
                Vector3[] posArr = {
                    new Vector3(-4.16f, 1.325f, 10),
                    new Vector3(4.16f, 1.325f, 11)
                };

                for (int i = 0; i < posArr.Length; i++)
                {
                    Instantiate(
                        monster_seaduNew,
                        posArr[i],
                        Quaternion.identity
                    );
                }

                _leftMonster = posArr.Length;
                break;
            case 1:         // 심해두1 뉴심해두 1
                Instantiate(
                    monster_seaduNew,
                    new Vector3(-4.6f, 1.325f, 11),
                    Quaternion.identity
                );
                Instantiate(
                    monster_seadu,
                    new Vector3(5f, 1.26f, 9.2f),
                    Quaternion.identity
                );

                _leftMonster = 2;
                break;
            case 2:         // 심해두 2
                Vector3[] posArr2 = {
                    new Vector3(-3f, 1.26f, 9),
                    new Vector3(3f, 1.26f, 9.2f)
                };
                for (int i = 0; i < posArr2.Length; i++)
                {
                    Instantiate(
                        monster_seadu,
                        posArr2[i],
                        Quaternion.identity
                    );
                }

                _leftMonster = posArr2.Length;
                break;
            case 3:             // 문어두 1
                Instantiate(
                    monster_octopusdu,
                    new Vector3(3f, 1.26f, 9.2f),
                    Quaternion.identity
                );
                _leftMonster = 1;
                break;
            case 4:             // 심해두 1 문어두 1
                Instantiate(
                    monster_octopusdu,
                    new Vector3(-3f, 1.26f, 11),
                    Quaternion.identity
                );
                Instantiate(
                    monster_seadu,
                    new Vector3(5f, 1.26f, 9.2f),
                    Quaternion.identity
                );

                _leftMonster = 2;
                break;
            case 5:             // 뉴심해두 1 문어두 1
                Instantiate(
                    monster_octopusdu,
                    new Vector3(-3f, 1.26f, 11),
                    Quaternion.identity
                );
                Instantiate(
                    monster_seaduNew,
                    new Vector3(4.9f, 1.325f, 10),
                    Quaternion.identity
                );

                _leftMonster = 2;
                break;
            case 6:             // 심해두1 뉴심해두 1
                Instantiate(
                    monster_seaduNew,
                    new Vector3(5.3f, 1.325f, 10),
                    Quaternion.identity
                );
                Instantiate(
                    monster_seadu,
                    new Vector3(-5f, 1.26f, 9f),
                    Quaternion.identity
                );

                _leftMonster = 2;
                break;
            case 7:             // 뉴심해두 3
                Vector3[] posArr3 = {
                    new Vector3(-5.4f, 1.325f, 8),
                    new Vector3(5.16f, 1.325f, 8),
                    new Vector3(-0.2f, 1.325f, 13)
                };

                for (int i = 0; i < posArr3.Length; i++)
                {
                    Instantiate(
                        monster_seaduNew,
                        posArr3[i],
                        Quaternion.identity
                    );
                }

                _leftMonster = posArr3.Length;
                break;
        }

        
    }
    protected override void dungeonMonsterPurple() {
        Instantiate(purpleLight, new Vector3(0, 0.88f, 10), Quaternion.Euler(20, 0, 0));

        dungeonMonster();
    }

    protected override void dungeonNPC() {
        List<int> notGainNPC = new List<int>();

        for (int i = 0; i < (int)NPC.__NONE__; i++)
        {
            // 획득 NPC 들 중에 포함되어 있지 않다면
            if (!GameMng.I.userData.npc.Contains(i))
            {
                notGainNPC.Add(i);
            }
        }

        if (notGainNPC.Count > 0)
        {
            int rand = Random.Range(0, notGainNPC.Count);
            
            GameObject npc_o = Instantiate(npc[notGainNPC[rand]], new Vector3(0, 1.44f, 8.256f), Quaternion.Euler(20, 0, 0));
            npcAnim = npc_o.GetComponent<Animator>();
            npc_o.GetComponent<BoxCollider>().enabled = false;

            GameMng.I.userData.npc.Add(notGainNPC[rand]);

            prison.SetActive(true);
        }
    }
    protected override void dungeonRandom() {}

    // public void someoneDamage(string name, int dmg)
    // {
    //     GameMng.I._monsters[name].getDamage(dmg);
    // }
}