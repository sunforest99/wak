using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu_New : Monster
{
    [SerializeField] GameObject skill_0_prefab;
    GameObject skill_0_pool_parent;
    List<GameObject>    skill_0_pool_lu = new List<GameObject>(), skill_0_pool_ld = new List<GameObject>(),
                        skill_0_pool_ru = new List<GameObject>(), skill_0_pool_rd = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        _hp = 30000000;
        _fullHp = 30000000;
        _nearness = 2;
        _moveSpeed = 2.5f;

        skill_0_pool_parent = Instantiate(new GameObject("pool"));
        int i = 0;
        while (i++ < 5)
        {
            skill_0_pool_lu.Add(Instantiate(skill_0_prefab, new Vector3(-1.5f * i, -1.52f, 1.15f * i), Quaternion.Euler(20, 0, 0), skill_0_pool_parent.transform));
            skill_0_pool_ld.Add(Instantiate(skill_0_prefab, new Vector3(-1.5f * i, -1.52f, -1.15f * i), Quaternion.Euler(20, 0, 0), skill_0_pool_parent.transform));
            skill_0_pool_ru.Add(Instantiate(skill_0_prefab, new Vector3(1.5f * i, -1.52f, 1.15f * i), Quaternion.Euler(20, 0, 0), skill_0_pool_parent.transform));
            skill_0_pool_rd.Add(Instantiate(skill_0_prefab, new Vector3(1.5f * i, -1.52f, -1.15f * i), Quaternion.Euler(20, 0, 0), skill_0_pool_parent.transform));
        }
    }

    protected override void attack(string msg)
    {
        _damage = 1000;
    }
    protected override void skill_0(string msg)
    {
        _damage = 2000;
        StartCoroutine(diagonalAttack());
    }
    
    /**
     * @brief 대각선으로 뿔 공격 소환
     */
    IEnumerator diagonalAttack()
    {
        yield return new WaitForSecondsRealtime(1);
        
        skill_0_pool_parent.transform.position = transform.position;

        int i = 0;
        while (i < 5)
        {
            // todo : 벽을 넘지는 않는지 검사해서 쏘기. mapsize 완성되면 하기
            skill_0_pool_lu[i].SetActive(true);
            skill_0_pool_ld[i].SetActive(true);
            skill_0_pool_ru[i].SetActive(true);
            skill_0_pool_rd[i].SetActive(true);
            i++;
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

    protected override IEnumerator think()
    {
        yield return new WaitForSecondsRealtime(4);

        int pattern = decideAct();
        
        // TODO : 
        if (pattern.Equals(-1))
        {
            // searchTarget();
            doSomething(pattern);

            NetworkMng.I.SendMsg(string.Format("MONSTER_PATTERN:{0}:{1}", name, pattern));
        }
        else
        {
            doSomething(pattern);

            NetworkMng.I.SendMsg(string.Format("MONSTER_PATTERN:{0}:{1}", name, pattern));
        }
        
    }
    
    protected override int decideAct()
    {
        float distance = getDistanceFromTarget(_target.position);
        int rand = Random.Range(0, 100);

        /*
            뉴 심해두 패턴

            10 : 대상 변경
            90 :
                대상과 거리가 가깝다면
                    80 : 기본 공격
                    30 : 패턴1
                대상과 거리가 멀다면
                    10 : 대상 변경
                    10 : 휴식
                    80 : 패턴1
        */

        if (rand < 10)
            return 0;           // 대상 변경

        if (distance <= _nearness + 1)
        {
            if (rand < 90)
                return 1;       // 기본 공격
            return 2;           // 패턴 1
        }
        if (rand < 20)
            return 0;           // 대상 변경
        else if (rand < 30)
            return -1;          // 휴식
        return 2;               // 패턴 1
    }

    void OnDestroy() {
        Destroy(skill_0_pool_parent);
    }
}
