using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltips : MonoBehaviour
{
    [SerializeField] private GameObject tooltip = null;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private TextMeshProUGUI buffNameText;
    [SerializeField] private UnityEngine.UI.Image tooltipImg;
    [SerializeField] private StateMng StatMngSc;

    void OnMouseEnter() 
    {
        tooltip.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z);
        tooltip.SetActive(true);
        tooltipImgSetting(this.transform.name);
    }

    void OnMouseExit() 
    {
        tooltip.SetActive(false);
    }

    void tooltipImgSetting(string name){
        string[] split = name.Split('_');
        
        if(split[0] == "Buff"){
            tooltipImg.sprite = StatMngSc.BuffSp[int.Parse(split[1])];
            switch(split[1]){
                case "0" : tooltipSetting("적중된 적에게 N초동안 ‘꾸짖음’을 남깁니다.\n‘꾸짖음’이 남은 적은 치명타 적중률이\n10% 상승된 상태로 피해를 입습니다.", "갈"); break; 
                case "1" : tooltipSetting("주님, 오늘도 정의로운 도둑이 되는 걸 허락해주세요.\n파티원 모두에게 자신의 체력 (10%/20%/30%)에 해당하는\n보호막을 (2초/3초/4초) 동안 생성합니다. ", "정의로운 도둑"); break;      
            }
        }
        else if(split[0] == "Debuff"){
            tooltipImg.sprite = StatMngSc.DeBuffSp[int.Parse(split[1])];
            switch(split[1]){
                case "0" : tooltipSetting("부패 시킨다.", "부패"); break;
                case "1" : tooltipSetting("침식 시킨다.", "침식"); break;
                case "2" : tooltipSetting("잠식 시킨다.", "잠식"); break;
                case "3" : tooltipSetting("쉴드를 부순다.", "쉴드 파괴"); break;
            }
        }
    }
    void tooltipSetting(string text, string name){
        tooltipText.text = text;
        buffNameText.text = name;
    }
}
