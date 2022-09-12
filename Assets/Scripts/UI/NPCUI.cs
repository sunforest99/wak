using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    public DialogUI dialogUI;
    public GameObject npcSelectUI;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void showDialog()
    {
        npcSelectUI.SetActive(false);
        dialogUI.gameObject.SetActive(true);
        GameMng.I.npcData.NextDialog();
    }
}
