# Objective
本プロジェクトの機能を実装する方法を共有し、後続のエンジニアの開発の助けとする。  
機能ごとに章分けしている。

# NRSDK General
- NRealのSDK。AR機能を提供する。
- Unity Packageとして配布されており、プレハブやC#スクリプト、デモシーンなどのリソースが含まれる。
  - Getting Started with NRSDKに基づいてパッケージを展開したら、UnityEditorにてAssets > NRSDKに内容物が入る。
  - NRSDK > Demosにデモシーンがある。自身の目標に近いシーンを複製し、それを元に自身のシーンを組み立てるとよい。
- NRSDKのAPIは、NRKernal名前空間にある。
  - c#ソースコードの最初に、`using NRKernal;`と記述することで、使用できる。

# NRSDKへの入力
- AR空間では、Android本体か、ユーザーの手を入力として用いる。両方でレーザーポインタ入力が使え、手のみハンドジェスチャーが使える。

## レーザーポインタによる入力
- NRInputプレハブのインスタンスをシーンに置くことで使用できる。
  - プレハブは、Assets/NRSDK/Prefabsにある。ここのNRInputプレハブを、シーンにドラッグ&ドロップすることで、そのインスタンスを配置できる。
- NRInputゲームオブジェクト(プレハブを配置して作ったインスタンス)の、NRInputコンポーネントの、InputSourceTypeから、入力形式を指定する。
  - 入力形式は、Android本体による入力か、手による入力か。
  - 手による入力の場合、次節のNRHandの配置も必要になると思う(配置しなかった場合を未検証)。
- Android本体、または手先から、レーザーポインタが出る。  
- PCにおけるマウスポインタのような使用感であり、Android画面のタップ、またはOKのジェスチャー(pinchという)がクリックに相当する。  
- ボタンなどのGUIは、Unityの一般的な設定のみで(配置、イベント関数の登録のみで)、上記のクリックで動作する。  
- ポインタが当たっているゲームオブジェクトや、座標は、以下のように取り出せる。
  - 手の場合、NRPointerRaycasterは、LaserRaycasterゲームオブジェクトのコンポーネントとして存在する。このゲームオブジェクトは、NRHand_R/L > NRHandPointer_R/L > LaserRaycasterという親子関係にある。
```cs
// レーザーポインタ
NRPointerRaycaster pointer = ...;

// レーザーポインタがどこで何に当たったかの情報
RaycastResult result = pointer.FirstRaycastResult();

// ゲームオブジェクト
GameObject obj = result.gameObject;

// 座標
Vector3 position = result.worldPosition;

// PlaneDetector上に物を置くサンプル
GameObject prefab = ...;

if (obj != null && obj.GetComponent<NRTrackableBehaviour>() != null) {
    var behaviour = obj.GetComponent<NRTrackableBehaviour>();
    if (behaviour.Trackable.GetTrackableType() != TrackableType.TRACKABLE_PLANE)
    {
        return;
    }

    Instantiate(prefab, result.worldPosition, Quaternion.identity, behaviour.transform);
}
```

## ハンドジェスチャーによる入力
- 手の動きを反映するには、次の操作が必要である。
  - Assets/NRSDK/Prefabs/Hands/にある、NRHand_R/Lプレハブを、シーンにあるNRInput > Right/Leftの子オブジェクトとしてシーンに配置する。
  - NRInputゲームオブジェクトのNRInputコンポーネントのInputourceTypeをHandsに変更する。
- ハンドジェスチャーの使い方は、https://xreal.gitbook.io/nrsdk/development/hand-tracking  が参考になる。
  - ハンドジェスチャーの種類は列挙型で定義されており、その一覧も載っている。

- ハンドジェスチャーは、以下のように取得できる。
```cs
HandState handState = NRInput.Hands.GetHandState(HandEnum.LeftHand);
switch (handState.currentGesture) {
    case HandGesture.Pinch:
        ...
        break;
    case HandGesture.Point:
        ...
        break;
}
```

# NRSDKの録画
ARグラスに映っている画面を録画できる。  
UnityEditorからAssets/NRSDK/DemosにあるRGB Camera-Recordシーンを開き、VideoCaptureExampleゲームオブジェクトを、録画したいシーンにコピペする。このゲームオブジェクトは録画関連のUIであり、赤いボタンを押すと、ボタンが緑の間、録画が行われる。 

## 権限の追加
権限がない場合、赤いボタンを押した時にエラーメッセージが表示される。この場合、表示されている権限(android.permission.RECORD_AUDIOなど)をAndroidManifest.xmlに追加する必要がある。AndroidManifest.xmlを編集するには、UnityEditor画面上のEdit > Project Settingsを選択し、Player設定のCustom Main Manifestにチェックをつけ、その下に表示されているパスにあるAndroidManifest.xmlを編集する。例えば、manifest直下に、次を追加する。  
`<uses-permission android:name="android.permission.RECORD_AUDIO" />`  

## 録画ファイルの取得
`adb pull <path_to_file_in_android>`  で取得できる。  
録画ファイルは、`/sdcard/Android/data/com.<company_name>.<product_name>/files`  にある。  
company_nameとproduct_nameは、Project SettingsのPlayer設定から確認・編集できる。  

# ストリーミング
- m3u8ファイルによるストリーミングは、HISPlayerというunitypackageから行う。
  - m3u8は、一般的な動画ストリーミング用フォーマットである。
- HISPlayerは無料デモ版を使っている。動画ストリーミングを製品に組み込むなら、ライセンスに注意
- https://github.com/HISPlayer/Unity_Video_Player/releases/tag/v3.4.1
- ストリーミングツールには、NexPlayerというunitypackageもある。特段比較をしていないため、動画ストリーミングを製品に組み込むなら要検討
- リアルタイムのストリーミングには、Flutterというフレームワークがあるらしい

