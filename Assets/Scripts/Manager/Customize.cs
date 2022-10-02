using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customize : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image hairImg;
    [SerializeField] UnityEngine.UI.Image faceImg;
    
    [SerializeField] Sprite[] hairs;
    [SerializeField] Sprite[] faces;

    [SerializeField] TMPro.TextMeshProUGUI hairTxt;
    [SerializeField] TMPro.TextMeshProUGUI faceTxt;

    int hairIdx;
    int faceIdx;

    public void hairPrev()
    {
        hairIdx--;
        if (hairIdx < 0)
            hairIdx = hairs.Length - 1;

        hairImg.sprite = hairs[hairIdx];
        hairTxt.text = "머리 " + hairIdx + 1;
    }
    public void hairNext()
    {
        hairIdx++;
        if (hairIdx >= hairs.Length)
            hairIdx = 0;
            
        hairImg.sprite = hairs[hairIdx];
        hairTxt.text = "머리 " + hairIdx + 1;
    }

    public void facePrev()
    {
        faceIdx--;
        if (faceIdx < 0)
            faceIdx = faces.Length - 1;

        faceImg.sprite = faces[faceIdx];
        faceTxt.text = "머리 " + faceIdx + 1;
    }
    public void faceNext()
    {
        faceIdx++;
        if (faceIdx >= faces.Length)
            faceIdx = 0;

        faceImg.sprite = faces[faceIdx];
        faceTxt.text = "머리 " + faceIdx + 1;
    }

    public void doneCustomize()
    {

    }
}
