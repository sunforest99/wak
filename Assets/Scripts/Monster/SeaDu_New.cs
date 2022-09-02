using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_New : Monster
{
    [SerializeField] GameObject skill_0_prefab;
    
    protected override void Start()
    {
        base.Start();
        _hp = 30000000;
        _fullHp = 30000000;
    }

    /**
     * @brief 대각선으로 뿔 공격 소환
     */
    protected override IEnumerator skill_0()
    {
        yield return new WaitForSecondsRealtime(1);
        int i = 0;
        while (i++ < 5)
        {
            Instantiate(skill_0_prefab, new Vector3(-1.5f * i, -0.158f, 1.15f * i), Quaternion.identity);
            Instantiate(skill_0_prefab, new Vector3(-1.5f * i, -0.158f, -1.15f * i), Quaternion.identity);
            Instantiate(skill_0_prefab, new Vector3(1.5f * i, -0.158f, 1.15f * i), Quaternion.identity);
            Instantiate(skill_0_prefab, new Vector3(1.5f * i, -0.158f, -1.15f * i), Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

    protected override IEnumerator think()
    {
        yield return new WaitForSecondsRealtime(2);

        doSomething(Random.Range(1, 3));
        //NetworkMng.I.SendMsg();     // TODO : 행동 메세지 보내기
    }

}
