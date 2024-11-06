using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GallaryReader: MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Update()
    {
        // タッチ時に呼ばれる
        if (Input.GetMouseButtonDown(0))
        {
            // 別のメディア選択操作がすでに進行中の場合
            if (NativeGallery.IsMediaPickerBusy())
                return;

            // 動画の読み込みと再生
            PickVideo();
        }
    }

    // 動画の読み込みと再生
    private void PickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // 動画の再生
                videoPlayer.url = path;
                videoPlayer.Play();
            }
        }, "Select a video" );
        Debug.Log( "Permission result: " + permission );
    }
}