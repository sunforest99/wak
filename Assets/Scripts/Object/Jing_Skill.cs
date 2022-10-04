using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jing_Skill : DestroySelf
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
