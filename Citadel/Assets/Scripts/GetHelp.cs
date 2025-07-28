using UnityEngine;
using System.IO;

public class GetHelp : MonoBehaviour
{
    void Start()
    {
        
    }
    public void OpenWebsite()
    {
        string filePath = Application.streamingAssetsPath + "/webpage/help1.html";
        string url = "";

        url = "file:///" + filePath;

        Application.OpenURL(url);
    }
}
