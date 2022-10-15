using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeInfo : MonoBehaviour
{
    public Dictionary<string, bool> skillTreeInfo = new Dictionary<string, bool>(); // key: 노드 이름, value: 해금 여부

    private string[] nodeNames = new string[] {
        "강인함-받는피해1",
        "강인함-받는피해2",
        "강인함-받는피해3",
        "강인함-체력스탯1",
        "강인함-체력스탯2",
        "강인함-체력스탯3",
        "강인함-받는회복량1",
        "강인함-받는회복량2",
        "강인함-받는회복량3",
        "데미지-백어택피해증가량1",
        "데미지-백어택피해증가량2",
        "데미지-백어택피해증가량3",
        "데미지-치명타확률1",
        "데미지-치명타확률2",
        "데미지-치명타확률3",
        "데미지-데미지스탯1",
        "데미지-데미지스탯2",
        "데미지-데미지스탯3",
        "기민함-이동속도1",
        "기민함-이동속도2",
        "기민함-이동속도3",
        "기민함-대쉬쿨타임1",
        "기민함-대쉬쿨타임2",
        "기민함-대쉬쿨타임3",
        "기민함-기상기쿨타임1",
        "기민함-기상기쿨타임2",
        "기민함-기상기쿨타임3",
        "능력-스킬1",
        "능력-스킬1-쿨타임1",
        "능력-스킬1-쿨타임2",
        "능력-스킬1-데미지증가1",
        "능력-스킬1-데미지증가2",
        "능력-스킬2",
        "능력-스킬2-쿨타임1",
        "능력-스킬2-쿨타임2",
        "능력-스킬2-데미지증가1",
        "능력-스킬2-데미지증가2",
        "능력-스킬3",
        "능력-스킬3-쿨타임1",
        "능력-스킬3-쿨타임2",
        "능력-스킬3-데미지증가1",
        "능력-스킬3-데미지증가2",
        "능력-스킬4",
        "능력-스킬4-쿨타임1",
        "능력-스킬4-쿨타임2",
        "능력-스킬4-데미지증가1",
        "능력-스킬4-데미지증가2",
        "능력-스킬5",
        "능력-스킬5-쿨타임1",
        "능력-스킬5-쿨타임2",
        "능력-스킬5-데미지증가1",
        "능력-스킬5-데미지증가2",
    };

    // Start is called before the first frame update
    void Start()
    {
        SetSkillTreeInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSkillTreeInfo()
    {
        for(int i = 0; i < nodeNames.Length; i++)
        {
            skillTreeInfo.Add(nodeNames[i], false);
        }
    }

    public Dictionary<string, bool> GetSkillTreeInfo()
    {
        return skillTreeInfo;
    }
}
