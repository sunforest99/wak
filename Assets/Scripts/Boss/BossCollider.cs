using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> render = new List<SpriteRenderer>();

    [SerializeField] private Material[] materials = new Material[2];

    [SerializeField] private Boss boss = null;

    [SerializeField] MCamera _camera;

    [SerializeField] Transform damagePopup;

    [SerializeField] GameObject _eff;
    [SerializeField] GameObject _backEff;

    [SerializeField] int damageTemp;

    bool isBackAttack;

    SpriteRenderer temp;
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<SpriteRenderer>();
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

    void createDamage(Vector2 pos, int damage, bool isCritical)
    {
        Transform damageObj = Instantiate(damagePopup, pos, Quaternion.identity);
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set(damage, isCritical);
    }

    void createDamage(Vector2 pos)
    {
        Transform damageObj = Instantiate(damagePopup, pos, Quaternion.identity);
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set("immune");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            // if (Mathf.Abs(transform.parent.position.z - other.transform.parent.position.z) > 2)
            //     return;

            bool isCritical;
            if (Character.usingSkill && Character.usingSkill.getBuffData && Character.usingSkill.getBuffData.isBossDebuf)
            {
                BuffActive(Character.usingSkill.getBuffData);
            }
            isCritical = CheckCritical();

            StartCoroutine(HitBlink());
            // _camera.shake();
            MCamera.I.shake(5f, .1f);

            // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남
            if (this.transform.localRotation.y == 180 && this.transform.position.x + 1 > other.transform.parent.transform.position.x)
            {
                Instantiate(_backEff, new Vector2(transform.position.x + Random.Range(-2f, 1f), other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f)), Quaternion.identity);
                isBackAttack = true;
            }
            // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남
            else if (this.transform.localRotation.y == 0 && this.transform.position.x + 1 < other.transform.parent.transform.position.x)
            {
                Instantiate(_backEff, new Vector2(transform.position.x + Random.Range(1.2f, 2.2f), other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f)), Quaternion.identity);
                isBackAttack = true;
            }
            // 일반 공격
            else
            {
                Instantiate(_eff, other.ClosestPoint(transform.position) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(1f, 1.2f), 0), Quaternion.identity);
                isBackAttack = false;
            }

            // 평타면서 전사라면 공격 방향으로 밀려나는 효과 주기
            if (!Character.usingSkill && GameMng.I.userData.job.Equals((int)JOB.WARRIER)) {
                GameMng.I.character.addForceImpulse(new Vector3(this.transform.position.x < other.transform.parent.transform.position.x ? -2 : 2, 0, -1.2f));

                // TODO : 네트워크 메세지
                // NetworkMng.I.SendMsg("FORCE:{0}:{1}",  );
            }

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
        else if (other.gameObject.CompareTag("Esther_Attack"))
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