# 外部サーバからのリソース取得
普通、データやリソース(3Dモデルやアニメーション等)は、プロジェクトのAssetsディレクトリ内に、静的に保存しておく。まずはこちらを検討するべきである。  
ここでは、アプリケーションの実行時にサーバーからデータ・リソースを動的に取得する方法を共有する。  

UnityWebRequestクラスを用いて、ダウンロードする。ダウンロードは、非同期処理で行うことになる。非同期処理は、IEnumerator型の関数で行う。非同期処理が完了するまでの間も、他のイベント関数(StartやUpdate)は実行されるため、ダウンロードしたデータ・リソースに依存する処理は、適宜場合分けしてnull参照を回避すること。  
テキスト・画像・その他で、微妙に異なる。  

## テキスト
テキストデータは、以下のようにダウンロードする。  
```cs
sometype SomeMethod()
{
    string url = "http://192.168.50.110:8000/mediapipe.txt";
    // 非同期処理開始
    StartCoroutine(Download(url));
}

IEnumerator Download(string url)  // 非同期処理の内容
{
    using (UnityWebRequest request = UnityWebRequest.Get(url))  // リクエストを作る。
    {
        // リクエストを送り、結果が返ってくるまで待機する。
        // ダウンロード完了か、エラーになったら、先に進む。
        yield return request.SendWebRequest();

        // エラーチェック
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // ダウンロードした文字列は、request.downloadHandler.textから取得可能。
            string sequence = request.downloadHandler.text;
            ...
        }
    }
}
```

## 画像
画像データは、以下のようにダウンロードする。  
テクスチャとしてダウンロードされるが、GUIに貼り付けるなどのユースケースでは、スプライトに変換して用いる。
```cs
sometype SomeMethod()
{
    string url = "https://www.sanrio.co.jp/wp-content/uploads/2022/06/list-pompompurin.png"
    // 非同期処理開始
    StartCoroutine(Download(url));
}

IEnumerator Download(string url)  // 非同期処理の内容
{
    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url)) // リクエストを作る
    {
        // リクエストを送り、結果が返ってくるまで待機する。
        // ダウンロード完了か、エラーになったら、先に進む。
        yield return webRequest.SendWebRequest();

        // エラーチェック
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError("Error loading image: " + webRequest.error);
        }
        else
        {
            // テクスチャを取得
            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

            // テクスチャをSpriteに変換して使うことが多い。
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            ...
        }
    }
}
```

## その他
その他多くのリソース(少なくとも、prefabやfbxファイル(3Dモデルやアニメーション))は、直接ダウンロードできない(バイト列を解釈できない)。サーバー側が一度AssetBundleというパッケージにビルドしてサーバーにデプロイし、クライアント側はAssetBundleをダウンロードした後に解凍する必要がある。  
AssetBundleへのビルドは、[BuildAssetBundle](https://github.com/Accel-Hack/AssetBundleBuilder)というプロジェクトで行っている。ビルド方法は、そちらのドキュメントを参照してほしい。  
AssetBundleのダウンロードは、以下のように行う。  
```cs
sometype SomeMethod()
{
    string url = "http://192.168.50.110:8000/external";
    StartCoroutine(Download(url));        
}

IEnumerator Download(string url)  // 非同期処理の内容
{
    using (UnityWebRequest webrequest = UnityWebRequestAssetBundle.GetAssetBundle(url))  // リクエストを作る。
    {
        // リクエストを送り、結果が返ってくるまで待機する。
        // ダウンロード完了か、エラーになったら、先に進む。
        yield return webrequest.SendWebRequest();

        // エラーチェック
        if (webrequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + webrequest.error);
        }
        else
        {
            // ダウンロードしたAssetBundleを取り出す。解凍する。
            AssetBundle myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(webrequest);

            if (myLoadedAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                yield break;
            }

            // AssetBundleの中身を取り出す。
            // fbxファイルでもゲームオブジェクト扱い。
            var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model.prefab");
            ...
        }
      }
}
```

# GUI　of NReal
UnityビルトインのGUIをそのまま使用できる。Canvasの上にGUIを並べることになる。  
CanvasとGUIはともにゲームオブジェクトであり、Canvasが親、GUIが子になる。  

## Canvas
Unity Editorにて、GameObject > UI > Canvasで作成できる。GUIを配置する土台である。  
CanvasコンポーネントのRenderModeをWorldにし、NRSDKに入っているCameraSmoothFollowコンポーネントを取り付けると、滑らかにカメラを追従する。  
PlaneDetectorがシーンにある場合、Planeに埋まってしまったGUIにはインタラクトできない。判定がPlaneに吸われてしまう。

## Button
Unityビルトインのボタンがそのまま使える。  
Unity Editorにて、GameObject > UI > Button - TextMeshProで作成できる。ButtonコンポーネントのOnClick()に、メソッドを登録できる。ボタンがクリックされると、登録しておいたメソッドが実行される。  
NRealにおいては、スマホ操作時は、ポインタ合わせ+画面タップで、ハンドジェスチャー操作時は、ポインタ合わせ+OKジェスチャーで、クリックしたことになる。  

## Scroll View
Unityビルトインのスクロールビューがそのまま使える。  
Unity Editorにて、GameObject > UI > Scroll Viewで作成できる。  
Scroll Viewの子オブジェクトのViewportの子オブジェクトのContentが、スクロールされるオブジェクトである。これを、スクロールさせたい大きいGUIに置き換えれば良い。その後、Scroll ViewのScroll RectコンポーネントのContent変数を、置き換えたGUIに対応させる必要がある。  
スクロールバーの大きさは、Scrollbar Horizontal / Verticalの、RectTransformコンポーネントの、height / widthから調整できる。