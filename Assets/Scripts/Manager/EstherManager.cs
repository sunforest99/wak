using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstherManager : MonoBehaviour
{
    [SerializeField] Animator estherAnim;
    [SerializeField] UnityEngine.UI.Image gaugeImg;
    float gauge = 0f;
    
    void Start()
    {
        setGauge(0);
    }

    void Update()
    {
        if (gauge < 1f) {
            addGauge(0.5f * Time.deltaTime);
            if (gauge >= 1f) {
                estherAnim.SetBool("isFull", true);
            }
        } else {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z)) {
                useEsther(0);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.X)) {
                useEsther(1);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C)) {
                useEsther(2);
            }
        }
    }

    void useEsther(int estherCode) {
        setGauge(0);
        estherAnim.SetBool("isFull", false);
    }

    void setGauge(float mount) {
        gauge = mount;
        gaugeImg.fillAmount = gauge;
    }

    public void addGauge(float mount) {
        gauge += mount;
        gaugeImg.fillAmount = gauge;
    }
}
