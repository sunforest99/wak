using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanService : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [HideInInspector] public Transform parent;
    Vector3 pos;

    private void FixedUpdate()
    {
        pos = parent.transform.position;
        pos.y = -0.199f;
        _rigidbody.MovePosition(pos);
    }

    public void setActiveOff()
    {
        this.gameObject.SetActive(false);
    }
}
