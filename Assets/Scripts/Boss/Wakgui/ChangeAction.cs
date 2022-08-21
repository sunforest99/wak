using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction : MonoBehaviour
{
    [SerializeField] private Wakgui wakgui;
    [SerializeField] Vector2[] circlevec = new Vector2[4];
    // [SerializeField] List<int> visit;

    void SetStap() => wakgui.action = WAKGUI_ACTION.BASE_STAP;

    void SetSlash() => wakgui.action = WAKGUI_ACTION.BASE_SLASH;

    void SetRoar() => wakgui.action = WAKGUI_ACTION.BASE_ROAR;

    void SetRush() => wakgui.action = WAKGUI_ACTION.BASE_RUSH;

    void SetPoo() => wakgui.action = WAKGUI_ACTION.PATTERN_POO;

    void SetKnife() => wakgui.action = WAKGUI_ACTION.PATTERN_KNIFE;

    void SetJump() => wakgui.action = WAKGUI_ACTION.PATTERN_JUMP;

    void SetCristal() => wakgui.action = WAKGUI_ACTION.PATTERN_CRISTAL;

    void SetWave() => wakgui.action = WAKGUI_ACTION.PATTERN_WAVE;

    void SetCounter() => wakgui.action = WAKGUI_ACTION.PATTERN_COUNTER;

    void SetCircle() => wakgui.action = WAKGUI_ACTION.PATTERN_CIRCLE;

    void SetIdle()
    {
        wakgui.action = WAKGUI_ACTION.IDLE;
        wakgui.Think();
    }

    void SetTelePort() => wakgui.action = WAKGUI_ACTION.TELEPORT;

    void SetTelePortSpawn() => wakgui.action = WAKGUI_ACTION.TELEPORT_SPAWN;

    void SetJumpPostion() => this.transform.parent.localPosition = wakgui.target.localPosition;
    void SetTelePortPostion() => this.transform.parent.position = new Vector2(GameMng.I.mapCenter.x, GameMng.I.mapCenter.y - 1.0f);

    void CreateCircle()
    {
        for (int i = 0; i < circlevec.Length; i++)
        {
            GameObject temp = Instantiate(wakgui.getCircle[i], circlevec[wakgui.visit[i]], Quaternion.identity);
            wakgui.marblelist.Add(temp.GetComponent<Marble>());
        }
        
        switch (wakgui.circle_answer)
        {
            case 0:
                wakgui.marblelist[2].answer = true;        // 빨 파 초 주
                break;
            case 1:
                wakgui.marblelist[3].answer = true;
                break;
            case 2:
                wakgui.marblelist[0].answer = true;
                break;
            case 3:
                wakgui.marblelist[1].answer = true;
                break;
            case 4:
                wakgui.marblelist[3].answer = true;
                break;
            case 5:
                wakgui.marblelist[0].answer = true;
                break;
        }


    }

    void SetOutcast() => wakgui.action = WAKGUI_ACTION.PATTERN_OUTCAST;

    void CreateOutcast()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(wakgui.getOutcast, GameMng.I.mapCenter, Quaternion.identity);
            wakgui.outCasts.Add(temp.GetComponent<OutCast>());
            wakgui.outCasts[i].distance = 4;
        }
        wakgui.outCasts[wakgui.outcastRand].checkFigure = true;
    }

    void CreateTotem()
    {
        Instantiate(wakgui.getTotem[0], new Vector2(-11.0f, GameMng.I.mapCenter.y), Quaternion.identity);
        Instantiate(wakgui.getTotem[1], new Vector2(11.0f, GameMng.I.mapCenter.y), Quaternion.identity);
    }

    void StartMoveOutcast()
    {
        for (int i = 0; i < wakgui.outCasts.Count; i++)
        {
            wakgui.outCasts[i].distance = i;
        }
    }
}