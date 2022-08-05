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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (checkFigure)
            {
                GameObject temp = Instantiate(triangle, Vector3.zero, Quaternion.identity);     // TODO: 이거 함수로 합쳐주기 그리고 플레이어 자식 가져와서 이름 비교해서 있으면 그냥 디스트로이
                temp.transform.SetParent(other.transform.parent);
                temp.transform.SetAsFirstSibling();
                temp.transform.localPosition = new Vector2(other.transform.localPosition.x, other.transform.localPosition.y + 5.0f);
                destroySelf();
            }
            else
            {
                GameObject temp = Instantiate(rect, Vector3.zero, Quaternion.identity);
                temp.transform.SetParent(other.transform.parent);
                temp.transform.SetAsFirstSibling();
                temp.transform.localPosition = new Vector2(other.transform.localPosition.x, other.transform.localPosition.y + 5.0f);
                destroySelf();
            }
        }
    }
}
