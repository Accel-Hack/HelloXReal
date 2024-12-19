using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main routine of multi axisy demo.

public class MultiSequenceMain : MonoBehaviour
{
    [SerializeField] GameObject axisyPrefab;
    private SimpleAxisy[] axisies;
    private int animationFrameCount = 0;

    private VideoData videoData = null;

    private Vector3 firstPeopleCenter = Vector3.zero;
    private readonly Vector3 peopleCenterWorld = new Vector3(0, 0, 5);

    void Start()
    {
        TextAsset textAsset = Resources.Load("sequence") as TextAsset;
        byte[] bytes = textAsset.bytes;

        this.videoData = new VideoData(bytes);

        Vector3 firstPeopleCenter = this.videoData.GetFirstPeopleCenter();
        this.firstPeopleCenter = new Vector3(firstPeopleCenter.x, 0, firstPeopleCenter.y);

        this.axisies = new SimpleAxisy[videoData.GetMaxPersonNum()];

        for (int i = 0; i < this.axisies.Length; i++) {
            axisies[i] = Instantiate(axisyPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<SimpleAxisy>();
        }
    }

    void FixedUpdate()
    {
        for (int i  = 0; i < this.axisies.Length; i++) {
            int deltaFrameCount = (int)(this.animationFrameCount * 30f * Time.fixedDeltaTime);
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
        }
        this.animationFrameCount++;
    }
}
