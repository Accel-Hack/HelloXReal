using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main routine of multi axisy demo.

public class YoloOnlyMain : MonoBehaviour
{
    private const float WORLD_WIDTH = 100.0f;
    private readonly Vector3 worldCenter = new Vector3(0, 0, 0);

    [SerializeField] GameObject piecePrefab;
    private GameObject[] pieces;
    private int animationFrameCount = 0;
    private YoloVideoData videoData = null;

    void Start()
    {
        TextAsset textAsset = Resources.Load("sequence") as TextAsset;
        byte[] bytes = textAsset.bytes;

        this.videoData = new YoloVideoData(bytes);

        this.pieces = new GameObject[videoData.GetMaxPersonNum()];
        for (int i = 0; i < this.pieces.Length; i++) {
            this.pieces[i] = Instantiate(piecePrefab);
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < this.pieces.Length; i++) {
            int deltaFrameCount = (int)(this.animationFrameCount * 60f * Time.fixedDeltaTime);
            Vector3? centerInImage = this.videoData.GetCenterInImage(i, deltaFrameCount);
            (int, int) videoSize = this.videoData.GetVideoSize();
            if (centerInImage.HasValue) {
                this.pieces[i].gameObject.SetActive(true);
                Vector3 positionInWorld = new Vector3(
                    centerInImage.Value.x - videoSize.Item1 / 2,
                    0,
                    videoSize.Item2 / 2 - centerInImage.Value.y
                ) * WORLD_WIDTH / videoSize.Item1
                + worldCenter;
                this.pieces[i].transform.position = positionInWorld;
            } else {
                this.pieces[i].gameObject.SetActive(false);
            }
        }
        this.animationFrameCount++;
    }
}
