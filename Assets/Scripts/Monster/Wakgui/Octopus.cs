using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : Monster
{
    [SerializeField] GameObject attack_prefab;          // 캐릭터 방향으로 검은 잉크

    [SerializeField] GameObject skill_0_prefab;         // 대각선으로 지면에서 뿔 생성하는 스킬
    GameObject skill_0_pool_parent;
    List<GameObject> skill_0_pool = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        _hp = 33299540;
        _fullHp = 33299540;
        _nearness = 2;
        _moveSpeed = 0.1f;

        ATTACK_DAMAGE = 1000;
        SKILL_0_DAMAGE = 2000;

        skill_0_pool_parent = new GameObject("pool");
        int i = 0;
        while (i++ < 5)
        {
            skill_0_pool.Add(Instantiate(skill_0_prefab, Vector3.zero, Quaternion.Euler(90, 0, 0), skill_0_pool_parent.transform));
        }
    }

    protected override void attack(string msg)
    {
        _damage = ATTACK_DAMAGE;
    }
    protected override void skill_0(string msg)
    {
        _damage = SKILL_0_DAMAGE;
        StartCoroutine(legAttack());
    }
    
       

    /**
     * @brief 캐릭터 방향으로 잉크 발사
     */
    public void fireBlackInk()
    {
        Instantiate(attack_prefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity).GetComponent<BlackInk>().startThrow();
    }


    /**
     * @brief 캐릭터 방향으로 지면 생성 공격
     */
    IEnumerator legAttack()
    {
        yield return new WaitForSecondsRealtime(1);

        int i = 0;
        while (i < 5)
        {
            skill_0_pool[i].transform.position = new Vector3(
                GameMng.I.character.transform.parent.position.x,
                -0.18f,
                GameMng.I.character.transform.parent.position.z
            );
            skill_0_pool[i].SetActive(true);
            i++;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    protected override IEnumerator think()
    {
        yield return new WaitForSecondsRealtime(Random.Range(2f, 3f));

        int pattern = decideAct();
        doSomething(pattern);
    }
    
    int beforeAct = 1;
    protected override int decideAct()
    {
        float distance = getDistanceFromTarget(_target.position);

        if (beforeAct.Equals(1))
        {
            beforeAct = 2;
            return 2;
        }
        beforeAct = 1;
        return 1;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Destroy(skill_0_pool_parent.gameObject);
    }
}
