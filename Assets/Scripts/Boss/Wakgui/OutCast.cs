using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCast : DestroySelf
{
    public int distance;        //< 상 하 좌 우
    public bool checkFigure;
    [SerializeField] private GameObject rect;       // <! 0 사각형 1 삼각형
    [SerializeField] private GameObject triangle;

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (distance)
        {
            case 0:     // <! 상
                this.transform.Translate(Vector3.up * 5 * Time.deltaTime);
                break;
            case 1:     // <! 하
                this.transform.Translate(Vector3.down * 5 * Time.deltaTime);
                break;
            case 2:     // <! 좌
                this.transform.Translate(Vector3.left * 5 * Time.deltaTime);
                break;
            case 3:     // <! 우
                this.transform.Translate(Vector3.right * 5 * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    void figureSetting(GameObject figure, Collider user)
    {
        figure.transform.SetParent(user.transform);
        figure.transform.SetAsFirstSibling();
        figure.transform.localPosition = new Vector2(0, 3.0f);
        destroySelf();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>())
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
