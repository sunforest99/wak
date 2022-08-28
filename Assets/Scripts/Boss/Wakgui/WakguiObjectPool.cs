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
        for (int i = 0; i < 20; i++)
        {
            GameObject temp = Instantiate(cristal, this.transform);
            cristalPool.Add(temp.GetComponent<Cristal>());
            temp.transform.parent = cristalParent;
            temp.SetActive(false);
        }
        for (int i = 0; i < 25; i++)
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
                knifePool[i].transform.position = new Vector3(posX, 0.14f, posZ);
                knifePool[i].SetActive(true);
                break;
            }
        }
    }

    public void WaveObject(float posX, float posZ, int rand)
    {
        for (int i = 0; i < wavesPool.Count; i++)
        {
            if (!wavesPool[i].gameObject.activeSelf)
            {
                wavesPool[i].gameObject.SetActive(true);
                wavesPool[i].transform.position = new Vector3(posX, 0.14f, posZ);
                wavesPool[i].rand = rand;
                break;
            }
        }
    }

    public void setCristalActive(float posX, float posZ)
    {
        for (int i = 0; i < cristalPool.Count; i++)
        {
            if (!cristalPool[i].gameObject.activeSelf)
            {
                cristalPool[i].gameObject.SetActive(true);
                cristalPool[i].transform.position = new Vector3(posX, 0.14f, posZ);
                cristalPool[i].objectPool = this;
                break;
            }
        }
    }

    public void setBulletActive(Vector3 pos, Vector3 rotate)
    {
        for (int i = 0; i < wavesPool.Count; i++)
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
