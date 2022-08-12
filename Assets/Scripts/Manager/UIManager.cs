using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
// public class SkillIcons
// {
//     public Sprite[] icons;
// }

public class UIManager : MonoBehaviour
{
    // ===== 스킬 =====================================================================================================
    // 0:전사, 1:법사, 2:힐러   (사용할땐 GameMng.I.userData.job 에서 항상 -1을 빼서 사용)
    // [SerializeField] SkillIcons[] job_skill_icons;
    // [SerializeField] UnityEngine.UI.Image[] skill_icons;
    // [SerializeField] GameObject skillIconWindow;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // void setSkillIcons()
    // {
    //     if (GameMng.I.userData.job == 0)
    //     {
    //         skillIconWindow.SetActive(false);
    //         return;
    //     }

    //     for (int i = 0; i < 5; i++)
    //     {
    //         skill_icons[i].sprite = job_skill_icons[GameMng.I.userData.job - 1].icons[i];
    //     }
    // }
}
