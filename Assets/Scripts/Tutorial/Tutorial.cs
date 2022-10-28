using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Tutorial : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] TimelineAsset[] timeline;

    [SerializeField] TMPro.TextMeshProUGUI l_head;
    [SerializeField] TMPro.TextMeshProUGUI l_body;
    [SerializeField] TMPro.TextMeshProUGUI r_head;
    [SerializeField] TMPro.TextMeshProUGUI r_body;

    [SerializeField] SpriteRenderer hair;
    [SerializeField] SpriteRenderer face;

    void Start()
    {
        Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("UI_Base"));

        face.sprite = Resources.Load<Sprite>($"Character/Face/{GameMng.I.userData.character.face}");
        hair.sprite = Resources.Load<Sprite>($"Character/Hair/{GameMng.I.userData.character.hair}");
        StartCoroutine(waiting());
    }

    public bool wakeup = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !wakeup) {
            Debug.Log("!");

            wakeup = true;
            director.Play(timeline[0]);     // 페이즈 2
            StopCoroutine(waiting());
            StartCoroutine(waking());
        }
    }

    IEnumerator waiting()
    {
        // 25초안에 스페이스바로 기상하면 안하면 자동으로 기상하게
        yield return new WaitForSeconds(25);
        
        if (!wakeup) {
            wakeup = true;
            director.Play(timeline[0]);     // 페이즈 2
            StartCoroutine(waking());
        }
    }
    IEnumerator waking()
    {
        yield return new WaitForSeconds(4);
        
        director.Play(timeline[1]);     // 페이즈 3

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
