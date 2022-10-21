using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldBuff : MonoBehaviour
{
    // BuffData의 데이터
    public int duration;                        // 지속시간 (카운트를 위해 정수)
    public int mount;                         // 쉴드량
    
    // 관리용 데이터
    public float countdown;

    public ShieldBuff(int duration, int mount)
    {
        this.duration = duration;
        this.countdown = duration;
        this.mount = mount;
    }

    public void resetCountdown()
    {
        this.countdown = this.duration;
    }

    public bool isActive()
    {
        return this.countdown <= this.duration;
    }
}

[System.Serializable]
public struct Player_HP_Numerical
{          // 체력에 필요한 수치 구조체
    public int fullHp;                    // 최대 체력
    public int fullShield;                // 최대 쉴드 << 필요한지 모르겠음
    public int Hp;                        // 현재 체력
    public int Shield_Mount;              // 현재 쉴드
    public float Shield_Pos;                // 쉴드 위치
}

[System.Serializable]
public struct PartyBuffGroup
{
    public PartyBuff[] userBuff;                             // 중하단 UI 플레이어 버프 게임오브젝트
}

// struct Player_HP_UI{
//     public Image HP_Img;
//     public Image Shield_Img;
// }

public class StateMng : MonoBehaviour
{
    public BuffData[] buffDatas;
    [SerializeField] private PartyBuffGroup[] partybuffGroups = new PartyBuffGroup[4];
    [SerializeField] private Buff[] ownBuff;        // 내꺼 버프

    public Image[] PartyHPImg = new Image[4];                                     // 좌측 UI 플레이어들 체력이미지
    public TMPro.TextMeshProUGUI[] PartyName;
    [SerializeField] Image[] PartyShieldImg = new Image[4];
    [SerializeField] Image PlayerHPImg;                                                     // 중하단 플레이어 체력 이미지
    [SerializeField] Image PlayerShieldImg;
    [SerializeField] TextMeshProUGUI PlayerHPText;                                          // 중하단 플레이어 체력 텍스트
    public Player_HP_Numerical[] Party_HP_Numerical = new Player_HP_Numerical[4]; // 좌측 UI 플레이어 수치
    public Player_HP_Numerical user_HP_Numerical;
    public List<ShieldBuff> user_Shield_Numerical;
    // public ShieldBuff user_shield = new Shil[4]_;
    
    public int nPlayerBuffCount;                                                                   // 플레이어의 버프 갯수
    public int nPlayerDeBuffCount;
    public BuffData b;
    float fImageSize;
    float fPlayerImgSize;




    void Start()
    {
        fImageSize = 148.0f;
        fPlayerImgSize = 358.0f;
        Party_HP_Numerical[0].fullHp = Party_HP_Numerical[0].fullShield = user_HP_Numerical.fullHp = Mathf.FloorToInt(942477 * Character._stat.incHPPer);
        Party_HP_Numerical[0].Hp = user_HP_Numerical.Hp = user_HP_Numerical.fullHp;
        // Party_HP_Numerical[0].Shield_Mount = user_HP_Numerical.Shield_Mount = 0;
        for (int i = 0; i < 4; i++)
        {
            Party_HP_Numerical[i].fullHp = 100;
            Party_HP_Numerical[i].fullShield = 100;
            Party_HP_Numerical[i].Hp = 100;
            Party_HP_Numerical[i].Shield_Mount = 10;
        }
        nPlayerBuffCount = 0;
        nPlayerDeBuffCount = 0;
    }

    void Update()
    {
        // 쉴드량 매 초마다 카운팅
        // for (int i = 0; i < Party_HP_Numerical.Length; i++)
        // {
        //     for (int j = Party_HP_Numerical[i].Shield.Count - 1; j >= 0; j--)
        //     {
        //         Party_HP_Numerical[i].Shield[j].countdown += Time.deltaTime;
        //         if (Party_HP_Numerical[i].Shield[j].countdown >= Party_HP_Numerical[i].Shield[j].duration) {
        //             // 유지시간 끝났으면 삭제
        //             Party_HP_Numerical[i].Shield.RemoveAt(j);
        //         }
        //     }
        // }
        for (int j = user_Shield_Numerical.Count - 1; j >= 0; j--)
        {
            user_Shield_Numerical[j].countdown += Time.deltaTime;
            if (user_Shield_Numerical[j].countdown >= user_Shield_Numerical[j].duration) {
                user_Shield_Numerical.RemoveAt(j);          // 쉴드 유지시간 끝났으면 삭제
                // TODO : 네트워크에 내 바뀐 체력 보내줌
            }
        }

        ShieldPos();
        PlayerHP();
    }

