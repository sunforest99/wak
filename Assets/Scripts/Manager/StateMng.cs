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
public struct User_HP_Numerical
{          // 체력에 필요한 수치 구조체
    public int fullHp;                    // 최대 체력
    //public int fullShield;                // 최대 쉴드 << 필요한지 모르겠음
    public int Hp;                        // 현재 체력
    // public int Shield_Mount;              // 현재 쉴드
    public float shieldPer;
    public float Shield_Pos;                // 쉴드 위치
}
[System.Serializable]
public struct Player_HP_Numerical
{
    public float hpPer;
    public float shieldPer;
    public float Shield_Pos;
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
    public User_HP_Numerical user_HP_Numerical;
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
        user_HP_Numerical.Hp = user_HP_Numerical.fullHp = 1;
        
        for (int i = 0; i < 4; i++)
        {
            Party_HP_Numerical[i].hpPer = 1;
            Party_HP_Numerical[i].shieldPer = 0;
        }
        nPlayerBuffCount = 0;
        nPlayerDeBuffCount = 0;
        
        ShieldPos();
        PlayerHP();

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
        
    }

    public void ShieldPos()
    {
        for (int i = 0; i < 4; i++)
        {
            // 파티창 HP 퍼센테이지 적용
            PartyHPImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].hpPer, 1.0f, 1.0f);
            // 파티창 쉴드 퍼센테이지 적용
            PartyShieldImg[i].rectTransform.localScale = new Vector3(Party_HP_Numerical[i].shieldPer, 1.0f, 1.0f);
            if (Party_HP_Numerical[i].hpPer + Party_HP_Numerical[i].shieldPer <= 1)
            {
                Party_HP_Numerical[i].Shield_Pos = PartyHPImg[i].rectTransform.anchoredPosition.x + (fImageSize * PlayerHPImg.rectTransform.localScale.x);
                PartyShieldImg[i].rectTransform.pivot = new Vector2(0.0f, 0.5f);
            }
            else
            {
                // 풀체력보다 클때
                PartyShieldImg[i].rectTransform.pivot = new Vector2(1.0f, 0.5f);
                Party_HP_Numerical[i].Shield_Pos = fImageSize / 2;
            }
            PartyShieldImg[i].rectTransform.anchoredPosition = new Vector2(Party_HP_Numerical[i].Shield_Pos, 0.0f);
        }
    }

    public void PlayerHP()
    {
        // 내 파티창 HP 퍼센테이지
        Party_HP_Numerical[0].hpPer = user_HP_Numerical.Hp / (float)user_HP_Numerical.fullHp;

        // 내 파티창 쉴드 퍼센테이지
        if (user_HP_Numerical.shieldPer > 1)
            user_HP_Numerical.shieldPer = 1;
        Party_HP_Numerical[0].shieldPer = user_HP_Numerical.shieldPer;

        // 내 체력창 HP 상태
        PlayerHPText.text = user_HP_Numerical.Hp.ToString() + " / " + user_HP_Numerical.fullHp.ToString();
        PlayerHPImg.rectTransform.localScale = new Vector3(Party_HP_Numerical[0].hpPer, 1.0f, 1.0f);
        PlayerShieldImg.rectTransform.localScale = new Vector3(user_HP_Numerical.shieldPer, 1.0f, 1.0f);

        if (Party_HP_Numerical[0].hpPer + user_HP_Numerical.shieldPer <= 1)
        {
            // 쉴드 일반적인 상태
            user_HP_Numerical.Shield_Pos = PlayerHPImg.rectTransform.anchoredPosition.x + (fPlayerImgSize * PlayerHPImg.rectTransform.localScale.x);
            PlayerShieldImg.rectTransform.pivot = new Vector2(0.0f, 0.5f);
        }
        else
        {
            // (현재체력 + 쉴드)가 총체력보다 크면 오버된 양만큼 우측에 추가로 표현함
            PlayerShieldImg.rectTransform.pivot = new Vector2(1.0f, 0.5f);
            user_HP_Numerical.Shield_Pos = fPlayerImgSize / 2;
        }
        PlayerShieldImg.rectTransform.anchoredPosition = new Vector2(user_HP_Numerical.Shield_Pos, 0.0f);
    }

    /**
     * @brief 파티창 버프/디버프 활성화 (스킬 사용 등으로, 다른 유저가 체크할 수 있는 부분)
     * @param buffData 버프
     */
    public void ActiveBuff(BuffData buffData)
    {
        for (int i = 0; i < partybuffGroups.Length; i++)
        {
            for (int j = 0; j < partybuffGroups[i].userBuff.Length; j++)
            {
                // 이미 활성화중인 버프가 다시 들어온거라면 시간 리셋
                if (partybuffGroups[i].userBuff[j].isApply && partybuffGroups[i].userBuff[j].buffData.BuffKind == buffData.BuffKind)
                {
                    partybuffGroups[i].userBuff[j].duration = buffData.duration;
                    break;
                }
                // 버프가 새로 들어왔다면 활성화
                else if (!partybuffGroups[i].userBuff[j].isApply)
                {
                    // ActiveOwnBuff(Character.usingSkill.getBuffData);
                    partybuffGroups[i].userBuff[j].buffData = buffData;
                    partybuffGroups[i].userBuff[j].gameObject.SetActive(true);
                    partybuffGroups[i].userBuff[j].isApply = true;
                    break;
                }
            }
        }
    }

    /**
     * @brief 내 영향으로 받는 버프/디버프 활성화  (아이템 사용이나 보스한테 맞는 등으로, 다른 유저가 체크하지 못하는 부분)
     * @param buffData 버프
     */
    public void ActiveOwnBuff(BuffData buffData)
    {
        for (int j = 0; j < partybuffGroups[0].userBuff.Length; j++)
        {
            // 이미 활성화중인 버프가 다시 들어온거라면 시간 리셋
            if (partybuffGroups[0].userBuff[j].isApply && partybuffGroups[0].userBuff[j].buffData.BuffKind == buffData.BuffKind)
            {
                partybuffGroups[0].userBuff[j].duration = buffData.duration;
                break;
            }
            // 버프가 새로 들어왔다면 활성화
            else if (!partybuffGroups[0].userBuff[j].isApply)
            {
                partybuffGroups[0].userBuff[j].buffData = buffData;
                partybuffGroups[0].userBuff[j].gameObject.SetActive(true);
                partybuffGroups[0].userBuff[j].isApply = true;
                break;
            }
        }

        for (int i = 0; i < ownBuff.Length; i++)
        {
            if (ownBuff[i].isApply && ownBuff[i].buffData.BuffKind == buffData.BuffKind && buffData.check_nesting)
            {
                ownBuff[i].count++;
                ownBuff[i].duration = buffData.duration;
                break;
            }
            else if (ownBuff[i].isApply && ownBuff[i].buffData.BuffKind == buffData.BuffKind)
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
        // Net 에 보내는 buffData.buffKind 값이 idx로 인지하는게 틀림
        NetworkMng.I.SendMsg(string.Format("BUFF:0:{0}:{1}", buffData.BuffKind.ToString(), NetworkMng.I.uniqueNumber));
    }

    /**
     * @brief 파티창 버프 활성화(네트워크용)
     * @param player 플레이어 파티 index
     * @param buff 버프 idx (insepector 순서와 BUFF 순서가 같아야함)
     */
    public void partyActiveBuff(int player, string buff)
    {
        BuffData buffData = Resources.Load<BuffData>($"Buff/{buff}");
    
        for (int j = 0; j < partybuffGroups[player].userBuff.Length; j++)
        {
            // 이미 활성화중인 버프가 다시 들어온거라면 시간 리셋
            if (partybuffGroups[player].userBuff[j].isApply && partybuffGroups[player].userBuff[j].buffData.BuffKind.ToString() == buff)
            {
                partybuffGroups[player].userBuff[j].duration = buffData.duration;
                break;
            }
            // 버프가 새로 들어왔다면 활성화
            else if (!partybuffGroups[player].userBuff[j].isApply)
            {
                // ActiveOwnBuff(Character.usingSkill.getBuffData);
                partybuffGroups[player].userBuff[j].buffData = buffData;
                partybuffGroups[player].userBuff[j].gameObject.SetActive(true);
                partybuffGroups[player].userBuff[j].isApply = true;
                break;
            }
        }
    }

    public void forcedDeath()
    {
        user_HP_Numerical.Hp = 0;
        user_Shield_Numerical.Clear();
    }

    public void takeDamage(int dmg)
    {
        // 받는 피해 감소 적용
        dmg = Mathf.FloorToInt(dmg * Character._stat.takenDamagePer);

        int temp = -1;
        for (int j = 0; j < user_Shield_Numerical.Count; j++)
        {
            // 가장 먼저 들어온 쉴드 먼저 데미지를 받음
            user_Shield_Numerical[j].mount -= dmg;

            if (user_Shield_Numerical[j].mount <= 0)
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
        
        // 쉴드량 퍼센테이지 계산
        float shieldMount = 0;
        for (int j = 0; j < user_Shield_Numerical.Count; j++) {
            shieldMount += user_Shield_Numerical[j].mount;
        }
        shieldMount = shieldMount / (float)user_HP_Numerical.fullHp;
        if (shieldMount > 1)
            shieldMount = 1;
        user_HP_Numerical.shieldPer =  shieldMount;
        
        ShieldPos();
        PlayerHP();


        if (user_HP_Numerical.Hp <= 0)
        {
            // 사망
            GameMng.I.character._anim.SetTrigger("Die");
            GameMng.I._keyMode = KEY_MODE.UI_MODE;
            Instantiate(GameMng.I.soulPrefab, GameMng.I.character.transform.position, Quaternion.identity);
            GameMng.I.character.enabled = false;
            GameMng.I.character.transform.parent.GetComponent<Rigidbody>().useGravity = false;
            GameMng.I.character.transform.parent.GetComponent<BoxCollider>().enabled = false;

            // 파티방이라면
            if (NetworkMng.I.myRoom > ROOM_CODE._PARTY_MAP_) {
                bool isAllDie = true;
                for (int i = 0; i < 4; i++)
                {
                    if (Party_HP_Numerical[i].hpPer > 0)
                    {
                        isAllDie = false;
                        break;
                    }
                }
                // 모두 사망시 실패 UI
                if (isAllDie)
                    GameMng.I.dieUI.raidFail();
            }
            // 싱글룸
            else {
                // 마을로 가기 UI
                GameMng.I.dieUI.dungeonDie();
            }
        }

        // 네트워크에 내 변경된 HP 보내기 |  응답 메세지는 "PARTY_HP" 임
        NetworkMng.I.SendMsg(string.Format("CHANGE_HP:{0}:{1}", 
            user_HP_Numerical.Hp / (float)user_HP_Numerical.fullHp,
            shieldMount
        ));
    }

    public void removeAllDebuff()
    {
        NetworkMng.I.SendMsg(string.Format("BUFF:3:{0}", NetworkMng.I.uniqueNumber));
        for (int i = 0; i < partybuffGroups[0].userBuff.Length; i++)
        {
            // 디버프 종류만 모두 지움
            if (!partybuffGroups[0].userBuff[i].buffData.BuffKind.ToString().Substring(0, 4).Equals("BUFF"))
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
            NetworkMng.I.SendMsg(string.Format("BUFF:1:{0}:{1}", partybuffGroups[0].userBuff[ debuffIdxList[randIdx] ].buffData.BuffKind.ToString(), NetworkMng.I.uniqueNumber));
        }
    }

    public void partyRemoveBuff(int player, string buff)
    {    
        for (int j = 0; j < partybuffGroups[player].userBuff.Length; j++)
        {
            // 이미 활성화중인 버프가 다시 들어온거라면 시간 리셋
            if (partybuffGroups[player].userBuff[j].isApply && partybuffGroups[player].userBuff[j].buffData.BuffKind.ToString() == buff)
            {
                partybuffGroups[player].userBuff[j].isApply = false;
                break;
            }
        }
    }

    public void partyRemoveBuffAll(int player)
    {
        for (int i = 0; i < partybuffGroups[player].userBuff.Length; i++)
        {
            // 디버프 종류만 모두 지움
            if (!partybuffGroups[player].userBuff[i].buffData.BuffKind.ToString().Substring(0, 4).Equals("BUFF"))
                partybuffGroups[player].userBuff[i].isApply = false;
        }
    }

    public bool isDebuff(BUFF b)
    {
        return b.Equals(BUFF.DEBUFF_BUPAE) || b.Equals(BUFF.DEBUFF_CHIMSIK) || b.Equals(BUFF.DEBUFF_JAMSIK) || b.Equals(BUFF.DEBUFF_SHIELD);
    }
}