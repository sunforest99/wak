using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_0 : Monster
{
    [SerializeField] GameObject skill_0_prefab;         // 캐릭터 방향으로 알폭탄 던지는 스킬
    
    protected override void Awake()
    {
        base.Awake();
        _hp = 300000;
        _fullHp = 300000;
        _nearness = 3.5f;
        _moveSpeed = 0.1f;

        ATTACK_DAMAGE = 1000;
        SKILL_0_DAMAGE = 2000;
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

            // NetworkMng.I.SendMsg(string.Format("MONSTER_PATTERN:{0}:{1}", name, pattern));
        }
        else
        {
            doSomething(pattern);

            // NetworkMng.I.SendMsg(string.Format("MONSTER_PATTERN:{0}:{1}", name, pattern));
        }
        
    }

    /**
     * @brief 캐릭터 방향으로 알 폭탄 발사
     */
    public void fireEggBomb()
    {
        Instantiate(skill_0_prefab, transform.position, Quaternion.identity).GetComponent<EggBomb>().startThrow();
    }

    protected override int decideAct()
    {
        // 기본공격 없음
        
        return 2;           // 패턴 1
    }
}
