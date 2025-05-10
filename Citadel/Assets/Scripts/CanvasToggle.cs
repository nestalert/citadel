using UnityEngine;

public class CanvasToggler : MonoBehaviour
{
    public GameObject targetCanvas;

    void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(false); // Start hidden
        }
    }

    public void ToggleCanvas()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(!targetCanvas.activeSelf);
        }
        else
        {
            Debug.LogError("Target Canvas GameObject is not assigned!");
        }
    }
}
