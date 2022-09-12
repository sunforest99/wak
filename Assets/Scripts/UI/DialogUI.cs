using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    // public RectTransform[] ui = new RectTransform[2];     // <! 0 npc, 1 플레이어
    [SerializeField] private TMPro.TextMeshProUGUI npc_name_ui;
    [SerializeField] private TMPro.TextMeshProUGUI npc_chat_ui;
    // [SerializeField] private TMPro.TextMeshProUGUI player_name_ui;
    [SerializeField] private TMPro.TextMeshProUGUI player_chat_ui;
    public GameObject selectBlock;
    [SerializeField] private TMPro.TextMeshProUGUI select_1_text;
    [SerializeField] private TMPro.TextMeshProUGUI select_2_text;

    public bool flow;       // <! 선택지 2개

    public string setNpcName
    {
        set { npc_name_ui.text = value; }
    }
    public string setNpcText
    {
        set { npc_chat_ui.text = value; }
    }
    // public string setPlayerName
    // {
    //     set { player_name_ui.text = value; }
    // }
    public string setPlayerText
    {
        set { player_chat_ui.text = value; }
    }

    // private void Awake()
    // {
    //     GameMng.I.dailogUI = this;
    // }

    public void setSelectBlock(string s1, string s2)
    {
        this.select_1_text.text = s1;
        this.select_2_text.text = s2;
        this.selectBlock.SetActive(true);
        MCamera.I.setTargetChange(GameMng.I.character.transform);
    }

    public void Select1()
    {
        flow = true;
        this.selectBlock.SetActive(false);
        GameMng.I.npcData.NextDialog();
    }
    public void Select2()
    {
        flow = false;
        this.selectBlock.SetActive(false);
        GameMng.I.npcData.NextDialog();
    }
}
