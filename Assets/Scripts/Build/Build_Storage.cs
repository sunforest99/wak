using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Storage : Build
{
    [SerializeField] GameObject inventoryUI;

    public override void activeBuild()
    {
        inventoryUI.SetActive(true);
    }
}
