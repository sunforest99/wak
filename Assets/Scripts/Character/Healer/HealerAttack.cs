using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Map_Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
