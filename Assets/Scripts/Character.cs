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

    void Start()
    {
    }

    void Update()
    {
        inputKey();
    }

    void inputKey()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            startMoving();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
            startMoving();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            startMoving();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _anim.SetBool("Move", false);

            isMoving = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.01f, 0, 0);
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(0.01f, 0, 0);
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0.01f, 0);
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 0.01f, 0);
            isMoving = true;
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
}
