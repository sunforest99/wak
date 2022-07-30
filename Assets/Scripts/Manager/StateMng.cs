using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateMng : MonoBehaviour
{
    [SerializeField] Image[] PartyHPImg = new Image[4];
    [SerializeField] Image[] PartyShieldImg = new Image[4];
    [SerializeField] Image PlayerHPImg;
    [SerializeField] Image PlayerShieldImg;
    [SerializeField] TextMeshProUGUI PlayerHPText;
    [SerializeField] float[] fFullPartyHP = new float[4];
    [SerializeField] float[] fFullPartyShield = new float[4];
    [SerializeField] float[] fPartyHP = new float[4];
    [SerializeField] float[] fPartyShield = new float[4];
    [SerializeField] float[] fShieldPos = new float[4];
    [SerializeField] float fPlayerFullHP;
    [SerializeField] float fPlayerHP;
    [SerializeField] float fPlayerShield;
    [SerializeField] float fPlayerShieldPos;

    float fImageSize;
    float fPlayerImgSize;

    // Start is called before the first frame update
    void Start()
    {
        fImageSize = 148.0f;
        fPlayerImgSize = 295.0f;
        fPartyHP[0] = fFullPartyHP[0] = fFullPartyShield[0] = fPlayerFullHP = fPlayerHP = 95959;
        fPartyShield[0] = fPlayerShield = 50000;
        for (int i = 1; i < 4; i++)
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
        PlayerHP();
    }

    void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            PartyHPImg[i].rectTransform.localScale = new Vector3(fPartyHP[i] / fFullPartyHP[i], 1.0f, 1.0f);
            PartyShieldImg[i].rectTransform.localScale = new Vector3(fPartyShield[i] / fFullPartyShield[i], 1.0f, 1.0f);
            if(fPartyHP[i] + fPartyShield[i] <= fFullPartyHP[i]){
                fShieldPos[i] = PartyHPImg[i].rectTransform.anchoredPosition.x + (fImageSize * PlayerHPImg.rectTransform.localScale.x);
                PartyShieldImg[i].rectTransform.pivot = new Vector2(0.0f, 0.5f);
            }
            else{       // <! 풀체력보다 클때
                PartyShieldImg[i].rectTransform.pivot = new Vector2(1.0f, 0.5f);
                fShieldPos[i] = fImageSize / 2;
            }
            PartyShieldImg[i].rectTransform.anchoredPosition = new Vector2(fShieldPos[i], 0.0f);
        }
    }

    void PlayerHP(){
        PlayerHPText.text = fPlayerHP.ToString() + " / " + fPlayerFullHP.ToString();
        fPartyHP[0] = fPlayerHP;
        fPartyShield[0] = fPlayerShield;
        PlayerHPImg.rectTransform.localScale = new Vector3(fPlayerHP / fPlayerFullHP, 1.0f, 1.0f);
        PlayerShieldImg.rectTransform.localScale = new Vector3(fPlayerShield / fPlayerFullHP, 1.0f, 1.0f);
        if(fPlayerHP + fPlayerShield <= fPlayerFullHP){
            fPlayerShieldPos = PlayerHPImg.rectTransform.anchoredPosition.x + (fPlayerImgSize * PlayerHPImg.rectTransform.localScale.x);
            PlayerShieldImg.rectTransform.pivot = new Vector2(0.0f, 0.5f);
        }
        else{       // <! 풀체력보다 클때
            PlayerShieldImg.rectTransform.pivot = new Vector2(1.0f, 0.5f);
            fPlayerShieldPos = fPlayerImgSize / 2;
        }
        PlayerShieldImg.rectTransform.anchoredPosition = new Vector2(fPlayerShieldPos, 0.0f);
    }
}
