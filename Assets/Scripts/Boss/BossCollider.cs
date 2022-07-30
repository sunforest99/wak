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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            StartCoroutine(HitBlink());
            _camera.shake();
            
            // 보스 우측 바라보는 상태에서  콜리더가 좌측에서 일어남
            if (this.transform.localRotation.y == 180  &&  this.transform.position.x + 1 > other.transform.parent.transform.position.x)
                Instantiate(_backEff, new Vector2(transform.position.x + Random.Range(-2f, 1f),  other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f)  ) , Quaternion.identity);
            // 보스 좌측 바라보는 상태에서  콜리더가 우측에서 일어남
            else if (this.transform.localRotation.y == 0  &&  this.transform.position.x + 1 < other.transform.parent.transform.position.x)
                Instantiate(_backEff, new Vector2(transform.position.x + Random.Range(1.2f, 2.2f),  other.ClosestPoint(transform.position).y + Random.Range(1f, 1.2f)  ) , Quaternion.identity);
            // 일반 공격
            else
                Instantiate(_eff, other.ClosestPoint(transform.position) + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(1f, 1.2f)), Quaternion.identity);

            int dmg = 0;
            bool isCritical = false;
            // if (Random.Range(0, 5) == 0) {
            //     isCritical = true;
            //     dmg = Random.Range(3000000, 6000000);
            // }
            // else {
            //     dmg = Random.Range(2000000, 4000000);
            // }

            createDamage(
                other.ClosestPoint(transform.position) + new Vector2(0, 3f),
                GameMng.I.gatDamage(),
                isCritical
           );
           boss._nestingHp -= GameMng.I.gatDamage();
        }
    }

    void SetJumpPostion()
    {
        this.transform.parent.localPosition = GameMng.I.targetList[GameMng.I.targetCount].localPosition;
    }
}
