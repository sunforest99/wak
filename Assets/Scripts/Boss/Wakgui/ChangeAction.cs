using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction : MonoBehaviour
{
    [SerializeField] private Wakgui wakgui;
    [SerializeField] Vector2[] circlevec = new Vector2[4];
    [SerializeField] List<int> visit;

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
        Debug.Log("setIdle");
    }

    void SetTelePort() => wakgui.action = WAKGUI_ACTION.TELEPORT;

    void SetTelePortSpawn() => wakgui.action = WAKGUI_ACTION.TELEPORT_SPAWN;

    void SetJumpPostion() => this.transform.parent.localPosition = GameMng.I.targetList[GameMng.I.targetCount].transform.localPosition;
    void SetTelePortPostion() => this.transform.parent.position = GameMng.I.mapCenter;

    void CreateCircle()
    {
        int rand = Random.Range(0, 4);
        for (int j = 0; j < circlevec.Length;)
        {
            if (visit.Contains(rand))
            {
                rand = Random.Range(0, 4);
            }
            else
            {
                visit.Add(rand);
                j++;
            }
        }
        for (int i = 0; i < circlevec.Length; i++)
        {
            GameObject temp = Instantiate(wakgui.getCircle[i], circlevec[visit[i]], Quaternion.identity);
            wakgui.marblelist.Add(temp.GetComponent<Marble>());
        }
    }
}