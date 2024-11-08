using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryReader: MonoBehaviour
{
    [SerializeField] Uploader uploader;

    // Select and upload video from Android gallery.
    public void UploadVideo()
    {
        // Media selection is already on progress.
        if (NativeGallery.IsMediaPickerBusy()) {
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            StartCoroutine(uploader.UploadFile(path));
        }, "Select a video" );
        Debug.Log( "Permission result: " + permission );
    }
}