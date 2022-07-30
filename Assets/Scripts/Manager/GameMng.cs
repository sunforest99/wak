using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public RaycastHit2D hit;

    public GameObject dialogPrefab;
    public Transform uiCanvas;
    [SerializeField]
    GameObject pingPrefab;

    public DailogUI dailogUI;
    public Npcdata npcData;
    public int charactorDmg;
    private float npcDistance = 3.0f;       // <! npc와의 최대 거리
    
    private static GameMng _instance = null;
    
    public List<Transform> targetList;

    public Character character = null;
    public int gatDamage() => character.usingSkill.CalcSkillDamage();

    public int targetCount => Random.Range(0, targetList.Count);
    
    public static GameMng I
    {
        get
        {
            if (_instance.Equals(null))
            {
                Debug.LogError("Instance is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);        // <! 필요하면 쓰장
    }

    public void mouseRaycast(Vector2 charPos)      // <! 이름바꾸기
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D ray = new Ray2D(pos, Vector2.zero);

        hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null && hit.collider.CompareTag("Npc") && Vector2.Distance(hit.collider.transform.localPosition, charPos) <= npcDistance)
        {
            npcData = hit.collider.gameObject.GetComponent<Npcdata>();

            if (!npcData.isDailog && npcData != null)
            {
                npcData.isDailog = true;

                GameObject temp = Instantiate(GameMng.I.dialogPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                npcData.tempDialog = temp;
                temp.transform.parent = uiCanvas;
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void createPing(Vector2 pos)
    {
        Instantiate(pingPrefab, pos += new Vector2(0, 0.65f), Quaternion.identity);
    }
}
