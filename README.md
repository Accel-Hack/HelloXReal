# Objective
AR glassのプロジェクトが本格化した時に、エンジニアが素早く開発を開始できるようにする。  

# General Information
本プロジェクトで用いている、一般的な技術について述べる。  
本プロジェクト固有の構成や実装は、次章で述べる。

## Setup
### Install
[Getting Started with NRSDK]に従う

- [NRSDK](https://developer.xreal.com/download)
- [Unity Hub](https://unity.com/ja/download)
- [Android Studio](https://developer.android.com/studio?hl=ja) (option)
  - ほしいのはAndroid SDK
  - UnityからもAndroid SDKをインストールできたのでいらないかも
- 注意
- Getting Started with NRSDKの記述に　加えて/反して　、以下の設定をすると動作した
  - Project Settings > Player > Other Settings > Minimum API Level をAndroid 10.0にする
  - Project Settings > Player > Other Settings > Target API Level をAndroid 13.0にする
    - Android 14.0からの動作の変更が原因と考えられる。https://developer.android.com/about/versions/14/behavior-changes-14?hl=ja#safer-dynamic-code-loading
  - Project Settings > Player > Other Settings > Allow 'unsafe' Code をtrueにする

[Getting Started with NRSDK]: https://xreal.gitbook.io/nrsdk/nrsdk-fundamentals/quickstart-for-android#configure-adapted-devices-optional

### Creating a Unity Project
- Unity HubでPersonal Licenseを取得する

### NOTE:

- Unityは過去 12 か月の収益や調達した資金が 10 万ドルを超えるとEnterprise版にする必要がある

> Unity の無料版で今すぐ制作を始めましょう。利用資格：過去 12 か月の収益や調達した資金が 10 万ドル未満の個人開発者および小規模企業のお客様は、Unity Personal をご利用いただけます。
> https://unity.com/ja/products/unity-personal

- Unityでbuildしたapk形式のアプリケーションはAndroid SDKに入っているadb cliでAndroidにインストールできる

> https://developer.android.com/tools/adb?hl=ja

- Sumsung Galaxy 23S側で、Sumsung dexをオフにしないとXRealがいい感じに検出されなかった

## Android
### ペアリング
PCとAndroidをペアリングする。1度ペアリングをすれば、以降は接続から始められる。
- Androidで、設定 > 開発者向けオプション > ワイヤレスデバッグ > ペア設定コードによるデバイスのペア設定
  - 画面に、IPアドレス、ポート番号、ペア設定コードが表示される
- PCで、`adb pair <ip address>:<port num>`  を実行
- ペア設定コード(6桁)を入力

### 接続
- Androidで、設定 > 開発者向けオプション > ワイヤレスデバッグ
  - 画面に、IPアドレスとポート　という項目がある
  - ペアリング時とはポート番号が異なる
- PCで、`adb connect <ip address>:<port num>`  を実行

- PCで、`adb devices`  を実行すると、接続中のデバイスを確認できる
- 意図せず複数のデバイスが接続されている場合、次にエラーが出るため、`adb disconnect`  で切断し、接続をやり直す

### インストール
- adb経由でアプリケーション(拡張子apk)をインストールできる
- `adb install <path to apk file>`
- `-r -d`  オプションで上書きインストールができる...ようだが、このオプションをつけなくても上書きされている

## NRSDK
- NRealのSDK。AR機能を提供する。
- Unity Packageとして配布されており、プレハブやC#スクリプト、デモシーンなどのリソースが含まれる。
  - Getting Started with NRSDKに基づいてパッケージを展開したら、UnityEditorにてAssets > NRSDKに内容物が入る。
  - NRSDK > Demosにデモシーンがある。自身の目標に近いシーンを複製し、それを元に自身のシーンを組み立てるとよい。
- NRSDKのAPIは、NRKernal名前空間にある。
  - c#ソースコードの最初に、`using NRKernal;`と記述することで、使用できる。

### 入力
- AR空間では、Android本体か、ユーザーの手を入力として用いる。両方でレーザーポインタ入力が使え、手のみハンドジェスチャーが使える。

#### レーザーポインタによる入力
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

#### ハンドジェスチャーによる入力
- 手の動きを反映するには、次の操作が必要である。
  - Assets/NRSDK/Prefabs/Hands/にある、NRHand_R/Lプレハブを、シーンにあるNRInput > Right/Leftの子オブジェクトとしてシーンに配置する。
  - NRInputゲームオブジェクトのNRInputコンポーネントのInputourceTypeをHandsに変更する。
- ハンドジェスチャーの使い方は、https://xreal.gitbook.io/nrsdk/development/hand-tracking  が参考になる

## ストリーミングについて
- m3u8ファイルによるストリーミングは、HISPlayerというunitypackageから行う。
  - m3u8は、一般的な動画ストリーミング用フォーマットである。
- HISPlayerは無料デモ版を使っている。動画ストリーミングを製品に組み込むなら、ライセンスに注意
- https://github.com/HISPlayer/Unity_Video_Player/releases/tag/v3.4.1
- ストリーミングツールには、NexPlayerというunitypackageもある。特段比較をしていないため、動画ストリーミングを製品に組み込むなら要検討
- リアルタイムのストリーミングには、Flutterというフレームワークがあるらしい

## 外部サーバからのリソース取得方法
普通、データやリソース(3Dモデルやアニメーション等)は、プロジェクトのAssetsディレクトリ内に、静的に保存しておく。まずはこちらを検討するべきである。  
ここでは、アプリケーションの実行時にサーバーからデータ・リソースを動的に取得する方法を共有する。  

UnityWebRequestクラスを用いて、ダウンロードする。ダウンロードは、非同期処理で行うことになる。非同期処理は、IEnumerator型の関数で行う。非同期処理が完了するまでの間も、他のイベント関数(StartやUpdate)は実行されるため、ダウンロードしたデータ・リソースに依存する処理は、適宜場合分けしてnull参照を回避すること。  
テキスト・画像・その他で、微妙に異なる。  

### テキスト
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

### 画像
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

### その他
その他多くのリソース(少なくとも、prefabやfbxファイル(3Dモデルやアニメーション))は、直接ダウンロードできない(バイト列を解釈できない)。サーバー側が一度AssetBundleというパッケージにビルドしてサーバーにデプロイし、クライアント側はAssetBundleをダウンロードした後に解凍する必要がある。  
私は、AssetBundleへのビルドは、[BuildAssetBundle](https://github.com/Accel-Hack/AssetBundleBuilder)というプロジェクトで行っている。ビルド方法は、そちらのドキュメントを参照してほしい。  
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

## 外部サーバへのアップロード
Unityアプリケーションからhttpサーバにファイルをアップロードできる。  
サーバは、ファイルのポストを受け取り、保存するようになっているものとする。 

WWWFormクラスが、フォームの内容を格納する。  
UnityWebRequestクラスが、サーバに送信するリクエストとなる。  

一連の処理は、やはり非同期処理(コルーチン)で行う。  

テキストファイルのサンプルコード
```cs
sometype SomeMethod()
{
    StartCoroutine(UploadFile("http://url/to/server/upload", "/path/to/file"));
}

public IEnumerator UploadFile(string url, string filePath)
{
    // Path from Unity application.
    filePath = Application.dataPath + "/" + filePath;

    // Convert the file into byte sequence.
    byte[] fileData = File.ReadAllBytes(filePath);

    WWWForm form = new WWWForm();
    form.AddBinaryData("file", fileData, "textfile.txt", "text/plain");

    using (UnityWebRequest www = UnityWebRequest.Post(url, form))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("File uploaded successfully.");
        }
        else
        {
            Debug.LogError("Error uploading file: " + www.error);
        }
    }
}
```

## GUI　of NReal
~~UnityビルトインのGUIと同様に、Canvasの上にGUIを並べる仕組みである。~~
UnityビルトインのGUIをそのまま使用できる。Canvasの上にGUIを並べることになる。  
CanvasとGUIはともにゲームオブジェクトであり、Canvasが親、GUIが子になる。  

### Canvas
Unity Editorにて、GameObject > UI > Canvasで作成できる。GUIを配置する土台である。  
CanvasコンポーネントのRenderModeをWorldにし、NRSDKに入っているCameraSmoothFollowコンポーネントを取り付けると、滑らかにカメラを追従する。  
PlaneDetectorがシーンにある場合、Planeに埋まってしまったGUIにはインタラクトできない。判定がPlaneに吸われてしまう。

### Button
Unityビルトインのボタンがそのまま使える。  
Unity Editorにて、GameObject > UI > Button - TextMeshProで作成できる。ButtonコンポーネントのOnClick()に、メソッドを登録できる。ボタンがクリックされると、登録しておいたメソッドが実行される。  
NRealにおいては、スマホ操作時は、ポインタ合わせ+画面タップで、ハンドジェスチャー操作時は、ポインタ合わせ+OKジェスチャーで、クリックしたことになる。  

### Scroll View
Unityビルトインのスクロールビューがそのまま使える。  
Unity Editorにて、GameObject > UI > Scroll Viewで作成できる。  
Scroll Viewの子オブジェクトのViewportの子オブジェクトのContentが、スクロールされるオブジェクトである。これを、スクロールさせたい大きいGUIに置き換えれば良い。その後、Scroll ViewのScroll RectコンポーネントのContent変数を、置き換えたGUIに対応させる必要がある。  
スクロールバーの大きさは、Scrollbar Horizontal / Verticalの、RectTransformコンポーネントの、height / widthから調整できる。

## Android Gallary
Unity製Androidアプリケーションから、Androidのギャラリーにアクセスし、動画を取り出す。  
[ここ](https://note.com/npaka/n/nc9fcedf31b33)の説明で十分だと思う。  
ライセンスはUnityさえ満たしていれば大丈夫そう。  

# Project Information
本プロジェクトの構成や実装について述べる。  

## シーンの説明
- Unityでは、ゲーム空間を作ることができ、それぞれの空間をシーンとして保存・再生できる。
- シーンは、エディタ画面のAssets > Scenesから切り替えられる。
- Flowerシーンでは、ハンドジェスチャーで植物を配置できる。
- IndicateControllerシーンでは、入力が画面に表示される。
- HISシーンでは、動画のストリーミング再生を行っている。
- DynamicModelシーンでは、サーバから3Dモデルをダウンロードし、空間に配置する。
- Stickmanシーンでは、MediaPipeが出力する関節座標群を受け取り、シーン中の棒人間の姿勢に反映する。
  - 関節座標群は、テキストファイルとしてサーバに置かれているものをダウンロードする。
- GUIStickmanシーンでは、GUIによって、棒人間のアニメーションを操作できる。

## Stickmanについて
- MediaPipeは、人間が写った写真や動画から、関節(特徴点)群の三次元座標を予測することができる。
- MediaPipeのPythonコードから、文字列として、関節座標群を受け取るとして、その姿勢を反映した棒人間を生成している。
- 各関節をグラフ構造のノードとして、骨格を形成している。
- StickmanNodeは、関節=ノードを定義している。
- MediapipeReceiverは、MediaPipeの出力を文字列として受け取り、List<\List<\Vector3>>を作っている。
  - 内側のリストは、ある時刻=フレームの、それぞれの関節の座標群
  - 外側のリストは、フレーム群
- StickmanCreaterは、MediaPipeReceiverの出力を元に、StickmanNodeを生成し、組み合わせて、棒人間を作っている。

## シーン: GUIStickmanについて
- StickmanLoaderが、サーバにリクエストを送り、対応する関節座標データを受け取っている。
- サーバはFlaskで実装しており、別プロジェクトにある。