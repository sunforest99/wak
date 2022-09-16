using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image[] skillIcon;
    [SerializeField] TextMeshProUGUI[] skillName;

    [SerializeField] TextMeshProUGUI skillLevel;

    [SerializeField] private int currentSkillIndex;
    void OnEnable()
    {
        for (int i = 0; i < GameMng.I.character.skilldatas.Length; i++)
        {
            skillIcon[i].sprite = GameMng.I.character.skilldatas[i].getSkllImg;
            skillName[i].text = GameMng.I.character.skilldatas[i].getSkillName;
        }
    }

    // bool CheckSkillLevel() => GameMng.I.character.skilldatas[currentSkillIndex].skillLevel >= 1 &&
    // GameMng.I.character.skilldatas[currentSkillIndex].skillLevel <= 3 ? true : false;

    // public void ClickSkill(int num)
    // {
    //     currentSkillIndex = num;
    //     skillLevel.text = GameMng.I.character.skilldatas[num].skillLevel.ToString();
    // }

    // public void SkillLevelUp()
    // {
    //     if (GameMng.I.character.skilldatas[currentSkillIndex].skillLevel < 3)
    //     {
    //         GameMng.I.character.skilldatas[currentSkillIndex].skillLevel++;
    //         skillLevel.text = GameMng.I.character.skilldatas[currentSkillIndex].skillLevel.ToString();
    //     }
    // }

    // public void SkillLevelDown()
    // {
    //     if (GameMng.I.character.skilldatas[currentSkillIndex].skillLevel > 1)
    //     {
    //         GameMng.I.character.skilldatas[currentSkillIndex].skillLevel--;
    //         skillLevel.text = GameMng.I.character.skilldatas[currentSkillIndex].skillLevel.ToString();
    //     }
    // }
}
