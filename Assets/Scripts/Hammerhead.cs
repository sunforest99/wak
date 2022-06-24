using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammerhead : Boss
{
    void Start()
    {
        moveSpeed = 0.3f;
        startHp = 14242442;
        bossnameText.text = bossName;
        currentHp = startHp;
    }

    void Update()
    {
        base.ChangeHpText();
        base.ChangeHpbar();
        base.BossMove();
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
