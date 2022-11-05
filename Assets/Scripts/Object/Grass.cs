using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Weapon_disposable_me"))
        {
            SoundMng.I.PlayEffectGrass();
            Instantiate(GameMng.I.grass_destroy_eff, transform.position + new Vector3(0, 0.8f, 0), Quaternion.Euler(270, 0, 0));
            Destroy(this.gameObject);
        }
    }
}
