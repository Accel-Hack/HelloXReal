using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class UploadButtonAxisy : MonoBehaviour
{
    [SerializeField] AxisymmetryMan axisy;

    // Called with OnClick event of UploadButton.
    public void OnClick()
    {
        // Indicate screen to let user select movie to upload from Android local disc.

        // Media selection is already on progress.
        if (NativeGallery.IsMediaPickerBusy()) {
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            StartCoroutine(this.UploadFile(path));
        }, "Select a video" );
        Debug.Log( "Permission result: " + permission );
    }

    // Upload a file at filePath to flask server.
    private IEnumerator UploadFile(string filePath)
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
                // stickmanLoader.SetAnimations(www.downloadHandler.text);
                // TODO: After refactor scroll view.
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }
}
