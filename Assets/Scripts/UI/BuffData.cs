using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "ScriptableObject/BuffData")]
public class BuffData : ScriptableObject
{
    public BUFF BuffKind;                       // 버프 종류
    public float numerical;                     // 수치

    public int duration;                        // 지속시간 (카운트를 위해 정수)

    public bool check_buff;                     // 버프인지 디버프인지
    public bool check_nesting;                  // 중첩 여부

    public string buffname;     // 버프이름
    
    [TextArea]
    public string buffcontent;      // 버프 설명
    
    public Sprite buffsprite;
}
