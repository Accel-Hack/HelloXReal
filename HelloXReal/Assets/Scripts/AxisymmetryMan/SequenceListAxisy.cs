using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// SequenceList is a group of sequence-buttons.
public class SequenceListAxisy : MonoBehaviour
{
    private readonly Vector3 positionOfFirstButton = new Vector3(0, -10, 0);
    private readonly Vector3 delta = new Vector3(0, -15, 0);

    // List of sequence-button. Each button corresponds to a sequence-text file (output of mediapipe).
    private List<GameObject> buttons;

    [SerializeField] GameObject sequenceButtonPrefab;

    // JSON form list of each sequence-file's names.
    private string sequenceNameListJson;

    // Class to read json.
    [System.Serializable]
    private class SequenceNameList
    {
        public List<string> sequenceNameList;
    }

    private void Awake()
    {
        this.buttons = new List<GameObject>();
        StartCoroutine(this.InitializeSequenceList());
    }

    private IEnumerator InitializeSequenceList()
    {
        yield return this.LoadSequenceListJson();
        this.ReplaceButtons(this.sequenceNameListJson);
    }

    private IEnumerator LoadSequenceListJson()
    {
        string url = "http://192.168.50.110:8000/list_files";

        using (UnityWebRequest request = UnityWebRequest.Get(url))  // Create request.
        {
            // Send request and wait until download completed.
            yield return request.SendWebRequest();

            // Error check
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                this.sequenceNameListJson = null;
                yield break;
            }
            else
            {
                // Download suceed.
                this.sequenceNameListJson = request.downloadHandler.text;
            }
        }
    }

    public void ReplaceButtons(string sequenceNameListJson)
    {
        // Read json as a FileList.
        List<string> sequenceNameList = JsonUtility.FromJson<SequenceNameList>(sequenceNameListJson).sequenceNameList;

        // If there are already buttons, clean up them.
        foreach (GameObject button in this.buttons) {
            Destroy(button);
        }
        this.buttons.Clear();

        // Create buttons for each animations on the server.
        for (int i = 0; i < sequenceNameList.Count; i++) {
            GameObject button = Instantiate(
                sequenceButtonPrefab,
                transform.position,
                transform.rotation,
                transform
            );
            this.buttons.Add(button);
            button.GetComponent<SequenceButtonAxisy>().Initialize(sequenceNameList[i]);

            // Place button at a relative position from AnimationSelecter.
            // You can't set relative position with the 2nd argment of Instantiate()...
            button.transform.localPosition = this.positionOfFirstButton + this.delta * i;
        }
    }
}
