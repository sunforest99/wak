using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "ScriptableObject/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] private int _startHp;          // <! 보스 총 체력
    public int startHp { get { return _startHp; } }

    [SerializeField] private float _radetime;       // <! 레이드 시간
    public float radetime { get { return _radetime; } }

    [SerializeField] private string _bossName;      // <! 보스이름
    public string bossName { get { return _bossName; } }

    [SerializeField] private float _moveSpeed;      // <! 보스 이동
    public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value;}}

    [SerializeField] private Color[] _barColor = new Color[6];      // <! 체력바 색
    public Color[] barColor { get { return _barColor; } }

    [SerializeField] private int _maxNesting;       // <! 체력바 중첩
    public int maxNesting { get { return _maxNesting; } }

    [SerializeField] private int _maxKnife;       // <! 칼 최대 개수
    public int maxKnife { get { return _maxKnife; } }
    [SerializeField] private int _maxCristal;       // <! 수정 최대 개수
    public int maxCristal { get { return _maxCristal; } }

    [SerializeField] private int _maxWave;       // <! 파도 최대 개수
    public int maxWave { get { return _maxWave; } }

    private Color color;

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#9B111E", out color);
        _barColor[0] = color;
        ColorUtility.TryParseHtmlString("#FF7F00", out color);
        _barColor[1] = color;
        ColorUtility.TryParseHtmlString("#FFD400", out color);
        _barColor[2] = color;
        ColorUtility.TryParseHtmlString("#9DD84B", out color);
        _barColor[3] = color;
        ColorUtility.TryParseHtmlString("#00A3D2", out color);
        _barColor[4] = color;
        ColorUtility.TryParseHtmlString("#8B00FF", out color);
        _barColor[5] = color;
    }
}
