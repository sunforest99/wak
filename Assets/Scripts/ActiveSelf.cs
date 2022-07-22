using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSelf : MonoBehaviour
{
    public void ActiveOnSelf()
    {
        this.gameObject.SetActive(true);
    }

    public void ActiveOffSelf()
    {
        this.gameObject.SetActive(false);
    }
}
