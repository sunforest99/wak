using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidRecord : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI[] records;
    
    List<string> raid_record_0 = new List<string>();
    List<string> raid_record_1 = new List<string>();
    List<string> raid_record_2 = new List<string>();

    void Start()
    {
        // 기록이 이미 4개가 꽉 찼다면 더이상 갱신될 일이 없기때문에 더이상 새로 고침하지 않음
        if (raid_record_0.Count < 4) {
            for (int i = 0; i < 4; i++) {
                raid_record_0.Add("0 우왁굳, 뢴트게늄, 히키킹, 도파민\n클리어 날짜 : 2022-10-11");
                raid_record_1.Add("1 우왁굳, 뢴트게늄, 히키킹, 도파민\n클리어 날짜 : 2022-10-11");
                raid_record_2.Add("2 우왁굳, 뢴트게늄, 히키킹, 도파민\n클리어 날짜 : 2022-10-11");
            }
        }
    }

    public void selectRaid_0() {
        for (int i = 0; i < 4; i++) {
            records[i].text = raid_record_0[i];
        }
    }
    public void selectRaid_1() {
        for (int i = 0; i < 4; i++) {
            records[i].text = raid_record_1[i];
        }
    }
    public void selectRaid_2() {
        for (int i = 0; i < 4; i++) {
            records[i].text = raid_record_2[i];
        }
    }
}
