using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;

public class Npcdata : MonoBehaviour
{
    [SerializeField] private int npcid;
    [SerializeField] private string npcname;
    [SerializeField] private int dialog_count;

    [SerializeField] private GameObject[] ui = new GameObject[2];
    [SerializeField] TMPro.TextMeshProUGUI npcname_ui;
    [SerializeField] TMPro.TextMeshProUGUI dialog_ui;
    [SerializeField] TMPro.TextMeshProUGUI player_dialog_ui;

    [SerializeField] List<string> playerdialog = new List<string>();
    [SerializeField] Dictionary<int, string> dialog = new Dictionary<int, string>();

    void Start()
    {
        LoadJsonData();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (dialog_count < dialog.Count)
        //     {
        //         npcname_ui.text = npcname;
        //         if (dialog[dialog_count].FirstOrDefault() == '$')
        //         {
        //             ui[1].SetActive(true);
        //             ui[0].SetActive(false);
        //             player_dialog_ui.text = dialog[dialog_count].Remove(0, 1);
        //         }
        //         else
        //         {
        //             ui[1].SetActive(false);
        //             ui[0].SetActive(true);
        //             dialog_ui.text = dialog[dialog_count];
        //         }
        //         dialog_count++;
        //     }
        //     else
        //     {
        //         for(int i = 0; i < ui.Length; i++)
        //         {
        //             ui[i].SetActive(false);
        //         }
        //         dialog_count = 0;
        //     }
        // }
    }

    void LoadJsonData()
    {
        try
        {
            object jsonStr = Resources.Load("Npc_data");
            JsonData jsondata = JsonMapper.ToObject(jsonStr.ToString());

            int temp = 0;
            for (int i = 0; i < jsondata.Count; i++)
            {
                if (int.Parse(jsondata[i]["npc_id"].ToString()) == npcid)
                {
                    npcname = jsondata[i]["npc_name"].ToString();
                    temp = 0;
                    foreach (var str in jsondata[i]["dialog"])
                    {
                        dialog.Add(temp, str.ToString());
                        temp++;
                    }
                }
            }
        }
        catch
        {
            Debug.LogError("Json Data load fail");
        }
    }
}
