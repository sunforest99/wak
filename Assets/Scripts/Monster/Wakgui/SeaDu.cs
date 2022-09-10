using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDu : Monster
{
    [SerializeField] GameObject skill_0_prefab;         // 캐릭터에게 총알 발사하는 스킬
    GameObject skill_0_pool_parent;
    List<SeaDu_Bullet> bulletPool = new List<SeaDu_Bullet>();

    protected override void Awake()
    {
        base.Awake();
        _hp = 30000000;
        _fullHp = 30000000;
        _nearness = 2;
        _moveSpeed = 2.5f;

        ATTACK_DAMAGE = 1000;
        SKILL_0_DAMAGE = 2000;

        skill_0_pool_parent = Instantiate(new GameObject("pool"));
        initBulletPool();
    }

    protected override void attack(string msg)
    {
        _damage = ATTACK_DAMAGE;
    }
    protected override void skill_0(string msg)
    {
        _damage = SKILL_0_DAMAGE;
        StartCoroutine(diagonalAttack());
    }
    
    void initBulletPool()
    {
        int i = 0;
        while (i++ < 3)
        {
            bulletPool.Add(
                Instantiate(skill_0_prefab, Vector3.zero, Quaternion.identity, skill_0_pool_parent.transform).GetComponent<SeaDu_Bullet>()
            );
        }
    }

    /**
     * @brief 캐릭터 방향으로 총알 발사
     */
    IEnumerator diagonalAttack()
    {
        yield return new WaitForSecondsRealtime(1);
        
        skill_0_pool_parent.transform.position = transform.position;

        Vector3 moveTo = GameMng.I.character.transform.parent.position;
        moveTo -= transform.position;
        moveTo.y = GameMng.I.character.transform.parent.position.y;

        float lookAngle =  Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;

        // Vector3 spawnPos = transform.position;
        // spawnPos.y = 0.3f;
        
        bool successShoot = false;
        for (int i = 0; i < 3; i++)
        {
            successShoot = false;
            int j = 0;

            Vector3 spawnPos = transform.position;
            spawnPos.y = moveTo.y;

            for (; j < bulletPool.Count; j++)
            {
                if (!bulletPool[j].gameObject.activeSelf)
                {
                    successShoot = true;
                    bulletPool[j].transform.position = spawnPos;
                    bulletPool[j].transform.rotation = Quaternion.Euler(90, 0, lookAngle);
                    bulletPool[j].setVelocity();
                    bulletPool[j].gameObject.SetActive(true);
                    break;
                }
            }
            if (!successShoot)
            {
                initBulletPool();
                for (; j < bulletPool.Count; j++)
                {
                    if (!bulletPool[j].gameObject.activeSelf)
                    {
                        successShoot = true;
                        bulletPool[j].transform.position = spawnPos;
                        bulletPool[j].transform.rotation = Quaternion.Euler(90, 0, lookAngle);
                        bulletPool[j].setVelocity();
                        bulletPool[j].gameObject.SetActive(true);
                        break;
                    }
                }
            }
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

    protected override void OnDestroy() {
        Destroy(skill_0_pool_parent.gameObject);
    }
}
