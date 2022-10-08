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
        // TODO : 좌표 여러개 추가하기
        Vector3[] posArr = {
            new Vector3(-4.16f, 1.325f, 10)
        };

        // TODO : 몬스터 다르게 하기 등록
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
    protected override void dungeonMonsterPurple() {
        // Instantiate(purpleLight, new Vector3(0, 0.88f, 10), Quaternion.Euler(20, 0, 0));

        // // TODO : 몬스터 만들면서 강화 및 회복 능력 부여
        // Vector3[] posArr = {
        //     new Vector3(-3f, 1.26f, 9),
        //     new Vector3(3f, 1.26f, 9.2f)
        // };

        // // TODO : 몬스터 다르게 하기 등록
        // for (int i = 0; i < posArr.Length; i++)
        // {
        //     // 뉴심해두 몬스터 생성
        //     Instantiate(
        //         monster_seadu,
        //         posArr[i],
        //         Quaternion.identity
        //     );
        // }
        
        // _leftMonster = posArr.Length;

        Vector3[] posArr = {
            new Vector3(3f, 1.26f, 9.2f)
        };

        for (int i = 0; i < posArr.Length; i++)
        {
            // 뉴심해두 몬스터 생성
            Instantiate(
                monster_octopusdu,
                posArr[i],
                Quaternion.identity
            );
        }
        
        _leftMonster = posArr.Length;
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
            Instantiate(npc[notGainNPC[rand]], new Vector3(0, 0.88f, 10), Quaternion.Euler(20, 0, 0));

            GameMng.I.userData.npc.Add(notGainNPC[rand]);
        }
    }
    protected override void dungeonRest() {
        campFire.SetActive(true);

        // TODO : 배틀 아이템 초기화

    }
    protected override void dungeonShop() {
        // Instantiate(npc_shop, new Vector3(0, 0.88f, 10), Quaternion.Euler(20, 0, 0));
        npc_shop.SetActive(true);
    }
    protected override void dungeonRandom() {}

    // public void someoneDamage(string name, int dmg)
    // {
    //     GameMng.I._monsters[name].getDamage(dmg);
    // }
}