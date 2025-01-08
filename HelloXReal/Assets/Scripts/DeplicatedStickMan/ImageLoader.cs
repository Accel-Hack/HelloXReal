using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ImageLoader : MonoBehaviour
{
    // Imageコンポーネントへの参照
    private Image image;

    // 画像のURL
    public string imageUrl = "https://example.com/yourimage.png";

    // 開始時に画像をロード
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(LoadImageFromUrl(imageUrl));
    }

    // Coroutineで画像をロード
    IEnumerator LoadImageFromUrl(string url)
    {
        // UnityWebRequestを使って画像を取得
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // ダウンロード開始
            yield return webRequest.SendWebRequest();

            // エラーチェック
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading image: " + webRequest.error);
            }
            else
            {
                // テクスチャを取得
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

                // テクスチャをSpriteに変換
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Imageコンポーネントに設定
                image.sprite = sprite;
            }
        }
    }
}
