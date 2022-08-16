using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직업 없는 Character
public class Person : Character
{
    public override void init()
    {
        _job = JOB.NONE;
        // DASH_SPEED = 20;
        MOVE_SPEED = 6;
        // DASH_COOLTIME = 6;
        // WAKEUP_COOLTIME = 10;
    
        _stat = new Stat(100, 100, 1, 1, 1, 1, 15, 1);
    }
    
}
