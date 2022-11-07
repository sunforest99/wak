using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_2 : Monster
{
    [SerializeField] GameObject skill_0_prefab;         // 캐릭터에게 침(총알) 발사하는 스킬
    GameObject skill_0_pool_parent;
    List<Bird2_Bullet> bulletPool = new List<Bird2_Bullet>();

    protected override void Awake()
    {
        base.Awake();
        _hp = 28349540;
        _fullHp = 28349540;
        _nearness = 3.5f;
        _moveSpeed = 0.1f;

        ATTACK_DAMAGE = 1000;
        SKILL_0_DAMAGE = 2000;

        skill_0_pool_parent = new GameObject("pool");
        initBulletPool();
    }

    protected override void attack(string msg)
    {
        _damage = ATTACK_DAMAGE;
        int xRand = Random.Range(0, 2) == 0 ? -1 : 1;
        int zRand = Random.Range(0, 2) == 0 ? -1 : 1;
        _rigidbody.AddForce(new Vector3(3 * xRand, 0, 3 * zRand), ForceMode.Impulse);
    }
    protected override void skill_0(string msg)
    {
        _damage = SKILL_0_DAMAGE;
        StartCoroutine(fireAttack());
    }
    
    void initBulletPool()
    {
        int i = 0;
        while (i++ < 3)
        {
            bulletPool.Add(
                Instantiate(skill_0_prefab, Vector3.zero, Quaternion.identity, skill_0_pool_parent.transform).GetComponent<Bird2_Bullet>()
            );
        }
    }

    /**
     * @brief 캐릭터 방향으로 총알 발사
     */
    IEnumerator fireAttack()
    {
        yield return new WaitForSeconds(1);
        
        bool successShoot = false;
        for (int i = 0; i < 3; i++)
        {
            successShoot = false;
            int j = 0;

            skill_0_pool_parent.transform.position = transform.position;

            Vector3 moveTo = GameMng.I.character.transform.parent.position;
            moveTo -= transform.position;
            moveTo.y = 1.11f;

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
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    protected override IEnumerator think()
    {
        yield return new WaitForSecondsRealtime(Random.Range(2f, 2.5f));

        int pattern = decideAct();
        doSomething(pattern);        
    }
    
    protected override int decideAct()
    {
        float distance = getDistanceFromTarget(_target.position);
        int rand = Random.Range(0, 100);

        /*
            새두2 패턴

            10 : 기본 공격 (위치 이동)
            00 : 패턴1 (침뱉기)
        */
        if (rand < 90)
            return 2;       // 패턴 1
        return 1;           // 기본 공격
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Destroy(skill_0_pool_parent.gameObject);
    }
}
