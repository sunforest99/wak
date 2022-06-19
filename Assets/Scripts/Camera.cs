using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject myCharacter;

    void Update()
    {
        transform.position = myCharacter.transform.position - new Vector3(0, 0, 10);
    }
}
