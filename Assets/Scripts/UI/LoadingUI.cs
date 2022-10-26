using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI loadingMsg;
    
    public void changeLoadingMsg()
    {
        string[] msg = {
            "제니훈",
            "본인의 깨달음을 채팅창에 설파하지 마라",
            "나대지마",
            "욕시그",
            "실력이 없는 것도 실력이다",
            "혹내병",
            "유부녀 최고야",
            "나는 나다",
        };

        loadingMsg.text = $"\"{msg[ Random.Range(0, msg.Length) ]}\"";
    }
}
