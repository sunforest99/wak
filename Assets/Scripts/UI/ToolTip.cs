using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip = null;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private UnityEngine.UI.Image tooltipImg;
    [SerializeField] Buff BuffSc;

    void OnMouseEnter()
    {
        tooltip.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
        tooltip.SetActive(true);
        toolTipSetting();
    }

    void OnMouseExit()
    {
        tooltip.SetActive(false);
    }

    void toolTipSetting()
    {
        tooltipImg.sprite = BuffSc.buffData.buffsprite;
        buffNameText.text = BuffSc.buffData.buffname;
        tooltipText.text = BuffSc.buffData.buffcontent;
    }


}
