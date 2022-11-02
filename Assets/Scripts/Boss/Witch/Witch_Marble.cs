using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_Marble : MonoBehaviour
{
    [Range(0, 7)]
    [SerializeField] int type;
    [SerializeField] Witch _witch;
    [SerializeField] Rigidbody _rigid;
    [SerializeField] Vector3 dir;
    

    private void FixedUpdate() {
        // 끝까지 못먹음
        if (-0.1f < transform.position.x && transform.position.x < 0.1f && 
            -0.1f < transform.position.z && transform.position.z < 0.1f)
        {
            this.gameObject.SetActive(false);
        } else {
            _rigid.velocity = dir * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _witch.checkMarbleDic.Add(type, other.name);
            this.gameObject.SetActive(false);
        } else if (other.CompareTag("Character")) {
            _witch.checkMarbleDic.Add(type, other.name);
            this.gameObject.SetActive(false);
        }
    }
}
