using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction : MonoBehaviour
{
    [SerializeField] private Wakgui wakgui;
    [SerializeField] private GameObject shadow;

    [Header("순서 왼위, 왼아래, 우위, 우아래")]
    [SerializeField] Vector3[] circlevec = new Vector3[4];
    // [SerializeField] List<int> visit;

    void SetStap() => wakgui.action = WAKGUI_ACTION.BASE_STAP;

    void SetSlash() => wakgui.action = WAKGUI_ACTION.BASE_SLASH;

    void SetRoar() => wakgui.action = WAKGUI_ACTION.BASE_ROAR;

    void SetRush() => wakgui.action = WAKGUI_ACTION.BASE_RUSH;

    void SetPoo() => wakgui.action = WAKGUI_ACTION.PATTERN_POO;

    void SetKnife() => wakgui.action = WAKGUI_ACTION.PATTERN_KNIFE;

    void SetJump()
    {
        wakgui.action = WAKGUI_ACTION.PATTERN_JUMP;
        wakgui.jump = true;
    }

    void SetCristal() => wakgui.action = WAKGUI_ACTION.PATTERN_CRISTAL;

    void SetWave() => wakgui.action = WAKGUI_ACTION.PATTERN_WAVE;

    void SetCounter() => wakgui.action = WAKGUI_ACTION.PATTERN_COUNTER;

    void SetCircle() => wakgui.action = WAKGUI_ACTION.PATTERN_CIRCLE;

    void SetIdle()
    {
        wakgui.action = WAKGUI_ACTION.IDLE;
        wakgui.isThink = false;
        // StartCoroutine(wakgui.Think());
    }

    void SetTelePort() => wakgui.action = WAKGUI_ACTION.TELEPORT;

    void SetTelePortSpawn() => wakgui.action = WAKGUI_ACTION.TELEPORT_SPAWN;

    void SetJumpPostion() => wakgui.jump = false;
    void SetTelePortPostion() => this.transform.parent.parent.position = new Vector3(GameMng.I.mapCenter.x, -3.6f);

    void CreateCircle()
    {
        for (int i = 0; i < circlevec.Length; i++)
        {
            GameObject temp = Instantiate(wakgui.getCircle[i], circlevec[wakgui.visit[i]], Quaternion.identity);
            wakgui.marblelist.Add(temp.GetComponent<Marble>());
            wakgui.marblelist[i].uniqueNum = i;
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
            GameObject temp = Instantiate(wakgui.getOutcast, new Vector3(wakgui.transform.position.x, 0.5f, wakgui.transform.position.z), Quaternion.Euler(20.0f, 0, 0));
            wakgui.outCasts.Add(temp.GetComponent<OutCast>());
            wakgui.outCasts[i].distance = i;
        }
        wakgui.outCasts[wakgui.outcastRand].checkFigure = true;
    }

    void CreateTotem()
    {
        Instantiate(wakgui.getTotem[0], new Vector3(-11.0f, GameMng.I.mapCenter.y, 0), Quaternion.Euler(90, 0, 0));
        Instantiate(wakgui.getTotem[1], new Vector3(11.0f, GameMng.I.mapCenter.y, 0), Quaternion.Euler(90, 0, 0));
    }

    void CreateWaves()
    {
        for(int i = 0; i < wakgui.spawnPattenVec.Count; i++)
        {
            wakgui.objectPool.setWaveObject(wakgui.spawnPattenVec[i].x,wakgui.spawnPattenVec[i].z);
        }
    }

    void CreateCristal()
    {
       for(int i = 0; i < wakgui.spawnPattenVec.Count; i++)
        {
            wakgui.objectPool.setCristalActive(wakgui.spawnPattenVec[i].x,wakgui.spawnPattenVec[i].z, i);
        }
    }

    void CreateKnife()
    {
        for(int i = 0; i < wakgui.spawnPattenVec.Count; i++)
        {
            wakgui.objectPool.setKnifeActive(wakgui.spawnPattenVec[i].x,wakgui.spawnPattenVec[i].z);
        }
    }

    void StartMoveOutcast()
    {
        for (int i = 0; i < wakgui.outCasts.Count; i++)
        {
            wakgui.outCasts[i].distance = i;
        }
    }

    void RushForce()
    {
        if (wakgui.isRFlip)
            wakgui.rigid.AddForce(new Vector3(20, 0, 0), ForceMode.Impulse);
        else
            wakgui.rigid.AddForce(new Vector3(-20, 0, 0), ForceMode.Impulse);
    }

    void HideShadow()
    {
        shadow.SetActive(false);
    }
    void ShowShadow()
    {
        shadow.SetActive(true);
    }
}