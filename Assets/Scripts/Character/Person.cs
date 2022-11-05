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
        // DASH_COOLTIME = 6;
        // WAKEUP_COOLTIME = 10;
    
        _stat = new Stat(
            1       /* 받는 피해량 퍼센트 */,
            5       /* 대쉬 쿨타임 */,
            10      /* 기상기 쿨타임 */,
            11       /* 이동 속도 */
        );
    }
    
}
