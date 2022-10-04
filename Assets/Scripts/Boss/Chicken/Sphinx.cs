using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphinx : DestroySelf
{
    [SerializeField] bool _isAnswer = false;

    [SerializeField] Collider[] colliders;
    private void Start()
    {
        StartCoroutine(Check());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(10.0f);
        Debug.Log("IN");
        colliders = Physics.OverlapSphere(this.transform.position, 2.5f);

        if (!this._isAnswer)
        {
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    Character temp = col.transform.GetChild(0).GetComponent<Character>();
                    if (temp.nickName == GameMng.I.character.nickName)
                    {
                        GameMng.I.stateMng.user_HP_Numerical.Hp -= GameMng.I.stateMng.user_HP_Numerical.fullHp;
                    }
                }
            }
        }
        destroySelf();
        yield return null;
    }
}
