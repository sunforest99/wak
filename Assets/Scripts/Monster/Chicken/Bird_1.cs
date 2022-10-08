using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_1 : Monster
{    
    protected override void Awake()
    {
        base.Awake();
        _hp = 300000;
        _fullHp = 300000;
        _nearness = 2;
        _moveSpeed = 6f;

        ATTACK_DAMAGE = 1000;
        _damage = ATTACK_DAMAGE;
    }

    protected override void FixedUpdate()
    {
        if (isMoving)
        {
            if (_target.position.x < transform.position.x)
                _body.transform.rotation = Quaternion.Euler(20, 0, 0);
            else
                _body.transform.rotation = Quaternion.Euler(-20, 180, 0);
                
            // _target 한테 move
            if (Vector3.Distance(_target.position, transform.position) > _nearness)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(
                    transform.position,
                    new Vector3(_target.position.x, transform.position.y, _target.position.z),
                    _moveSpeed * Time.deltaTime
                ));
            }
            else
            {
                doSomething(decideAct());
            }
        }
    }
    
    protected override IEnumerator think()
    {
        yield return null;
    }

    void endActBird1Attack()
    {
        _damage = 0;
        isMoving = true;
    }

    protected override int decideAct()
    {
        // 기본공격 밖에 없음        
        return 1;           // 기본공격
    }
}
