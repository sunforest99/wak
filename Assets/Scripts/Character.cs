using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Animator _anim;

    [SerializeField] GameObject[] footprints;

    int footprintIdx = 0;
    bool isMoving = false;

    // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Mesh Pro
    [SerializeField] Transform damagePopup;

    [SerializeField] private Transform skill;

    private List<TMPro.TextMeshProUGUI> cooltime_UI = new List<TMPro.TextMeshProUGUI>();
    private List<UnityEngine.UI.Image> skill_Img = new List<UnityEngine.UI.Image>();
    private bool[] checkSkill = new bool[5];

    void Start()
    {
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

    IEnumerator SkillCoolDown(int skillnum, float cooltime)        // <! ³ªÁß¿¡ ¹Ù²Ù±â
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            startMoving();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
            startMoving();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            startMoving();
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            _anim.SetBool("Move", false);

            isMoving = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.01f, 0, 0);
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(0.01f, 0, 0);
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.01f, 0);
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, 0.01f, 0);
            isMoving = true;
        }


        if (Input.GetKey(KeyCode.Q))
        {
            if (!checkSkill[0])
                StartCoroutine(SkillCoolDown(0, 15));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (!checkSkill[1])
                StartCoroutine(SkillCoolDown(1, 10));
        }
        else if (Input.GetKey(KeyCode.R))
        {
            if (!checkSkill[2])
                StartCoroutine(SkillCoolDown(2, 15));
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!checkSkill[3])
                StartCoroutine(SkillCoolDown(3, 15));
        }
        else if (Input.GetKey(KeyCode.F))
        {
            if (!checkSkill[4])
                StartCoroutine(SkillCoolDown(4, 15));
        }

        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
            createDamage(
                UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition),
                300
            );
        }
    }
}
