using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : DestroySelf
{
    enum TOTEM_TYPE
    {
        RECT = 0,
        TRIANGLE
    }

    [SerializeField] Collider2D[] colliders;
    [SerializeField] TOTEM_TYPE type;

    void Start()
    {
        StartCoroutine(TimeEnd());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }

    IEnumerator TimeEnd()
    {
        yield return new WaitForSeconds(10.0f);
        Debug.Log("IN");
        colliders = Physics2D.OverlapCircleAll(this.transform.position, 2.5f);

        switch (type)
        {
            case TOTEM_TYPE.RECT:
                if (CheckCount("Rect") != 3)
                {
                    GameMng.I.stateMng.user_HP_Numerical.Hp -= GameMng.I.stateMng.user_HP_Numerical.fullHp;
                    Debug.Log("사각형 실패 \"" + CheckCount("Rect") + "\"");
                }
                break;
            case TOTEM_TYPE.TRIANGLE:
                if (CheckCount("Triangle") != 1)
                {
                    GameMng.I.stateMng.user_HP_Numerical.Hp -= GameMng.I.stateMng.user_HP_Numerical.fullHp;
                    Debug.Log("삼각형 실패 \"" + CheckCount("Triangle") + "\"");
                }
                break;
        }
        Destroy(GameMng.I.character.transform.GetChild(0).gameObject);
        foreach(var value in NetworkMng.I.v_users.Values)
        {
            Destroy(value.transform.GetChild(0).gameObject);
        }
        destroySelf();
        yield return null;
    }

    int CheckCount(string tagname)
    {
        int count = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag(tagname))
            {
                count++;
            }
        }
        return count;
    }
}
