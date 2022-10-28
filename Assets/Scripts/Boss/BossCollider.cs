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

    private void OnTriggerEnter(Collider other)
    {
        // Weapon : 캐릭터에게 붙어있는 공격 콜리더
        // Weapon_disposable : 다른 사람의 일회용(맞으면 사라져야하는) 공격&스킬
        // Weapon_disposable_me : 나의 일회용 공격(일반 공격, 스킬 X)
        // Skill : 나의 스킬 공격 (분리된 것들)
        // Skill_disposable_me : 나의 일회용 스킬 공격 (분리된 것들)
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me") || other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me"))
        {
            // 보스가 맞은게 공격인지 소환식 혹은 일반 스킬들인지 확인
            SkillData skillData = other.gameObject.CompareTag("Weapon") ?
                    Character.usingSkill : (other.gameObject.CompareTag("Skill") || other.gameObject.CompareTag("Skill_disposable_me")) ?
                    GameMng.I.character.skilldatas[int.Parse(other.gameObject.name)] :
                    null /* "Weapon_disposable_me" */;

            // 버프 존재하는 스킬에 맞은건지 확인 ======================================================================================================================
            if (skillData && skillData.getBuffData && skillData.getBuffData.isBossDebuf)
            {
                boss.BuffActive(skillData.getBuffData);
                NetworkMng.I.SendMsg($"BUFF:2:{skillData.getBuffData.BuffKind.ToString()}");
            }

            // 크리티컬 계산 ==========================================================================================================================================
            bool isCritical = CheckCritical();

            // 타격 효과 ===============================================================================================================================================
            StartCoroutine(HitBlink());
            if (skillData)
                MCamera.I.shake(skillData.getIntensity, skillData.getShakeTime);
            else if (GameMng.I.userData.job.Equals((int)JOB.WARRIER))
                MCamera.I.shake(5, .1f);
            else    // 법사 or 힐러 평타
                MCamera.I.shake(4, .1f);

            if (skillData && skillData.isBackAttackSkill)
            {
                // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남 ===================================================================================================
                if (boss_o.parent.transform.localRotation.y.Equals(180) && this.transform.position.x + 1 > GameMng.I.character.transform.parent.position.x)
                {
                    isBackAttack = true;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(-2f, 1f),
                        other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f),
                        transform.position.z
                    ));
                }
                // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남 ====================================================================================================
                else if (boss_o.parent.transform.localRotation.y.Equals(0) && this.transform.position.x + 1 < GameMng.I.character.transform.parent.position.x)
                {
                    isBackAttack = true;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(1.2f, 2.2f),
                        other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f),
                        transform.position.z
                    ));
                }
                // (백어택 가능 공격인데) 일반 공격 =======================================================================================================================
                else
                {
                    isBackAttack = false;
                    GameMng.I.createEffect(isBackAttack, new Vector3(
                        transform.position.x + Random.Range(-0.1f, 0.1f),
                        other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f),
                        transform.position.z
                    ));
                }
            }
            // 일반 공격 ================================================================================================================================================
            else
            {
                isBackAttack = false;
                GameMng.I.createEffect(isBackAttack, new Vector3(
                    transform.position.x + Random.Range(-0.1f, 0.1f),
                    other.ClosestPoint(transform.position).y + Random.Range(1.2f, 1.4f),
                    transform.position.z
                ));
            }

            // 평타면서 전사라면 공격 방향으로 밀려나는 효과 주기 ==========================================================================================================
            if (!skillData && GameMng.I.userData.job.Equals((int)JOB.WARRIER)) {
                NetworkMng.I.SendMsg(string.Format("FORCE:{0}:{1}", this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, -1.2f));
                GameMng.I.character.addForceImpulse(new Vector3(this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, 0, -1.2f));
            }

            // 무적 상태인지 확인 =========================================================================================================================================
            if (!boss.isAnnihilation)
            {
                damageTemp = GameMng.I.getCharacterDamage(
                    skillData,
                    isCritical,
                    isBackAttack
                );
                
                int chimsikCount = GameMng.I.stateMng.checkDebuff(BUFF.DEBUFF_CHIMSIK);

                // 만약 '디버프 침식'을 소유중이라면 데미지 감소
                if (chimsikCount > 0) {
                    damageTemp = Mathf.FloorToInt(damageTemp * (1 - 0.08f * chimsikCount));
                }

                // 에스더 버프 중이라면 데미지 증가 120% 증가 버프
                if (GameMng.I.estherManager._esther_buff_state.Equals(ESTHER_BUFF.COTTON_BUFF))
                {
                    damageTemp = Mathf.CeilToInt(damageTemp * 1.2f);
                }

                GameMng.I.createDamage(
                    other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0),
                    damageTemp,
                    isCritical
                );
                NetworkMng.I.SendMsg(string.Format("DAMAGE:{0}", damageTemp));
                // boss._nestingHp -= damageTemp;
            }
            else
            {
                GameMng.I.createDamage(other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0));
            }
        }
        else if (other.gameObject.CompareTag("Esther_Attack_Skill"))
        {
            damageTemp = !boss.isAnnihilation ? Random.Range(50123300, 54987889) : 0;
            Vector3 dmgSpawnPos = other.ClosestPoint(transform.position) + new Vector3(0, 3f, 0);

            if (!damageTemp.Equals(0)) {
                GameMng.I.createDamage(
                    dmgSpawnPos,
                    damageTemp,       // <- 에스더는 고정 데미지인데 랜덤 값이 너무 딱 맞는 값 나오면 이상하니까 겹쳐도 적당한 수
                    true
                );
                boss._nestingHp -= damageTemp;
            }
            else {
                GameMng.I.createDamage(dmgSpawnPos);
            }
            NetworkMng.I.SendMsg(string.Format("ESTHER_DAMAGE:{0}:{1}:{2}:{3}", dmgSpawnPos.x, dmgSpawnPos.y, dmgSpawnPos.z, damageTemp));
        }

        // 일회용 공격이면 제거 =========================================================================================================================================
        if (other.gameObject.CompareTag("Weapon_disposable") || other.gameObject.CompareTag("Weapon_disposable_me") || other.gameObject.CompareTag("Skill_disposable_me"))
        {
            Destroy(other.gameObject);
        }
    }

    // void createEffect(float posX, float posY)
    // {
    //     Instantiate(
    //         isBackAttack ? GameMng.I.backEff : GameMng.I.eff,
    //         new Vector3(posX, posY, transform.position.z),
    //         Quaternion.identity
    //     );
    // }

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
}
