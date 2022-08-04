using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateMng : MonoBehaviour
{
    const int BuffCount = 2;
    const int DeBuffCount = 4;

    public Buff[] BuffSc = new Buff[BuffCount];
    public Buff[] DeBuffSc = new Buff[DeBuffCount];

    public List<Buff> BuffStatus;                            // 현재 가지고있는 버프
    public List<Buff> DeBuffStatus;                          // 현재 가지고있는 디버프

    public GameObject[] PlayerBuffGams = new GameObject[BuffCount];               // 좌측 UI 버프, 디버프 게임오브젝트
    public GameObject[] PlayerDeBuffGams = new GameObject[DeBuffCount];

    [SerializeField] Transform PlayerBuffSlot;                                              // 좌측 UI 플레이어 버프 슬롯
    [SerializeField] Transform PlayerDeBuffSlot;

    [SerializeField] GameObject[] BuffGams = new GameObject[BuffCount];                     // 중하단 UI 플레이어 버프 게임오브젝트
    [SerializeField] GameObject[] DeBuffGams = new GameObject[DeBuffCount];

    [SerializeField] Image[] PartyHPImg = new Image[4];                                     // 좌측 UI 플레이어들 체력이미지
    [SerializeField] Image[] PartyShieldImg = new Image[4];
    [SerializeField] Image PlayerHPImg;                                                     // 중하단 플레이어 체력 이미지
    [SerializeField] Image PlayerShieldImg;
    [SerializeField] TextMeshProUGUI PlayerHPText;                                          // 중하단 플레이어 체력 텍스트
    [SerializeField] TextMeshProUGUI[] PlayerDebuffCountText = new TextMeshProUGUI[DeBuffCount];    // 중하단 플레이어 디버프 갯수 텍스트

    public Sprite[] BuffSp = new Sprite[BuffCount];                                         // 버프 스프라이트
    public Sprite[] DeBuffSp = new Sprite[DeBuffCount];

    [SerializeField] float[] fFullPartyHP = new float[4];                                   // 좌측 플레이어들 최대 체력
    [SerializeField] float[] fFullPartyShield = new float[4];
    [SerializeField] float[] fPartyHP = new float[4];
    [SerializeField] float[] fPartyShield = new float[4];
    [SerializeField] float[] fShieldPos = new float[4];
    [SerializeField] float fPlayerFullHP;                                                   // 플레이어의 최대 체력
    [SerializeField] float fPlayerHP;
    [SerializeField] float fPlayerShield;
    float fPlayerShieldPos;

    int nPlayerBuffCount;                                                                   // 플레이어의 버프 갯수
    int nPlayerDeBuffCount;

    bool[] bBuffKind = new bool[BuffCount];                                                 // 어떤 버프가 켜져 있는지
    bool[] bDeBuffKind = new bool[DeBuffCount];

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
        nPlayerBuffCount = 0;
        nPlayerDeBuffCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldPos();
        PlayerHP();
        PlayerBuffMng();
    }

    void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            PartyHPImg[i].rectTransform.localScale = new Vector3(fPartyHP[i] / fFullPartyHP[i], 1.0f, 1.0f);
            PartyShieldImg[i].rectTransform.localScale = new Vector3(fPartyShield[i] / fFullPartyShield[i], 1.0f, 1.0f);
            if (fPartyHP[i] + fPartyShield[i] <= fFullPartyHP[i])
            {
                fShieldPos[i] = PartyHPImg[i].rectTransform.anchoredPosition.x + (fImageSize * PlayerHPImg.rectTransform.localScale.x);
                PartyShieldImg[i].rectTransform.pivot = new Vector2(0.0f, 0.5f);
            }
            else
            {       // <! 풀체력보다 클때
                PartyShieldImg[i].rectTransform.pivot = new Vector2(1.0f, 0.5f);
                fShieldPos[i] = fImageSize / 2;
            }
            PartyShieldImg[i].rectTransform.anchoredPosition = new Vector2(fShieldPos[i], 0.0f);
        }
    }

    void PlayerHP()
    {
        PlayerHPText.text = fPlayerHP.ToString() + " / " + fPlayerFullHP.ToString();
        fPartyHP[0] = fPlayerHP;
        fPartyShield[0] = fPlayerShield;
        PlayerHPImg.rectTransform.localScale = new Vector3(fPlayerHP / fPlayerFullHP, 1.0f, 1.0f);
        PlayerShieldImg.rectTransform.localScale = new Vector3(fPlayerShield / fPlayerFullHP, 1.0f, 1.0f);
        if (fPlayerHP + fPlayerShield <= fPlayerFullHP)
        {
            fPlayerShieldPos = PlayerHPImg.rectTransform.anchoredPosition.x + (fPlayerImgSize * PlayerHPImg.rectTransform.localScale.x);
            PlayerShieldImg.rectTransform.pivot = new Vector2(0.0f, 0.5f);
        }
        else
        {       // <! 풀체력보다 클때
            PlayerShieldImg.rectTransform.pivot = new Vector2(1.0f, 0.5f);
            fPlayerShieldPos = fPlayerImgSize / 2;
        }
        PlayerShieldImg.rectTransform.anchoredPosition = new Vector2(fPlayerShieldPos, 0.0f);
    }

    void PlayerBuffMng()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int kind = Random.Range(0, 2);
            if (kind == 0)
            {
                int buffkind = Random.Range(0, BuffCount);
                bBuffKind[buffkind] = true;
                ActiveBuff(buffkind);
            }
            else if (kind == 1)
            {
                int debuffkind = Random.Range(0, DeBuffCount);
                bDeBuffKind[debuffkind] = true;
                ActiveDeBuff(debuffkind);
            }
        }
    }

    void ActiveBuff(int kind)
    {
        if (bBuffKind[kind])
        {
            if (BuffSc[kind].count == 0)                                                       // 해당 버프가 지금 있나
            {
                BuffGams[kind].SetActive(true);
                PlayerBuffGams[kind].SetActive(true);
                nPlayerBuffCount++;
                BuffStatus.Add(BuffGams[kind].GetComponent<Buff>());
                BuffStatus[BuffStatus.Count - 1].count++;
            }
            else
            {
                Buff tmp = BuffStatus.Find(name => name.BuffKind == (BUFF)kind);            // 버프의 이름을 확인하여 중첩 가능한 버프면 중첩 수 증가
                if(tmp.check_nesting)
                    tmp.count++;
            }
            bBuffKind[kind] = false;
        }
    }

    void ActiveDeBuff(int kind)
    {
        if (bDeBuffKind[kind])
        {
            if (DeBuffSc[kind].count == 0)                                                     // 해당 디버프가 지금 있나
            {
                DeBuffGams[kind].SetActive(true);
                PlayerDeBuffGams[kind].SetActive(true);;
                nPlayerDeBuffCount++;
                DeBuffStatus.Add(DeBuffGams[kind].GetComponent<Buff>());
                DeBuffStatus[DeBuffStatus.Count - 1].count++;
            }
            else
            {
                Buff tmp = DeBuffStatus.Find(name => name.DeBuffKind == (DEBUFF)kind);        // 디버프의 이름을 확인하여 중첩 가능한 버프면 중첩 수 증가
                if(tmp.check_nesting)
                    tmp.count++;
            }
            bDeBuffKind[kind] = false;
            //PlayerDebuffCountText[kind].text = 'x' + nDeBuffKind[kind].ToString();
        }
    }
}
