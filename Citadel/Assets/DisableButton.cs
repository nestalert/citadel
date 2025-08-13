using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    public Button buttonToDisable;

    public void DisableButton()
    {
        if (buttonToDisable != null)
        {
            buttonToDisable.interactable = false;
        }
    }
}
