using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphinx : MonoBehaviour
{
    [SerializeField] Chicken chicken = null;
    [SerializeField] private Vector3 pos;
    public bool _isAnswer = false;

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
                        GameMng.I.stateMng.forcedDeath();
                    }
                }
            }
        }
        this.gameObject.SetActive(false);
        yield return null;
    }

    void OnEnable()
    {
        this.transform.position = pos;
    }

    void OnDisable()
    {
        chicken.question.gameObject.SetActive(false);
        chicken.getAction = CHICKEN_ACTION.IDLE;
        _isAnswer = false;
    }
}
