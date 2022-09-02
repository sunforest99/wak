using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCast : DestroySelf
{
    public int distance;        //< 상 하 좌 우
    public bool checkFigure;
    [SerializeField] private GameObject rect;       // <! 0 사각형 1 삼각형
    [SerializeField] private GameObject triangle;
    [SerializeField] private Rigidbody rigid;

    void Start()
    {
        Move();
    }

    void Move()
    {
        switch (distance)
        {
            case 0:     // <! 상
                rigid.velocity = transform.TransformDirection(Vector3.forward * 5);
                break;
            case 1:     // <! 하
                rigid.velocity = transform.TransformDirection(Vector3.back * 5);
                break;
            case 2:     // <! 좌
                rigid.velocity = transform.TransformDirection(Vector3.left * 5);
                break;
            case 3:     // <! 우
                rigid.velocity = transform.TransformDirection(Vector3.right * 5);
                break;
            default:
                break;
        }
    }

    void figureSetting(GameObject figure, Collider user)
    {
        figure.transform.SetParent(user.transform.GetChild(0));
        figure.transform.SetAsFirstSibling();
        figure.transform.localPosition = new Vector2(0, 3.0f);
        figure.transform.localRotation = Quaternion.identity;
        destroySelf();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Character"))
        {
            if (other.transform.GetChild(0).name == "Triangle" || other.transform.GetChild(0).name == "Rect")
            {
                destroySelf();
                return;
            }
            if (checkFigure)
            {
                GameObject temp = Instantiate(triangle, Vector3.zero, Quaternion.identity);
                temp.name = "Triangle";
                figureSetting(temp, other);
            }
            else
            {
                GameObject temp = Instantiate(rect, Vector3.zero, Quaternion.identity);
                temp.name = "Rect";
                figureSetting(temp, other);
            }
        }
    }
}
