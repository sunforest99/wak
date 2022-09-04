using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMng : MonoBehaviour
{
    [SerializeField] GameObject monster_seaduNew;       // 뉴심해두 몬스터 prefab


    void Start()
    {
        dungeonType_0();
    }

    void dungeonType_0()
    {
        Vector3[] posArr = {
            new Vector3(-4.16f, 1.325f, 0),
            new Vector3(3.98f, 1.325f, -2.02f),
            new Vector3(-2.04f, 1.325f, -5.62f)
        };
        for (int i = 0; i < posArr.Length; i++)
        {
            // 뉴심해두 몬스터 생성
            GameMng.I._monsters.Add(
                "m" + i,
                Instantiate(
                            monster_seaduNew,
                            new Vector3(-4.16f, 1.325f, 0),
                            Quaternion.identity
                        ).GetComponent<Monster>()
            );
            GameMng.I._monsters["m" + i].name = "m" + i;
        }
    }

    public void someoneDamage(string name, int dmg)
    {
        GameMng.I._monsters[name].getDamage(dmg);
    }
}
