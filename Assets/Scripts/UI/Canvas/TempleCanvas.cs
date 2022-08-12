using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempleCanvas : MonoBehaviour
{
    public void decideJob(int job)
    {
        GameMng.I.userData.job = job;
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
