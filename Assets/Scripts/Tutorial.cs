using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI l_head;
    [SerializeField] TMPro.TextMeshProUGUI l_body;
    [SerializeField] TMPro.TextMeshProUGUI r_head;
    [SerializeField] TMPro.TextMeshProUGUI r_body;

    void Start()
    {
        StartCoroutine(showCredit());
    }

    IEnumerator showCredit()
    {
        l_head.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        r_head.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        l_head.text = "< 기획 >";
        l_body.text = "청달이, 호라용이";
        l_head.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        r_head.text = "< 디자인 >";
        r_body.text = "노창, 눈딸기, 밤바다, 좋냥이";
        r_head.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        l_head.text = "< 개발 >";
        l_body.text = "고롱스, sun";
        l_head.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);

        r_head.text = "< 사운드 >";
        r_body.text = "티콘";
        r_head.gameObject.SetActive(true);
    }
}
