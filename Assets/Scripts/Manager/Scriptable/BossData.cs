using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "ScriptableObject/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] private int _startHp;          // <! 보스 총 체력
    public int getStartHp { get { return _startHp; } }

    [SerializeField] private string _name;      // <! 보스이름
    public string getName { get { return _name; } }

    [SerializeField] private float _moveSpeed;      // <! 보스 이동
    public float getMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField] private int[] _damage;

    [SerializeField] private Dictionary<int, int> _bossDamage;

    [SerializeField] private float _radetime;       // <! 레이드 시간
    public float radetime { get { return _radetime; } }

    [SerializeField] private Color[] _barColor = new Color[6];      // <! 체력바 색
    public Color[] barColor { get { return _barColor; } }

    [SerializeField] private int _maxNesting;       // <! 체력바 중첩
    public int maxNesting { get { return _maxNesting; } }

    [SerializeField] private int _bossAction;
    public int bossAction { set { _bossAction = value; } get { return _bossAction; } }

    private Color color;

    [SerializeField] private BuffData[] _buffdata;
    public BuffData[] getBuffs { get { return _buffdata; } }

    public int getDamage() => _damage[_bossAction];
    public int getDamage(int action) => _damage[action];

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
