using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailogUI : MonoBehaviour
{
    public RectTransform[] ui = new RectTransform[2];     // <! 0 npc, 1 플레이어
    [SerializeField] private TMPro.TextMeshProUGUI npcname_ui;
    [SerializeField] private TMPro.TextMeshProUGUI dialog_ui;
    [SerializeField] private TMPro.TextMeshProUGUI player_name_ui;
    [SerializeField] private TMPro.TextMeshProUGUI player_dialog_ui;

    public string setPlayerName
    {
        set { player_name_ui.text = value; }
    }
    public string setNpcName
    {
        set { npcname_ui.text = value; }
    }
    public string setPlayerText
    {
        set { player_dialog_ui.text = value; }
    }
    public string setNpcText
    {
        set { dialog_ui.text = value; }
    }

    public GameObject SelectBtn;
    public bool flow;       // <! 선택지 2개

    private void Awake()
    {
        GameMng.I.dailogUI = this;
    }

    public void Select1()
    {
        flow = false;
        GameMng.I.npcData.NextDialog();
        this.SelectBtn.SetActive(false);
    }
    public void Select2()
    {
        flow = true;
        GameMng.I.npcData.NextDialog();
        this.SelectBtn.SetActive(false);
    }
}
