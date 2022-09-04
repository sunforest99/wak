using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianFire : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Map_Wall"))
        {
            GameMng.I.showEff(EFF_TYPE.REMOVE_EFF, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
