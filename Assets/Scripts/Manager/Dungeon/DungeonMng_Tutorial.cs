using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMng_Tutorial : DungeonMng
{
    [Space(20)][Header("[  몬스터 프리팹  ]")]  // ==========================================================================================================================
    [SerializeField] GameObject monster_seadu;          // 심해두 몬스터 prefab

    [SerializeField] GameObject tutorial_npc;           // 튜토리얼에서 획득하는 npc (뢴트게늄)

    // [SerializeField]

    int monsterStage = 0;

    protected override void Start()
    {
        // 플레이어 생성
        // 다른 씬과는 다르게 만들어져 있는 캐릭터를 커마 수정함

        // GameMng.I.createMe();
        // GameMng.I.character.transform.position = new Vector3(-54, 0, 8.5f);

        GameMng.I._keyMode = KEY_MODE.UI_MODE;
        // base.Start();
        
        // GameMng.I.character.setMoveDir(1, 0);
        // GameMng.I.character.startMove();
    }

    protected override void dungeonMonster()
    {
        if (monsterStage.Equals(0))
        {
            Vector3[] posArr = {
                new Vector3(-4.16f, 1.325f, 10),
                new Vector3(-2.04f, 1.325f, 5.62f)
            };

            for (int i = 0; i < posArr.Length; i++)
            {
                // 뉴심해두 몬스터 생성
                Instantiate(
                    monster_seadu,
                    posArr[i],
                    Quaternion.identity
                );
            }

            _leftMonster = posArr.Length;
        }
        else
        {
            Vector3[] posArr = {
                new Vector3(-3f, 1.26f, 9),
                new Vector3(3f, 1.26f, 9.2f)
            };

            for (int i = 0; i < posArr.Length; i++)
            {
                // 뉴심해두 몬스터 생성
                Instantiate(
                    monster_seadu,
                    posArr[i],
                    Quaternion.identity
                );
            }

            _leftMonster = posArr.Length;
        }

        monsterStage++;
    }

    protected override void dungeonNPC() {
        tutorial_npc.SetActive(true);
    }
    
}