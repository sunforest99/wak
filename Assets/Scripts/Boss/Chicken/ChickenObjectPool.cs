using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObjectPool : MonoBehaviour
{
    [Header("오브젝트")]
    public GameObject bird;
    public GameObject rock;

    [Header("오브젝트 부모")]
    [SerializeField] Transform birdParent;
    [SerializeField] Transform rockParent;

    [Header("오브젝트 리스트")]
    [SerializeField] public List<Bird> birdPool;
    [SerializeField] public List<GameObject> rockPool;

    // 소횐되는 오브젝트 y좌표 
    [SerializeField] private float posY;
    public List<int> rand = new List<int>();

    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject temp = Instantiate(bird, this.transform);
            birdPool.Add(temp.GetComponent<Bird>());
            temp.transform.parent = birdParent;
            temp.SetActive(false);
        }
        for (int i = 0; i < 12; i++)
        {
            GameObject temp = Instantiate(rock, this.transform);
            rockPool.Add(temp);
            temp.transform.parent = rockParent;
            temp.SetActive(false);
        }
    }

    public void setBridActive(int pos)
    {
        for (int i = 0; i < birdPool.Count; i++)
        {
            if (!birdPool[i].gameObject.activeSelf)
            {
                birdPool[i].transform.position = new Vector3(0, 1f, 0f);
                birdPool[i].rand = pos;
                birdPool[i].gameObject.SetActive(true);
            }
        }
    }

    public void setRockActive()
    {
        foreach (var trans in NetworkMng.I.v_users.Values)
        {
            for (int i = 0; i < rockPool.Count; i++)
            {
                if (!rockPool[i].gameObject.activeSelf)
                {
                    rockPool[i].transform.position = new Vector3(trans.transform.position.x, 5.0f, trans.transform.position.z);
                    rockPool[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}