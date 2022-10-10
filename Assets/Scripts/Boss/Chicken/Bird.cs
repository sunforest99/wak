using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;

    public int rand;
    int damage;

    void Start()
    {
        // damage = GameMng.I.boss.bossData.getDamage((int)WAKGUI_ACTION.PATTERN_WAVE);

        // _rigidbody.velocity = transform.TransformDirection(Vector3.left * 3f);
    }

    private void OnEnable()
    {
        switch (rand)
        {
            case (int)POS.UP:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, -90.0f);
                break;

            case (int)POS.DOWN:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 90.0f);
                break;


            case (int)POS.RIGHT:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 180.0f);
                break;

            case (int)POS.LEFT:
                this.transform.localRotation = Quaternion.Euler(90.0f, 0, 0);
                break;
        }

        _rigidbody.velocity = transform.TransformDirection(Vector3.left * 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if(other.CompareTag("Player"))
        // {
        //     GameMng.I.stateMng.ActiveOwnBuff(GameMng.I.boss.bossData.getBuffs[2]);
        //     GameMng.I.stateMng.takeDamage(damage);
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Map_Wall"))
        {
            GameMng.I.showEff(EFF_TYPE.REMOVE_EFF, this.transform.localPosition);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }
    }
}
