using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const float DISAPPEAR_TIME = .4f;
    const float MOVE_SPEED = .3f;

    [SerializeField]
    TMPro.TextMeshPro resourceText;
    
    private Color textColor;
    private float disappearTime = 0;
    private bool show = false;          // 보여졌는지, Set을 통해 보여져야함
    private bool isCritical = false;

    void Update()
    {
        if (!show)
            return;

        transform.position += new Vector3(0, MOVE_SPEED * 2) * Time.deltaTime;
        if (disappearTime > DISAPPEAR_TIME / 3)
        {
            if (!isCritical)    // 크리티컬 터지면
                transform.localScale += new Vector3(MOVE_SPEED * 2, MOVE_SPEED * 2, 0) * Time.deltaTime;
            else                // 크리티컬 안터지면
                transform.localScale += new Vector3(MOVE_SPEED * 8, MOVE_SPEED * 8, 0) * Time.deltaTime;
        }
        else
        {
            transform.localScale -= new Vector3(MOVE_SPEED, MOVE_SPEED, 0) * Time.deltaTime;
        }

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

    public void set(int dmg, bool isCritical)
    {
        resourceText.text = string.Format("{0:#,###}", dmg);
        textColor = resourceText.color;
        if (isCritical) {
            textColor = new Color32(255, 189, 0, 255);
            resourceText.color = textColor;
            this.isCritical = isCritical;
        }
        disappearTime = DISAPPEAR_TIME;
        transform.position += new Vector3(0, Random.Range(0, 1.5f) , 0);
        show = true;
    }
}
