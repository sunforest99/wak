using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBuff : MonoBehaviour
{
    public BuffData buffData;
    public Boss BossSc;
    public int kind;
    public int apply_count;                     // 버프가 유지된 카운트
    public int count;                           // 중첩 갯수
    public bool isApply;                        // 현재 적용 중인지

    void OnEnable()
    {
        buffData = BossSc.bufflist[kind];
        apply_count = buffData.duration;
        StartCoroutine(cool());
    }

    IEnumerator cool()
    {
        yield return new WaitForSeconds(1.0f);
        if (apply_count >= 1)
        {
            apply_count--;
            StartCoroutine(cool());
        }
        else
        {
            gameObject.SetActive(false);
            if (buffData.check_buff)
            {
                BossSc.bossBuff[(int)buffData.BuffKind].SetActive(false);
            }
            else
            {
                BossSc.bossBuff[(int)buffData.BuffKind].SetActive(false);
            }
            isApply = false;
            count = 0;
        }
    }
}
