using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class Uploader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator UploadFile(string filePath)
    {
        filePath = Application.dataPath + "/" + filePath;
        string url = "http://192.168.50.110:8000/upload";
        byte[] fileData = File.ReadAllBytes(filePath); // Convert the file into byte sequence.
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, "textfile.txt", "text/plain");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully.");
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }
}
