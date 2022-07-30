using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum CHARACTER_STATE
{
    IDLE,                   // 일반 상태, 움직이거나 스킬 사용이 가능한 상태
    CANT_ANYTHING,          // 스킬 쓰는 상태, 아무것도 못함
    CAN_MOVE                // 스킬 쓰는 상태, 캔슬이 가능한 상태
}

public class Character : MonoBehaviour
{
    [SerializeField] Animator _anim;
    CHARACTER_STATE _state;

    [SerializeField] GameObject[] footprints;

    const float MAX_DASH_TIME = 0.1f;
    public float curDashTime = 0.1f;
    const float DASH_SPEED = 20;
    //public float dashStoppingSpeed = 0.1f;

    int footprintIdx = 0;
    bool isMoving = false;

    [SerializeField] private Transform skill;

    private List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    private List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();


    [SerializeField] private SkillData[] skilldatas = new SkillData[5];

    public SkillData usingSkill;

    private bool[] checkSkill = new bool[7];    // 스킬5개 + 대쉬 + 기상기

    void Start()
    {
        GameMng.I.character = this;
        GameMng.I.targetList.Add(this.transform);
        _state = CHARACTER_STATE.IDLE;
        for (int i = 0; i < skill.transform.childCount; i++)
        {
            skill_Img.Add(skill.GetChild(i).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>());
            cooltime_UI.Add(skill.GetChild(i).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>());
        }
    }

    void Update()
    {
        inputKey();
    }

    IEnumerator SkillCoolDown(int skillnum)        // <! 나중에 바꾸기
    {
        float cooltime = 0;
        if (skillnum == 5) {
            skill_Img[skillnum].transform.parent.gameObject.SetActive(true);
            cooltime = 6;
        }
        else
        {
            usingSkill = skilldatas[skillnum];
            cooltime = usingSkill.getColldown;
        }

        checkSkill[skillnum] = true;
        float time = 0.0f;
        skill_Img[skillnum].color = new Color32(175, 175, 175, 255);
        cooltime_UI[skillnum].gameObject.SetActive(true);

        while (time < cooltime)
        {
            time += Time.deltaTime;
            skill_Img[skillnum].fillAmount = time / cooltime;
            cooltime_UI[skillnum].text = Mathf.Floor(cooltime - time).ToString();
            yield return new WaitForEndOfFrame();
        }

        cooltime_UI[skillnum].gameObject.SetActive(false);
        skill_Img[skillnum].color = Color.white;
        checkSkill[skillnum] = false;
        
        if (skillnum == 5) {
            skill_Img[skillnum].transform.parent.gameObject.SetActive(false);
        }
        
    }

    void startMoving()
    {
        if (!isMoving) {
            _anim.SetBool("Move", true);
            isMoving = true;
            StartCoroutine(showFootprint());
        }
    }

    IEnumerator showFootprint()
    {
        footprints[footprintIdx].SetActive(true);
        footprints[footprintIdx].transform.position = transform.position - new Vector3(0, 0.65f, 0);
        footprintIdx = footprintIdx >= 2 ? 0 : footprintIdx + 1;

        yield return new WaitForSeconds(0.4f);
        
        if (_state == CHARACTER_STATE.CANT_ANYTHING)
            isMoving = false;
            
        if (isMoving)
            StartCoroutine(showFootprint());
        
    }

    void inputKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !checkSkill[5])
        {
            curDashTime = 0.0f;

            _anim.SetTrigger("Dash");
            transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * 3 * Time.deltaTime;

            _state = CHARACTER_STATE.CAN_MOVE;
            StartCoroutine(SkillCoolDown(5));
        }

        // 아무것도 아닌 상태가 아닌 경우는 이동이 가능한 상태
        if (_state == CHARACTER_STATE.CANT_ANYTHING)
            return;

        // 이동
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
        }
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * 3 * Time.deltaTime;

        // 대시 이동
        if (curDashTime < MAX_DASH_TIME)
        {
            curDashTime += Time.deltaTime;
            transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * DASH_SPEED * Time.deltaTime;
        }

        // 이동 애니메이션 관리
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            startMoving();
        }
        else
        {
            isMoving = false;
            _anim.SetBool("Move", false);
        }

        // 모든 스킬은 IDLE 상태에서만 가능하기 때문에 체크함
        if (_state != CHARACTER_STATE.IDLE)
            return;

        // 스킬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!checkSkill[0])
            {
                StartCoroutine(SkillCoolDown(0));
                _state = CHARACTER_STATE.CANT_ANYTHING;
                _anim.SetTrigger("Skill_Gal");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!checkSkill[1])
            {
                StartCoroutine(SkillCoolDown(1));
                _state = CHARACTER_STATE.CANT_ANYTHING;
                _anim.SetTrigger("Skill_AGDZ");
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (!checkSkill[2]) {
                StartCoroutine(SkillCoolDown(2));
                _state = CHARACTER_STATE.CAN_MOVE;
                _anim.SetTrigger("Skill_Bigrr");
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!checkSkill[3]){
                StartCoroutine(SkillCoolDown(3));
                _state = CHARACTER_STATE.CANT_ANYTHING;
                _anim.SetTrigger("Skill_SG");
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (!checkSkill[4]){
                StartCoroutine(SkillCoolDown(4));
                _state = CHARACTER_STATE.CANT_ANYTHING;
                _anim.SetTrigger("Skill_JH");
            }
        }
        // 임시 기절 키
        else if (Input.GetKeyDown(KeyCode.Y))
        {
                _state = CHARACTER_STATE.CANT_ANYTHING;
                _anim.SetTrigger("Sleep");
        }

        // 마우스 좌클릭 - 일반 공격
        if (Input.GetMouseButtonDown(0) && GameMng.I.dailogUI == null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // 좌우 반전
            if (Input.mousePosition.x < Screen.width / 2)
                transform.rotation = Quaternion.Euler(Vector3.zero);
            else
                transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));

            _state = CHARACTER_STATE.CANT_ANYTHING;
            _anim.SetTrigger("Attack");
        }
        // 핑
        else if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("@@@@@@@@@@@@@");
            GameMng.I.createPing(UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        // 마우스 우클릭 - 상호작용
        else if (Input.GetMouseButtonDown(1))
        {
            GameMng.I.mouseRaycast(this.transform.localPosition);
        }
    }

    void endAct()
    {
        _state = CHARACTER_STATE.IDLE;
        usingSkill = null;
    }
}
