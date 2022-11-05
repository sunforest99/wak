using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenBomb : MonoBehaviour
{
    [SerializeField] GameObject egg_bomb_eff;
    
    void Start()
    {
        StartCoroutine(timerCounting());
    }

    IEnumerator timerCounting() {
        yield return new WaitForSeconds(3);

        Instantiate(egg_bomb_eff, new Vector3(transform.position.x, -0.197f, transform.position.z), Quaternion.Euler(90, 0, 0));
        Destroy(this.gameObject);
    }
    
}
