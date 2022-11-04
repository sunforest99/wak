using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RUHuman : MonoBehaviour
{
    [SerializeField] Animator _anim;

    public void found()
    {
        _anim.SetTrigger("Found");
    }
}
