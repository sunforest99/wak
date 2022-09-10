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
            new Vector3(-4.16f, 1.325f, 10),
            new Vector3(3.98f, 1.325f, 8.02f),
            new Vector3(-2.04f, 1.325f, 5.62f)
        };

        // TODO : 몬스터 다르게 하기 등록
        for (int i = 0; i < posArr.Length; i++)
        {
            // 뉴심해두 몬스터 생성
            Instantiate(
                monster_seaduNew,
                posArr[i],
                Quaternion.identity
            );
        }
    }
    protected override void dungeonMonsterPurple() {
        Instantiate(purpleLight, new Vector3(0, 0, 0), Quaternion.identity);

        // TODO : 몬스터 만들면서 강화 및 회복 능력 부여
        Vector3[] posArr = {
            new Vector3(-4.16f, 1.325f, 0),
            new Vector3(3.98f, 1.325f, -2.02f),
            new Vector3(-2.04f, 1.325f, -5.62f)
        };

        // TODO : 몬스터 다르게 하기 등록
        for (int i = 0; i < posArr.Length; i++)
        {
            // 뉴심해두 몬스터 생성
            Instantiate(
                monster_seaduNew,
                posArr[i],
                Quaternion.identity
            );
        }
    }
    protected override void dungeonNPC() {
        
    }
    protected override void dungeonReset() {}
    protected override void dungeonShop() {}
    protected override void dungeonRandom() {}

    // public void someoneDamage(string name, int dmg)
    // {
    //     GameMng.I._monsters[name].getDamage(dmg);
    // }
}