using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    // 나중에 맵 변경 될때마다 해당 Text UI에 맵 이름 뜨게 함
    [SerializeField] UnityEngine.UI.Text mapNameTxt;        // !< GameObject가 관리해도 괜찮아 보임
    [SerializeField] string mapName = "";
    [SerializeField] bool isFocusingMap = true;

    void Start()
    {
        // 플레이어 생성
        GameMng.I.createMe();

        // 캐릭터에게 포커싱(카메라) 해야할 맵인지
        GameMng.I.isFocusing = isFocusingMap;

        // 로딩  끝

        // mapNameTxt.text = mapName;
    }
}
