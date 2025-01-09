using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

// This is attached on DropDown GUI component which indicates a list of usable sequences.
public class SelectSequenceDropDownMulti : MonoBehaviour
{
    // Class to read json.
    [System.Serializable]
    private class SequenceNameList
    {
        public List<string> sequenceNameList;
    }

    private readonly string urlToLoadSequenceList = "http://192.168.50.110:8000/list_files";
    private enum Status {
        Initializing,
        Initialized,
        Failed
    }
    private Status status = Status.Initializing;

    private TMP_Dropdown dropdown;
    private MultiAxisyPlayer player;
    private SequenceLoaderMulti sequenceLoader;
    private PlayButtonMulti playButton;

    private string uploadingSequenceName = "";

    private void Start()
    {
        this.dropdown = GetComponent<TMP_Dropdown>();
        this.player = FindObjectOfType<MultiAxisyPlayer>();
        this.sequenceLoader = FindObjectOfType<SequenceLoaderMulti>();
        this.playButton = FindObjectOfType<PlayButtonMulti>();
        LoadSequenceList();
    }

    // This method is also called from Reload button.
    public void LoadSequenceList()
    {
        StartCoroutine(LoadSequenceListCoroutine());
    }

    private IEnumerator LoadSequenceListCoroutine()
    {
        this.status = Status.Initializing;
        using (UnityWebRequest request = UnityWebRequest.Get(this.urlToLoadSequenceList))  // Create request.
        {
            // Send request and wait until download completed.
            yield return request.SendWebRequest();

            // Error check
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                this.status = Status.Failed;
                yield break;
            }
            else
            {
                // Download suceed.
                string loadedString = request.downloadHandler.text;
                ReflectSequenceList(loadedString);
            }
        }
    }

    public void ReflectSequenceList(string sequenceListString)
    {
        // sequenceListString is JSON format.
        // Read json as a FileList instance.
        List<string> sequenceNameList = JsonUtility.FromJson<SequenceNameList>(sequenceListString).sequenceNameList;
        sequenceNameList.Insert(0, "Select Video");

        this.dropdown.ClearOptions();
        this.dropdown.AddOptions(sequenceNameList);
    }

    // Called by the eventhandler DropDown.OnValueChanged when a video is selected on the DropDown.
    public void SetSequence()
    {
        string sequenceName = this.dropdown.options[this.dropdown.value].text;
        if (sequenceName == "Select Video")
        {
            Debug.Log("Default value");
            this.playButton.Wait("");
        }
        else
        {
            this.sequenceLoader.LoadSequence(sequenceName);
            Debug.Log("sequenceName: " + sequenceName);
        }   
    }

    public void RecordUploadingSequenceName(string name)
    {
        this.uploadingSequenceName = name;
    }

    public bool IsUploading(string sequenceName)
    {
        return this.uploadingSequenceName == sequenceName;
    }
}
