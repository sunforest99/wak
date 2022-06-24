using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    Animator _anim;

    [SerializeField]
    GameObject[] footprints;

    int footprintIdx = 0;
    bool isMoving = false;

    // µ¥¹ÌÁö Mesh Pro
    [SerializeField]
    Transform damagePopup;

    void Start()
    {
    }

    void Update()
    {
        inputKey();
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

        }
        else if (Input.GetKey(KeyCode.E))
        {

        }
        else if (Input.GetKey(KeyCode.Z))
        {

        }
        else if (Input.GetKey(KeyCode.X))
        {

        }
        else if (Input.GetKey(KeyCode.C))
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            createDamage(
                UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition),
                300
            );
        }
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
}
