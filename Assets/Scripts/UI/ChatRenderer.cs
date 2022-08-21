using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRenderer : MonoBehaviour
{
    public Renderer MyRenderer;

    private void Start() {
        MyRenderer.sortingLayerName = "UI";
        MyRenderer.sortingOrder = 1;
    }
}
