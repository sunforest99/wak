using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    // 나중에 맵 변경 될때마다 해당 Text UI에 맵 이름 뜨게 함
    [SerializeField] UnityEngine.UI.Text mapNameTxt;        // !< GameObject가 관리해도 괜찮아 보임
    [SerializeField] string mapName = "";


    void Start()
    {
        // 플레이어 생성
        GameMng.I.createPlayer();

        // 로딩  끝

        // mapNameTxt.text = mapName;
    }
}
