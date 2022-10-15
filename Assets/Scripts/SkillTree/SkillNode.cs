using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode : MonoBehaviour
{
    public string skillName;
    public string prevNode;

    public GameObject center;
    public SkillTreeInfo skill;


    // private bool isActive = false;
    private int hogamdo = 0;  // 호감도 조건 달성 여부를 검사하는 임시 변수
     

    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Center");
        skill = GetComponent<SkillTreeInfo>();
        Debug.Log(skill.skillTreeInfo.GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * @ 스킬을 찍음
     */
    void TakeSkill()
    {

    }

    /*
     * @ 스킬을 찍을 수 있는지 검사
     */
    void CheckSkillCondition()
    {
        
    }
}
