using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    [SerializeField] private Boss boss = null;

    // 타격 렌더 ================================================================
    [SerializeField] private Transform boss_o;      // 보스 이미지들 모인 Gameobject, sprite 부모
    [SerializeField] private Material[] materials = new Material[2];
    [SerializeField] private List<SpriteRenderer> render = new List<SpriteRenderer>();


    // 데미지 ===================================================================
    [SerializeField] int damageTemp;
    [SerializeField] Transform damagePopup;
    [SerializeField] GameObject _eff;
    [SerializeField] GameObject _backEff;
    bool isBackAttack;



    SpriteRenderer temp;
    void Start()
    {
        for (int i = 0; i < this.boss_o.childCount; i++)
        {
            temp = boss_o.GetChild(i).GetComponent<SpriteRenderer>();
            if (temp)
            {
                render.Add(temp);
            }
        }
    }

    /**
     * @bref 맞을때 호출해주기
     */
    IEnumerator HitBlink()
    {
        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[1];
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < render.Count; i++)
        {
            render[i].material = materials[0];
        }
    }

    void createDamage(Vector3 pos, int damage, bool isCritical)
    {
        Transform damageObj = Instantiate(damagePopup, pos, Quaternion.identity);
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set(damage, isCritical);
    }

    void createDamage(Vector3 pos)
    {
        Transform damageObj = Instantiate(damagePopup, pos, Quaternion.identity);
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set("immune");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me"))
        {
            // 버프 존재하는 스킬에 맞은건지 확인 ======================================================================================================================
            if (Character.usingSkill && Character.usingSkill.getBuffData && Character.usingSkill.getBuffData.isBossDebuf)
            {
                BuffActive(Character.usingSkill.getBuffData);
            }

            // 크리티컬 계산 ==========================================================================================================================================
            bool isCritical;
            isCritical = CheckCritical();

            // 타격 효과 ===============================================================================================================================================
            StartCoroutine(HitBlink());
            MCamera.I.shake(5f, .1f);

            // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남 =======================================================================================================
            if (Character.usingSkill && Character.usingSkill.isBackAttackSkill)
            {
                if (this.transform.localRotation.y == 180 && this.transform.position.x + 1 > other.transform.parent.transform.position.x)
                {
                    isBackAttack = true;
                    createEffect(transform.position.x + Random.Range(-2f, 1f), other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f));
                }
                // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남 ====================================================================================================
                else if (this.transform.localRotation.y == 0 && this.transform.position.x + 1 < other.transform.parent.transform.position.x)
                {
                    isBackAttack = true;
                    createEffect(transform.position.x + Random.Range(1.2f, 2.2f), other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f));
                }
                // (백어택 가능 공격인데) 일반 공격 =======================================================================================================================
                else
                {
                    isBackAttack = false;
                    createEffect(transform.position.x + Random.Range(-0.1f, 0.1f), other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f));
                }
            }
            // 일반 공격 ================================================================================================================================================
            else
            {
                isBackAttack = false;
                createEffect(transform.position.x + Random.Range(-0.1f, 0.1f), other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f));
            }

            // 평타면서 전사라면 공격 방향으로 밀려나는 효과 주기 ==========================================================================================================
            if (!Character.usingSkill && GameMng.I.userData.job.Equals((int)JOB.WARRIER)) {
                NetworkMng.I.SendMsg(string.Format("FORCE:{0}:{1}", this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, -1.2f));
                GameMng.I.character.addForceImpulse(new Vector3(this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, 0, -1.2f));
            }

            // 무적 상태인지 확인 =========================================================================================================================================
            if (!boss.isAnnihilation)
            {
                damageTemp = GameMng.I.getCharacterDamage(isCritical, isBackAttack);

                createDamage(
                    other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0),
                    damageTemp,
                    isCritical
                );
                NetworkMng.I.SendMsg(string.Format("DAMAGE:{0}", damageTemp));
                // boss._nestingHp -= damageTemp;
            }
            else
            {
                createDamage(other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0));
            }
        }
        else if (other.gameObject.CompareTag("Esther_Attack_Skill"))
        {
            if (!boss.isAnnihilation)
            {
                createDamage(
                    other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0),
                    Random.Range(50123300, 54987889),       // <- 에스더는 고정 데미지인데 랜덤 값이 너무 딱 맞는 값 나오면 이상하니까 겹쳐도 적당한 수
                    true
                );
                NetworkMng.I.SendMsg(string.Format("DAMAGE:{0}", damageTemp));
                // boss._nestingHp -= damageTemp;
            }
        }

        // 일회용 공격이면 제거 =========================================================================================================================================
        if (other.gameObject.CompareTag("Weapon_disposable") || other.gameObject.CompareTag("Weapon_disposable_me"))
        {
            Destroy(other.gameObject);
        }
    }

    void createEffect(float posX, float posY)
    {
        Instantiate(
            isBackAttack ? _backEff : _eff,
            new Vector3(posX, posY, transform.position.z),
            Quaternion.identity
        );
    }

    Vector3 getHitEffPos(Vector3 closetPoint)
    {
        closetPoint += new Vector3(0, 3, 0);
        closetPoint.z = transform.position.z;
        return closetPoint;
    }

    bool CheckCritical()
    {
        float criticalrand = Random.Range(0.0f, 100.0f);
        float criticalPer = Character._stat.criticalPer;
        for (int i = 0; i < boss.bossDeBuffs.Length; i++)
        {
            if (boss.bossDeBuffs[i].gameObject.activeInHierarchy)
            {
                if (boss.bossDeBuffs[i].buffData.BuffKind == BUFF.BUFF_GAL)
                {
                    criticalPer += boss.bossDeBuffs[i].buffData.numerical;
                    break;
                }
            }
        }

        if (criticalrand <= criticalPer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void BuffActive(BuffData hitbuffData)
    {
        for (int i = 0; i < boss.bossDeBuffs.Length; i++)
        {
            if (boss.bossDeBuffs[i].gameObject.activeInHierarchy && boss.bossDeBuffs[i].buffData.name == hitbuffData.name)
            {
                boss.bossDeBuffs[i].duration = Character.usingSkill.getBuffData.duration;
                break;
            }
            else if (!boss.bossDeBuffs[i].gameObject.activeInHierarchy)
            {
                boss.bossDeBuffs[i].buffData = Character.usingSkill.getBuffData;
                boss.bossDeBuffs[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
