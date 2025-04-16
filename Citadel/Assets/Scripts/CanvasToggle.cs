using UnityEngine;

public class CanvasToggler : MonoBehaviour
{
    public GameObject targetCanvas;

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