using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{

    public StateMng StateMngSc;
    public BUFF BuffKind;                       // 버프 종류                NOT_BUFF일시 디버프
    public DEBUFF DeBuffKind;                   // 디버프 종류              NOT_DEBUFF일시 버프

    [SerializeField] TMPro.TextMeshProUGUI Mount;

    public float duration;                       // 지속시간
    public float numerical;                     // 수치

    public int count;                           // 중첩 갯수

    public bool check_nesting;                  // 중첩 여부

    void OnEnable()
    {
        StartCoroutine(cool());
    }

    IEnumerator cool(){
        yield return new WaitForSeconds(duration);

        if(count > 1)
            StartCoroutine(cool());
        else if(count <= 1)
        {
            gameObject.SetActive(false);
            if(DeBuffKind == DEBUFF.NOT_DEBUFF)
                StateMngSc.PlayerBuffGams[(int)BuffKind].SetActive(false);
            if(BuffKind == BUFF.NOT_BUFF)
                StateMngSc.PlayerDeBuffGams[(int)DeBuffKind].SetActive(false);
        }
        count--;
        
        if(BuffKind == BUFF.NOT_BUFF && check_nesting)
            Mount.text = 'x' + count.ToString();
    }

}
