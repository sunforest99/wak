using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    float MOVE_SPEED = 10;
    Vector2 _moveDir;
    
    void Start()
    {
        MCamera.I.setTargetChange(this.transform);
    }

    void Update()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (GameMng.I._keyMode.Equals(KEY_MODE.PLAYER_MODE)) {
            Vector3 moveDist = _moveDir.normalized;
            _rigidBody.velocity = new Vector3(moveDist.x, _rigidBody.velocity.y, moveDist.y) * MOVE_SPEED; 
        }
    }
}
