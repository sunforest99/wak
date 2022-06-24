using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const float DISAPPEAR_TIME = .5f;
    const float MOVE_SPEED = .7f;

    [SerializeField]
    TMPro.TextMeshPro resourceText;
    
    private Color textColor;
    private float disappearTime = 0;
    private bool show = false;          // 보여졌는지, Set을 통해 보여져야함

    void Update()
    {
        if (!show)
            return;

        transform.position += new Vector3(0, MOVE_SPEED) * Time.deltaTime;

        disappearTime -= Time.deltaTime;
        if (disappearTime < 0)
        {
            textColor.a -= 5 * Time.deltaTime;
            resourceText.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void set(int dmg)
    {
        resourceText.text = dmg.ToString();
        textColor = resourceText.color;
        disappearTime = DISAPPEAR_TIME;
        show = true;
    }
}
