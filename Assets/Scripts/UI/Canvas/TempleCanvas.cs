using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TempleCanvas : MonoBehaviour
{
    // [SerializeField] GameObject normalPerson;
    // string BASE_URL = "localhost:3000/";
    public delegate void responseFunc(string res);
    int tempJob = 0;

    [SerializeField] GameObject book;

    public void selectJob(int job)
    {
        tempJob = job;
    }

    public void deicdeJob()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_oid", GameMng.I.userData._id);
        form.AddField("job", tempJob);

        StartCoroutine(HttpPost(
            "users/job",
            form,
            success: (msg) => {
                MessagePacket mp = JsonUtility.FromJson<MessagePacket>(msg);
                if (mp.success) {
                    // 직업 변경 성공
                    GameMng.I.userData.job = tempJob;
                    book.SetActive(false);
                } else {
                    // 실패
                    
                }
            },
            failure: (msg) => {
                // 실패
                
            }
        ));
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("SampleScene");
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
