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
        _nearness = 3.5f;
        _moveSpeed = 0.1f;

        ATTACK_DAMAGE = 1000;
        SKILL_0_DAMAGE = 2000;

        skill_0_pool_parent = Instantiate(new GameObject("pool"));
        initBulletPool();
    }

    protected override void attack(string msg)
    {
        _damage = ATTACK_DAMAGE;
        int xRand = Random.Range(0, 2) == 0 ? -1 : 1;
        int zRand = Random.Range(0, 2) == 0 ? -1 : 1;
        _rigidbody.AddForce(new Vector3(8 * xRand, 0, 8 * zRand), ForceMode.Impulse);
    }
    protected override void skill_0(string msg)
    {
        _damage = SKILL_0_DAMAGE;
        StartCoroutine(fireAttack());
    }
    
    void initBulletPool()
    {
        int i = 0;
        while (i++ < 5)
        {
            bulletPool.Add(
                Instantiate(skill_0_prefab, Vector3.zero, Quaternion.identity, skill_0_pool_parent.transform).GetComponent<SeaDu_Bullet>()
            );
        }
    }

    /**
     * @brief 캐릭터 방향으로 총알 발사
     */
    IEnumerator fireAttack()
    {
        yield return new WaitForSeconds(0.4f);

        // Vector3 spawnPos = transform.position;
        // spawnPos.y = 0.3f;
        
        bool successShoot = false;
        for (int i = 0; i < 5; i++)
        {
            successShoot = false;
            int j = 0;

            skill_0_pool_parent.transform.position = transform.position;

            Vector3 moveTo = GameMng.I.character.transform.parent.position;
            moveTo -= transform.position;
            moveTo.y = GameMng.I.character.transform.parent.position.y;

            float lookAngle = Mathf.Atan2(moveTo.z, moveTo.x) * Mathf.Rad2Deg;

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
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    protected override IEnumerator think()
    {
        yield return new WaitForSecondsRealtime(Random.Range(2f, 2.5f));

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
            심해두 패턴

            대상과 거리가 가깝다면
                80 : 기본 공격
                20 : 패턴1
            대상과 거리가 멀다면
                20 : 기본 공격
                80 : 패턴1
        */
        if (distance <= _nearness + 1)
        {
            if (rand < 80)
                return 1;       // 기본 공격
            return 2;           // 패턴 1
        }
        if (rand < 20)
            return 1;           // 기본 공격
        return 2;               // 패턴 1
    }

    protected override void OnDestroy() {
        Destroy(skill_0_pool_parent.gameObject);
    }
}
