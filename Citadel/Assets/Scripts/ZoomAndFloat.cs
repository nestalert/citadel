using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAndFloat : MonoBehaviour
{
    public float zoomSpeed = 1f;        // How fast it zooms
    public float zoomAmount = 0.05f;    // How much it zooms

    public float floatSpeed = 1f;       // How fast it floats
    public float floatAmount = 0.05f;   // How high it floats

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition; // for UI / 2D
    }

    void Update()
    {
        // Zoom in/out
        float scaleOffset = Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        transform.localScale = originalScale + Vector3.one * scaleOffset;

        // Float up/down
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = originalPosition + new Vector3(0f, floatOffset, 0f);
    }
}
