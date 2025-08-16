using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No panel assigned to close.");
        }
    }
}
