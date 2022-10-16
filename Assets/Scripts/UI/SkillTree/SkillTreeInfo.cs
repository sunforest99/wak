using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeInfo : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI noticeMessage;
    [SerializeField] UnityEngine.UI.Button[] skillNodeBTs;
    [SerializeField] Sprite skillNodeSpr_Active;
    [SerializeField] Sprite skillNodeSpr_UnActive;
    [SerializeField] TMPro.TextMeshProUGUI haveLoveTxt;

    public Dictionary<Skill_TREE, bool> skillTreeInfo = new Dictionary<Skill_TREE, bool>(); // key: 노드 이름, value: 해금 여부

    void Start()
    {
        SetSkillTreeInfo();

        haveLoveTxt.text = (GameMng.I.userData.love - GameMng.I.userData.upgrade.Count) + " / " + GameMng.I.userData.love;
    }

    public void changeUpgrade(int node)
    {
        if (GameMng.I.userData.love - GameMng.I.userData.upgrade.Count <= 0)
        {
            showMessage("남은 호감도 기운이 없습니다.");
            return;
        }

        // Active On 하는 상황
        if (skillTreeInfo[(Skill_TREE)node] == false)
        {
            switch ((Skill_TREE)node)
            {
                case Skill_TREE.DAMAGE_2: case Skill_TREE.CRITICAL_PER_2: case Skill_TREE.INC_BACK_2:
                case Skill_TREE.TAKEN_2: case Skill_TREE.HP_2: case Skill_TREE.HEAL_2:
                case Skill_TREE.SPEED_2: case Skill_TREE.DASH_2: case Skill_TREE.WAKEUP_2:
                    if (GameMng.I.userData.love > 5)
                    {
                        showMessage("총 호감도 5개 이상을 수집해야합니다.");
                        return;
                    }
                    if (!GameMng.I.userData.upgrade.Contains((Skill_TREE)(node - 1)))
                    {
                        showMessage("이전 단계를 업그레이드 해야 합니다.");
                        return;
                    }
                    break;
                case Skill_TREE.DAMAGE_3: case Skill_TREE.CRITICAL_PER_3: case Skill_TREE.INC_BACK_3:
                case Skill_TREE.TAKEN_3: case Skill_TREE.HP_3: case Skill_TREE.HEAL_3:
                case Skill_TREE.SPEED_3: case Skill_TREE.DASH_3: case Skill_TREE.WAKEUP_3:
                    if (GameMng.I.userData.love > 10)
                    {
                        showMessage("총 호감도 10개 이상을 수집해야합니다.");
                        return;
                    }
                    if (!GameMng.I.userData.upgrade.Contains((Skill_TREE)(node - 1)))
                    {
                        showMessage("이전 단계를 업그레이드 해야 합니다.");
                        return;
                    }
                    break;
                case Skill_TREE.SKILL_0_COOL_1: case Skill_TREE.SKILL_0_DMG_1:
                case Skill_TREE.SKILL_1_COOL_1: case Skill_TREE.SKILL_1_DMG_1:
                case Skill_TREE.SKILL_2_COOL_1: case Skill_TREE.SKILL_2_DMG_1:
                case Skill_TREE.SKILL_3_COOL_1: case Skill_TREE.SKILL_3_DMG_1:
                case Skill_TREE.SKILL_4_COOL_1: case Skill_TREE.SKILL_4_DMG_1:
                    if (GameMng.I.userData.love > 7)
                    {
                        showMessage("총 호감도 7개 이상을 수집해야합니다.");
                        return;
                    }
                    if (!GameMng.I.userData.upgrade.Contains((Skill_TREE)(node - 1)))
                    {
                        showMessage("이전 단계를 업그레이드 해야 합니다.");
                        return;
                    }
                    break;
            }
            
            skillTreeInfo[(Skill_TREE)node] = true;
            skillNodeBTs[node].image.sprite = skillNodeSpr_UnActive;

            GameMng.I.userData.upgrade.Add((Skill_TREE)node);
            haveLoveTxt.text = (GameMng.I.userData.love - GameMng.I.userData.upgrade.Count) + " / " + GameMng.I.userData.love;

            return;
        }

        // Active Off 하는 상황
        switch ((Skill_TREE)node)
        {
            case Skill_TREE.DAMAGE_2: case Skill_TREE.CRITICAL_PER_2: case Skill_TREE.INC_BACK_2:
            case Skill_TREE.TAKEN_2: case Skill_TREE.HP_2: case Skill_TREE.HEAL_2:
            case Skill_TREE.SPEED_2: case Skill_TREE.DASH_2: case Skill_TREE.WAKEUP_2:
            case Skill_TREE.DAMAGE_1: case Skill_TREE.CRITICAL_PER_1: case Skill_TREE.INC_BACK_1:
            case Skill_TREE.TAKEN_1: case Skill_TREE.HP_1: case Skill_TREE.HEAL_1:
            case Skill_TREE.SPEED_1: case Skill_TREE.DASH_1: case Skill_TREE.WAKEUP_1:
            case Skill_TREE.SKILL_0_COOL_0: case Skill_TREE.SKILL_0_DMG_0:
            case Skill_TREE.SKILL_1_COOL_0: case Skill_TREE.SKILL_1_DMG_0:
            case Skill_TREE.SKILL_2_COOL_0: case Skill_TREE.SKILL_2_DMG_0:
            case Skill_TREE.SKILL_3_COOL_0: case Skill_TREE.SKILL_3_DMG_0:
            case Skill_TREE.SKILL_4_COOL_0: case Skill_TREE.SKILL_4_DMG_0:
                if (GameMng.I.userData.upgrade.Contains((Skill_TREE)(node + 1)))
                {
                    showMessage("이후 단계를 해제 해야 합니다.");
                    return;
                }
                break;
        }

        skillTreeInfo[(Skill_TREE)node] = false;
        skillNodeBTs[node].image.sprite = skillNodeSpr_Active;

        GameMng.I.userData.upgrade.Remove((Skill_TREE)node);
        haveLoveTxt.text = (GameMng.I.userData.love - GameMng.I.userData.upgrade.Count) + " / " + GameMng.I.userData.love;
    }

    void SetSkillTreeInfo()
    {
        // Dictionary 에 저장
        for (int i = 0; i < skillNodeBTs.Length; i++)
        {
            skillTreeInfo.Add((Skill_TREE)i, false);
        }
        // Dictionary Refresh
        for (int i = 0; i < GameMng.I.userData.upgrade.Count; i++)
        {
            skillTreeInfo[GameMng.I.userData.upgrade[i]] = true;
            skillNodeBTs[(int)GameMng.I.userData.upgrade[i]].image.sprite = skillNodeSpr_UnActive;
        }
    }

    void showMessage(string msg)
    {
        noticeMessage.gameObject.SetActive(false);
        noticeMessage.text = msg;
        noticeMessage.gameObject.SetActive(true);
    }
}
