using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeCells : MonoBehaviour
{
    private Vector2 resolution;

    void Awake()
    {
        resolution = new Vector2(Screen.width, Screen.height);
    }

    void FixedUpdate()
    {
        if (gameObject.activeInHierarchy && (resolution.x != Screen.width || resolution.y != Screen.height))
            Resize();
    }

    void Resize()
    {
        float width = GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / 2.6f, width / 2.6f);
        GetComponent<GridLayoutGroup>().cellSize = newSize;

        resolution.x = Screen.width;
        resolution.y = Screen.height;
    }
}
