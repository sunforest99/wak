using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_UIOpen : Build
{
    [SerializeField] GameObject UI;

    public override void activeBuild()
    {
        UI.SetActive(true);
    }
}
