using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakguiObjectPool : MonoBehaviour
{
    [Header("오브젝트")]
    public GameObject waves;
    public GameObject knife;
    public GameObject cristal;
    public GameObject bullet;

    [Header("오브젝트 부모")]
    [SerializeField] Transform waveParent;

    [SerializeField] Transform knifeParent;

    [SerializeField] Transform cristalParent;

    [SerializeField] Transform bulletParent;

    [Header("오브젝트 리스트")]
    [SerializeField] private List<Waves> wavesPool;
    [SerializeField] private List<GameObject> knifePool;
    [SerializeField] private List<Cristal> cristalPool;
    [SerializeField] private List<GameObject> bulletPool;

    // 소횐되는 오브젝트 y좌표 
    [SerializeField] private float posY;
    public List<int> rand = new List<int>();

    void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject temp = Instantiate(waves, this.transform);
            wavesPool.Add(temp.GetComponent<Waves>());
            temp.transform.parent = waveParent;
            temp.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject temp = Instantiate(knife, this.transform);
            knifePool.Add(temp);
            temp.transform.parent = knifeParent;
            temp.SetActive(false);
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject temp = Instantiate(cristal, this.transform);
            cristalPool.Add(temp.GetComponent<Cristal>());
            cristalPool[i].uniqueNum = i;
            temp.transform.parent = cristalParent;
            temp.SetActive(false);
        }
        for (int i = 0; i < 150; i++)
        {
            GameObject temp = Instantiate(bullet, this.transform);
            bulletPool.Add(temp);
            temp.transform.parent = bulletParent;
            temp.SetActive(false);
        }
    }

    public void setKnifeActive(float posX, float posZ)
    {
        for (int i = 0; i < knifePool.Count; i++)
        {
            if (!knifePool[i].activeSelf)
            {
                knifePool[i].transform.position = new Vector3(posX, posY, posZ);
                knifePool[i].SetActive(true);
                break;
            }
        }
    }

    public void enableCirstal(int uniqueNum)
    {
        cristalPool[uniqueNum].transform.position = Vector3.zero;
        cristalPool[uniqueNum].gameObject.SetActive(false);
    }

    public void setWaveObject(float posX, float posZ)
    {
        for (int i = 0; i < wavesPool.Count; i++)
        {
            if (!wavesPool[i].gameObject.activeSelf)
            {
                wavesPool[i].gameObject.SetActive(true);
                wavesPool[i].transform.position = new Vector3(posX, posY, posZ);
                wavesPool[i].rand = rand[i];
                break;
            }
        }
    }

    public void setCristalActive(float posX, float posZ, int num)
    {
        for (int i = 0; i < cristalPool.Count; i++)
        {
            if (!cristalPool[i].gameObject.activeSelf)
            {
                cristalPool[i].gameObject.SetActive(true);
                cristalPool[i].transform.position = new Vector3(posX, posY, posZ);
                cristalPool[i].objectPool = this;
                cristalPool[i].spawnNum = num;
                break;
            }
        }
    }

    public void setBulletActive(Vector3 pos, Vector3 rotate)
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].gameObject.activeSelf)
            {
                bulletPool[i].gameObject.SetActive(true);
                bulletPool[i].transform.position = pos;
                bulletPool[i].transform.localRotation = Quaternion.Euler(rotate);
                break;
            }
        }
    }
}
