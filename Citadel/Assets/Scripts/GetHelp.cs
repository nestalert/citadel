using UnityEngine;
using System.IO;

public class GetHelp : MonoBehaviour
{
    void Start()
    {
        
    }
    public void OpenWebsite()
    {
        string filePath = Application.streamingAssetsPath + "/webpage/RetroStuff.htm";
        string url = "";

        url = "file:///" + filePath;

        Application.OpenURL(url);
    }
}
