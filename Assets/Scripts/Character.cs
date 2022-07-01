using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int footprintIdx = 0;
    bool isMoving = false;

    // 占쏙옙占쏙옙占쏙옙 Mesh Pro
    [SerializeField] Transform damagePopup;

    [SerializeField] private Transform skill;

    private List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    private List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();
    private bool[] checkSkill = new bool[5];


    void Start()
    {
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

    IEnumerator SkillCoolDown(int skillnum, float cooltime)        // <! 나중에 바꾸기
    {
        checkSkill[skillnum] = true;
        float time = 0.0f;
        skill_Img[skillnum].color = Color.gray;
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
    }

    void startMoving()
    {
        _anim.SetBool("Move", true);
        if (!isMoving)
            StartCoroutine(showFootprint());
    }

    IEnumerator showFootprint()
    {
        footprints[footprintIdx].SetActive(true);
        footprints[footprintIdx].transform.position = transform.position - new Vector3(0, 0.55f, 0);

        yield return new WaitForSeconds(0.25f);

        footprints[footprintIdx].SetActive(false);
        footprintIdx = footprintIdx >= 2 ? 0 : footprintIdx + 1;

        if (isMoving)
            StartCoroutine(showFootprint());
    }

    void createDamage(Vector2 pos, int damage)
    {
        Transform damageObj = Instantiate(damagePopup, pos, Quaternion.identity);
        Damage dmg = damageObj.GetComponent<Damage>();
        dmg.set(damage);
    }

    void inputKey()
    {
        if (_state != CHARACTER_STATE.IDLE)
            return;

        // 이동
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            isMoving = true;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            isMoving = true;
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
        }
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * 3 * Time.deltaTime;

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

        // 스킬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!checkSkill[0])
                StartCoroutine(SkillCoolDown(0, 15));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!checkSkill[1])
                StartCoroutine(SkillCoolDown(1, 10));
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (!checkSkill[2])
                StartCoroutine(SkillCoolDown(2, 15));
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!checkSkill[3])
                StartCoroutine(SkillCoolDown(3, 15));
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (!checkSkill[4])
                StartCoroutine(SkillCoolDown(4, 15));
        }

        // 마우스 좌클릭 - 일반 공격
        if (Input.GetMouseButtonDown(0))
        {
            // 좌우 반전
            if (Input.mousePosition.x < Screen.width / 2)
                transform.rotation = Quaternion.Euler(Vector3.zero);
            else
                transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));

            _state = CHARACTER_STATE.CANT_ANYTHING;
            _anim.SetTrigger("Attack");

            createDamage(
                UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition),
                300
            );
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
    }
}
