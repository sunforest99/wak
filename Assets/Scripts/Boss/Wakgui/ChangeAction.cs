using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction : MonoBehaviour
{
    [SerializeField] private Wakgui wakgui;

    void SetStap() => wakgui.action  = WAKGUI_ACTION.BASE_STAP;
    
    void SetSlash() => wakgui.action  = WAKGUI_ACTION.BASE_SLASH;
    
    void SetRoar() => wakgui.action  = WAKGUI_ACTION.BASE_ROAR;
    
    void SetRush() => wakgui.action  = WAKGUI_ACTION.BASE_RUSH;
    
    void SetPoo() => wakgui.action  = WAKGUI_ACTION.PATTERN_POO;
    
    void SetKnife() => wakgui.action  = WAKGUI_ACTION.PATTERN_KNIFE;
    
    void SetJump() => wakgui.action  = WAKGUI_ACTION.PATTERN_JUMP;
    
    void SetCristal() => wakgui.action  = WAKGUI_ACTION.PATTERN_CRISTAL;
    
    void SetWave() => wakgui.action  = WAKGUI_ACTION.PATTERN_WAVE;
    
    void SetCounter() => wakgui.action  = WAKGUI_ACTION.PATTERN_COUNTER;
    
    void SetCircle() => wakgui.action  = WAKGUI_ACTION.PATTERN_CIRCLE;

    void SetIdle() => wakgui.action = WAKGUI_ACTION.IDLE;

    void SetJumpPostion() => this.transform.parent.localPosition = GameMng.I.targetList[GameMng.I.targetCount].localPosition;
}
