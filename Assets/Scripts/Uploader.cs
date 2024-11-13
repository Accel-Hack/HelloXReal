using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using TMPro;

// Upload video file to flask server.
public class Uploader : MonoBehaviour
{
    [SerializeField] StickmanLoader stickmanLoader;
    [SerializeField] TextMeshProUGUI text;

    // Upload a file at filePath to flask server.
    public IEnumerator UploadFile(string filePath)
    {
        // filePath = Application.dataPath + "/" + filePath;
        string url = "http://192.168.50.110:8000/upload";
        byte[] fileData = File.ReadAllBytes(filePath); // Convert the file into byte sequence.
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, "video.mp4", "video/mp4");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully.");
                text.text = www.downloadHandler.text;
                stickmanLoader.SetAnimations(www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }
}
