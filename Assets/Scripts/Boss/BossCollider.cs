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
            Instantiate(_eff, other.ClosestPoint(transform.position) + new Vector2(0, 1f), Quaternion.identity);

            int dmg = 0;
            bool isCritical = false;
            if (Random.Range(0, 5) == 0) {
                isCritical = true;
                dmg = Random.Range(3000000, 6000000);
            }
            else {
                dmg = Random.Range(2000000, 4000000);
            }

            createDamage(
                other.ClosestPoint(transform.position) + new Vector2(0, 3f),
                dmg,
                isCritical
           );
           boss._nestingHp -= 3192856;
        }
    }
}
