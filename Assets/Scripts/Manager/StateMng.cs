using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct Player_HP_Numerical
{          // 체력에 필요한 수치 구조체
    public float fullHp;                    // 최대 체력
    public float fullShield;                // 최대 쉴드 << 필요한지 모르겠음
    public float Hp;                        // 현재 체력
    public float Shield;                    // 현재 쉴드
    public float Shield_Pos;                // 쉴드 위치
}

// struct Player_HP_UI{
//     public Image HP_Img;
//     public Image Shield_Img;
// }

public class StateMng : MonoBehaviour
{
    public BuffData[] bufflist;
    const int BuffCount = 6;

    public Buff[] BuffSc = new Buff[BuffCount];

    public GameObject[] userBuff = new GameObject[BuffCount];                               // 중하단 UI 플레이어 버프 게임오브젝트
    public GameObject[] PlayerBuffGams = new GameObject[BuffCount];                         // 좌측 UI 버프, 디버프 게임오브젝트

    [SerializeField] Image[] PartyHPImg = new Image[4];                                     // 좌측 UI 플레이어들 체력이미지
    [SerializeField] Image[] PartyShieldImg = new Image[4];
    [SerializeField] Image PlayerHPImg;                                                     // 중하단 플레이어 체력 이미지
    [SerializeField] Image PlayerShieldImg;
    [SerializeField] TextMeshProUGUI PlayerHPText;                                          // 중하단 플레이어 체력 텍스트
    public Player_HP_Numerical[] Party_HP_Numerical = new Player_HP_Numerical[4]; // 좌측 UI 플레이어 수치
    public Player_HP_Numerical user_HP_Numerical;

    ///////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] float fPlayerHP;                                                   // 플레이어 체력 시각 효과를 위한 변수 나중에 꼭 지우기
    [SerializeField] float fPlayerShield;
    ///////////////////////////////////////////////////////////////////////////////////////////

    int nPlayerBuffCount;                                                                   // 플레이어의 버프 갯수
    int nPlayerDeBuffCount;

    bool[] bBuffKind = new bool[BuffCount];                                                 // 어떤 버프가 켜져 있는지

    float fImageSize;
    float fPlayerImgSize;

    void Start()
    {
        fImageSize = 148.0f;
        fPlayerImgSize = 295.0f;
        Party_HP_Numerical[0].fullHp = Party_HP_Numerical[0].fullShield = Party_HP_Numerical[0].Hp = user_HP_Numerical.fullHp = user_HP_Numerical.Hp = 95959;
        Party_HP_Numerical[0].Shield = user_HP_Numerical.Shield = 50000;
        fPlayerHP = 95959;
        fPlayerShield = 50000;
        for (int i = 1; i < 4; i++)
        {
            Party_HP_Numerical[i].fullHp = 100;
            Party_HP_Numerical[i].fullShield = 100;
            Party_HP_Numerical[i].Hp = 100;
            Party_HP_Numerical[i].Shield = 10;
        }
        nPlayerBuffCount = 0;
        nPlayerDeBuffCount = 0;
    }

    void Update()
    {
        ShieldPos();
        PlayerHP();
        PlayerBuffMng();
        user_HP_Numerical.Hp = fPlayerHP;
        user_HP_Numerical.Shield = fPlayerShield;
    }

    void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            PartyHPImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].Hp / Party_HP_Numerical[i].fullHp, 1.0f, 1.0f);
            PartyShieldImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].Shield / Party_HP_Numerical[i].fullShield, 1.0f, 1.0f);
            if (Party_HP_Numerical[i].Hp + Party_HP_Numerical[i].Shield <= Party_HP_Numerical[i].fullHp)
            {
                Party_HP_Numerical[i].Shield_Pos = PartyHPImg[i].rectTransform.anchoredPosition.x + (fImageSize * PlayerHPImg.rectTransform.localScale.x);
                PartyShieldImg[i].rectTransform.pivot = new Vector2(0.0f, 0.5f);
            }
            else
            {       // <! 풀체력보다 클때
                PartyShieldImg[i].rectTransform.pivot = new Vector2(1.0f, 0.5f);
                Party_HP_Numerical[i].Shield_Pos = fImageSize / 2;
            }
            PartyShieldImg[i].rectTransform.anchoredPosition = new Vector2(Party_HP_Numerical[i].Shield_Pos, 0.0f);
        }
    }

    void PlayerHP()
    {
        PlayerHPText.text = user_HP_Numerical.Hp.ToString() + " / " + user_HP_Numerical.fullHp.ToString();
        Party_HP_Numerical[0].Hp = user_HP_Numerical.Hp;
        Party_HP_Numerical[0].Shield = user_HP_Numerical.Shield;
        PlayerHPImg.rectTransform.localScale = new Vector3(user_HP_Numerical.Hp / user_HP_Numerical.fullHp, 1.0f, 1.0f);
        PlayerShieldImg.rectTransform.localScale = new Vector3(user_HP_Numerical.Shield / user_HP_Numerical.fullHp, 1.0f, 1.0f);
        if (user_HP_Numerical.Hp + user_HP_Numerical.Shield <= user_HP_Numerical.fullHp)
        {
            user_HP_Numerical.Shield_Pos = PlayerHPImg.rectTransform.anchoredPosition.x + (fPlayerImgSize * PlayerHPImg.rectTransform.localScale.x);
            PlayerShieldImg.rectTransform.pivot = new Vector2(0.0f, 0.5f);
        }
        else
        {       // <! 풀체력보다 클때
            PlayerShieldImg.rectTransform.pivot = new Vector2(1.0f, 0.5f);
            user_HP_Numerical.Shield_Pos = fPlayerImgSize / 2;
        }
        PlayerShieldImg.rectTransform.anchoredPosition = new Vector2(user_HP_Numerical.Shield_Pos, 0.0f);
    }

    void PlayerBuffMng(/*bool kind, int num*/)                      // 버프가 다 구현되면 호출할 함수 kind = 버프인지 디버프인지, num 버프 종류
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int buffkind = Random.Range(0, BuffCount);
            bBuffKind[buffkind] = true;
            ActiveBuff(buffkind);
        }
    }

    void ActiveBuff(int num)
    {
        if (bBuffKind[num])
        {
            if (!BuffSc[num].isApply)                                                       // 해당 버프가 지금 있나
            {
                userBuff[num].SetActive(true);
                PlayerBuffGams[num].SetActive(true);
                nPlayerBuffCount++;
                BuffSc[num].kind = num;
                BuffSc[num].count++;
                BuffSc[num].isApply = true;
                BuffSc[num].BuffImg.enabled = true;
            }
            else
            {
                if (BuffSc[num].buffData.check_nesting)
                    BuffSc[num].count++;
                BuffSc[num].apply_count = BuffSc[num].buffData.duration;
            }
            bBuffKind[num] = false;
        }
    }
}
