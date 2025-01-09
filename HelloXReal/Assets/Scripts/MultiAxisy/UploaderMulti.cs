using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

// Upload video file to flask server.
public class UploaderMulti : MonoBehaviour
{
    private SelectSequenceDropDownMulti dropDown;
    private readonly WaitForFixedUpdate waitOneFrame = new WaitForFixedUpdate();

    private void Start()
    {
        this.dropDown = FindObjectOfType<SelectSequenceDropDownMulti>();
    }

    // Upload a file at filePath to flask server.
    public IEnumerator UploadFile(string filePath)
    {
        // filePath = Application.dataPath + "/" + filePath;
        string url = "http://192.168.50.110:8000/upload";
        byte[] fileData = File.ReadAllBytes(filePath); // Convert the file into byte sequence.
        TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open("Video Name", TouchScreenKeyboardType.NamePhonePad);
        while (keyboard.status != TouchScreenKeyboard.Status.Done) yield return this.waitOneFrame;
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, keyboard.text + ".mp4", "video/mp4");
        this.dropDown.RecordUploadingSequenceName(keyboard.text);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully.");
                this.dropDown.ReflectSequenceList(www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }
}
