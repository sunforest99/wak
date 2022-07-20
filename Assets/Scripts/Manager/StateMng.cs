using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMng : MonoBehaviour
{
    [SerializeField] Image[] PartyHPImg = new Image[4];
    [SerializeField] Image[] PartyShieldImg = new Image[4];

    [SerializeField] float[] fFullPartyHP = new float[4];
    [SerializeField] float[] fFullPartyShield = new float[4];
    [SerializeField] float[] fPartyHP = new float[4];
    [SerializeField] float[] fPartyShield = new float[4];
    [SerializeField] float[] fShieldPos = new float[4];

    float fImageSize;

    // Start is called before the first frame update
    void Start()
    {
        fImageSize = 148.0f;
        for (int i = 0; i < 4; i++)
        {
            fFullPartyHP[i] = 100;
            fFullPartyShield[i] = 100;
            fPartyHP[i] = 100;
            fPartyShield[i] = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShieldPos();
    }

    void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            PartyHPImg[i].rectTransform.localScale = new Vector3(fPartyHP[i] / fFullPartyHP[i], 1.0f, 1.0f);
            PartyShieldImg[i].rectTransform.localScale = new Vector3(fPartyShield[i] / fFullPartyShield[i], 1.0f, 1.0f);
            if(fPartyHP[i] + fPartyShield[i] <= fFullPartyHP[i]){
                fShieldPos[i] = PartyHPImg[i].rectTransform.anchoredPosition.x + (fImageSize * PartyHPImg[i].rectTransform.localScale.x);
                PartyShieldImg[i].rectTransform.pivot = new Vector2(0.0f, 0.5f);
            }
            else{       // <! 풀체력보다 클때
                PartyShieldImg[i].rectTransform.pivot = new Vector2(1.0f, 0.5f);
                fShieldPos[i] = fImageSize / 2;
            }
            PartyShieldImg[i].rectTransform.anchoredPosition = new Vector2(fShieldPos[i], 0.0f);
        }
    }
}