    void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            PartyHPImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].Hp / Party_HP_Numerical[i].fullHp, 1.0f, 1.0f);
            PartyShieldImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].Shield_Mount / Party_HP_Numerical[i].fullShield, 1.0f, 1.0f);
            if (Party_HP_Numerical[i].Hp + Party_HP_Numerical[i].Shield_Mount <= Party_HP_Numerical[i].fullHp)
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
        Party_HP_Numerical[0].Hp = user_HP_Numerical.Hp;
        Party_HP_Numerical[0].Shield_Mount = user_HP_Numerical.Shield_Mount;

        PlayerHPText.text = user_HP_Numerical.Hp.ToString() + " / " + user_HP_Numerical.fullHp.ToString();
        Party_HP_Numerical[0].Hp = user_HP_Numerical.Hp;
        Party_HP_Numerical[0].Shield_Mount = user_HP_Numerical.Shield_Mount;
        PlayerHPImg.rectTransform.localScale = new Vector3(user_HP_Numerical.Hp / user_HP_Numerical.fullHp, 1.0f, 1.0f);
        PlayerShieldImg.rectTransform.localScale = new Vector3(user_HP_Numerical.Shield_Mount / user_HP_Numerical.fullHp, 1.0f, 1.0f);
        if (user_HP_Numerical.Hp + user_HP_Numerical.Shield_Mount <= user_HP_Numerical.fullHp)
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

    public void ActiveBuff(BuffData buffData)
    {
        for (int i = 0; i < partybuffGroups.Length; i++)
        {
            for (int j = 0; j < partybuffGroups[i].userBuff.Length; j++)
            {
                if (partybuffGroups[i].userBuff[j].isApply && partybuffGroups[i].userBuff[j].buffData.name == buffData.name)
                {
                    partybuffGroups[i].userBuff[j].duration = buffData.duration;
                    break;
                }
                else if (!partybuffGroups[i].userBuff[j].isApply)
                {
                    ActiveOwnBuff(Character.usingSkill.getBuffData);
                    partybuffGroups[i].userBuff[j].buffData = buffData;
                    partybuffGroups[i].userBuff[j].gameObject.SetActive(true);
                    partybuffGroups[i].userBuff[j].isApply = true;
                    break;
                }
            }
        }
    }

    public void ActiveOwnBuff(BuffData buffData)
    {
        for (int i = 0; i < ownBuff.Length; i++)
        {
            if (ownBuff[i].isApply && ownBuff[i].buffData.name == buffData.name && buffData.check_nesting)
            {
                ownBuff[i].count++;
                ownBuff[i].duration = buffData.duration;
                break;
            }
            else if (ownBuff[i].isApply && ownBuff[i].buffData.name == buffData.name)
            {
                ownBuff[i].duration = buffData.duration;
                break;
            }
            else if (!ownBuff[i].isApply)
            {
                ownBuff[i].buffData = buffData;
                ownBuff[i].gameObject.SetActive(true);
                ownBuff[i].isApply = true;
                break;
            }
        }
        NetworkMng.I.SendMsg(string.Format("BUFF:{0}:{1}", NetworkMng.I.uniqueNumber, (int)buffData.BuffKind));
    }

    public void partyActiveBuff(int player, BUFF buff)
    {
        if (!partybuffGroups[player].userBuff[(int)buff].isApply)
        {
            partybuffGroups[player].userBuff[(int)buff].buffData = buffDatas[(int)buff];
            partybuffGroups[player].userBuff[(int)buff].isApply = true;
            partybuffGroups[player].userBuff[(int)buff].gameObject.SetActive(true);
        }
        else
            partybuffGroups[player].userBuff[(int)buff].duration = partybuffGroups[player].userBuff[(int)buff].buffData.duration;
    }

    public void forcedDeath()
    {
        user_HP_Numerical.Hp = 0;
        user_Shield_Numerical.Clear();
    }

    public void takeDamage(int dmg)
    {
        Debug.Log("데미지 : " + dmg);
        Debug.Log("받는 피해 증가량 : " + Character._stat.takenDamagePer);
        
        // 받는 피해 감소 적용
        dmg = Mathf.FloorToInt(dmg * Character._stat.takenDamagePer);

        Debug.Log("결과 데미지 : " + dmg);

        int temp = -1;
        for (int j = 0; j < user_Shield_Numerical.Count; j++)
        {
            // 가장 먼저 들어온 쉴드 먼저 데미지를 받음
            user_Shield_Numerical[j].mount -= dmg;

            if (user_Shield_Numerical[j].mount < 0)
            {
                dmg = -user_Shield_Numerical[j].mount;
                temp = j + 1;
            }
            else
            {
                dmg = 0;
                break;
            }
        }

        if (temp > 0)
            user_Shield_Numerical.RemoveRange(0, temp);

        user_HP_Numerical.Hp -= dmg;

        if (user_HP_Numerical.Hp <= 0)
        {
            // 사망
        }

        // TODO : 네트워크에 내 변경된 HP 보내기
    }

    public void removeAllDebuff()
    {
        for (int i = 0; i < partybuffGroups[0].userBuff.Length; i++)
        {
            partybuffGroups[0].userBuff[i].isApply = false;
        }
    }

    public void removeRandomDebuff()
    {
        List<int> debuffIdxList = new List<int>();
        for (int i = 0; i < partybuffGroups[0].userBuff.Length; i++)
        {
            if (partybuffGroups[0].userBuff[i].isApply && isDebuff(partybuffGroups[0].userBuff[i].buffData.BuffKind))
            {
                debuffIdxList.Add(i);
            }
        }

        if (debuffIdxList.Count > 0) {
            int randIdx = Random.Range(0, debuffIdxList.Count);

            partybuffGroups[0].userBuff[ debuffIdxList[randIdx] ].isApply = false;
        }
    }

    public bool isDebuff(BUFF b)
    {
        return b.Equals(BUFF.DEBUFF_BUPAE) || b.Equals(BUFF.DEBUFF_CHIMSIK) || b.Equals(BUFF.DEBUFF_JAMSIK) || b.Equals(BUFF.DEBUFF_SHIELD);
    }
}