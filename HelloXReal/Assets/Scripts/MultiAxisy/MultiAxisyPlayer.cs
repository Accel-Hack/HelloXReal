using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAxisyPlayer : MonoBehaviour
{
    [SerializeField] GameObject axisyPrefab;
    private SimpleAxisy[] axisies = null;
    private int animationFrameCount = 0;

    private VideoData videoData = null;

    private Vector3 firstPeopleCenter = Vector3.zero;
    private Vector3 peopleCenterWorld = Vector3.zero;

    private string waitingSequenceName = "";

    private enum Status {
        Uninitialized,  // Reject play button.
        NotPlaying,     // Accept play button.
        Playing
    }
    private Status status = Status.Uninitialized;

    // Initialize for video data.
    public void Initialize(VideoData videoData)
    {
        this.videoData = videoData;
        Vector3 firstPeopleCenter = this.videoData.GetFirstPeopleCenter();
        this.firstPeopleCenter = new Vector3(firstPeopleCenter.x, 0, firstPeopleCenter.y);

        if (this.axisies != null) {
            foreach (SimpleAxisy axisy in this.axisies) {
                Destroy(axisy.gameObject);
            }
        }
        this.axisies = new SimpleAxisy[videoData.GetMaxPersonNum()];

        for (int i = 0; i < this.axisies.Length; i++) {
            axisies[i] = Instantiate(axisyPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<SimpleAxisy>();
        }
        this.status = Status.NotPlaying;
        this.animationFrameCount = 0;
    }

    public void Play()
    {
        if (this.status == Status.NotPlaying) {
            this.status = Status.Playing;
        }
    }

    public void Stop()
    {
        if (this.status == Status.Playing) {
            this.status = Status.NotPlaying;
        }
    }

    public void FixedUpdate()
    {
        if (this.status == Status.Playing) {
            for (int i  = 0; i < this.axisies.Length; i++) {
                int deltaFrameCount = (int)(this.animationFrameCount * 30f * Time.fixedDeltaTime);
                if (this.videoData.FrameExists(deltaFrameCount)) {
                    List<Vector3> pose = this.videoData.GetPose(i, deltaFrameCount);
                    Vector3? centerInImage = this.videoData.GetCenterInImage(i, deltaFrameCount);
                    if (pose != null && centerInImage.HasValue) {
                        this.axisies[i].gameObject.SetActive(true);
                        this.axisies[i].Pose(pose);
                        Vector3 centerInWorld = new Vector3(
                            centerInImage.Value.x,
                            0,
                            centerInImage.Value.y
                        );
                        this.axisies[i].transform.position = centerInWorld - this.firstPeopleCenter + this.peopleCenterWorld;
                    } else {
                        this.axisies[i].gameObject.SetActive(false);
                    }
                } else {
                    // The video has finished.
                    this.status = Status.NotPlaying;
                    this.animationFrameCount = 0;
                    return;
                }
            }
            this.animationFrameCount++;
        }
    }

    private void Start()
    {
        SequenceLoaderMulti sequenceLoader = FindObjectOfType<SequenceLoaderMulti>();
        sequenceLoader.StartLoadingAction += this.Wait;
        sequenceLoader.EndLoadingAction += this.Activate;
    }

    public void Wait(string sequenceName)
    {
        this.waitingSequenceName = sequenceName;
    }

    public void Activate(LoadedDataMulti loadedData)
    {
        if (loadedData.sequenceName == this.waitingSequenceName)
        {
            this.Initialize(loadedData.videoData);
            this.waitingSequenceName = "";
        }
    }
}
