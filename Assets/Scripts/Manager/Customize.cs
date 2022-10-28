using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Customize : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image hairImg;
    [SerializeField] UnityEngine.UI.Image faceImg;
    
    [SerializeField] Sprite[] hairs;
    [SerializeField] Sprite[] faces;

    [SerializeField] TMPro.TextMeshProUGUI hairTxt;
    [SerializeField] TMPro.TextMeshProUGUI faceTxt;

    // string BASE_URL = "localhost:3000/";
    public delegate void responseFunc(string res);

    int hairIdx;
    int faceIdx;

    public void hairPrev()
    {
        hairIdx--;
        if (hairIdx < 0)
            hairIdx = hairs.Length - 1;

        hairImg.sprite = hairs[hairIdx];
        hairTxt.text = "머리 " + (hairIdx + 1);
    }
    public void hairNext()
    {
        hairIdx++;
        if (hairIdx >= hairs.Length)
            hairIdx = 0;
            
        hairImg.sprite = hairs[hairIdx];
        hairTxt.text = "머리 " + (hairIdx + 1);
    }

    public void facePrev()
    {
        faceIdx--;
        if (faceIdx < 0)
            faceIdx = faces.Length - 1;

        faceImg.sprite = faces[faceIdx];
        faceTxt.text = "머리 " + (faceIdx + 1);
    }
    public void faceNext()
    {
        faceIdx++;
        if (faceIdx >= faces.Length)
            faceIdx = 0;

        faceImg.sprite = faces[faceIdx];
        faceTxt.text = "머리 " + (faceIdx + 1);
    }

    public void doneCustomize()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_oid", GameMng.I.userData._id);
        form.AddField("hair", hairIdx);
        form.AddField("face", faceIdx);

        StartCoroutine(HttpPost(
            "users/customize",
            form,
            success: (msg) => {
                MessagePacket mp = JsonUtility.FromJson<MessagePacket>(msg);
                if (mp.success) {
                    GameMng.I.userData.character.hair = hairIdx;
                    GameMng.I.userData.character.face = faceIdx;

                    // 커마데이터 변경 성공
                    SceneManager.LoadScene("TutorialCine");
                } else {
                    // 실패
                    this.gameObject.SetActive(false);
                }
            },
            failure: (msg) => {
                // 실패
                this.gameObject.SetActive(false);
            }
        ));
    }

    IEnumerator HttpPost(string url, WWWForm form, responseFunc success, responseFunc failure)
    {
        UnityWebRequest req = new UnityWebRequest();

        using  (req = UnityWebRequest.Post(NetworkMng.DB_URL + url, form)) {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError) {
                failure(req.error);
            } else {
                success(req.downloadHandler.text);
            }
        }
    }
}
