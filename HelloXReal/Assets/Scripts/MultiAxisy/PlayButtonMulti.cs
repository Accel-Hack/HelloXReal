using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonMulti : MonoBehaviour
{
    [SerializeField] MultiAxisyPlayer player;
    private SequenceLoaderMulti sequenceLoader;
    private Button button;
    private string waitingSequenceName = "";

    private void Start()
    {
        this.sequenceLoader = FindObjectOfType<SequenceLoaderMulti>();
        this.button = GetComponent<Button>();
        this.sequenceLoader.StartLoadingAction += this.Wait;
        this.sequenceLoader.EndLoadingAction += this.Activate;
    }

    // Called by button OnClick event.
    public void OnClick()
    {
        this.player.Play();
    }

    // Called when the loading sequence starts.
    public void Wait(string sequenceName)
    {
        this.waitingSequenceName = sequenceName;
        this.button.interactable = false;
    }

    // Called when the loading sequence ends.
    public void Activate(LoadedDataMulti loadedData)
    {
        if (this.waitingSequenceName == loadedData.sequenceName)
        {
            this.button.interactable = true;
            this.waitingSequenceName = "";
        }
    }
}
