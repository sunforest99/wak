using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void destroySelf()
    {
        Destroy(this.gameObject);
    }
}
