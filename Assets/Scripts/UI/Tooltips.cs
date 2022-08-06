using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltips : MonoBehaviour
{
    [SerializeField] protected GameObject tooltip = null;
    [SerializeField] protected TextMeshProUGUI tooltipText;
    [SerializeField] protected TextMeshProUGUI buffNameText;
    [SerializeField] protected UnityEngine.UI.Image tooltipImg;

    protected void toolTipSetting(Sprite buffimg, string buffname, string buffcontent)
    {
        tooltipImg.sprite = buffimg;
        buffNameText.text = buffname;
        tooltipText.text = buffcontent;
    }
}