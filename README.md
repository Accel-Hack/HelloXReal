# Setup

## Install
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

## Creating a Unity Project

- Unity HubでPersonal Licenseを取得する

## NOTE:

- Unityは過去 12 か月の収益や調達した資金が 10 万ドルを超えるとEnterprise版にする必要がある

> Unity の無料版で今すぐ制作を始めましょう。利用資格：過去 12 か月の収益や調達した資金が 10 万ドル未満の個人開発者および小規模企業のお客様は、Unity Personal をご利用いただけます。
> https://unity.com/ja/products/unity-personal

- Unityでbuildしたapk形式のアプリケーションはAndroid SDKに入っているadb cliでAndroidにインストールできる

> https://developer.android.com/tools/adb?hl=ja

- Sumsung Galaxy 23S側で、Sumsung dexをオフにしないとXRealがいい感じに検出されなかった

# シーンの説明
- Unityでは、ゲーム空間を作ることができ、それぞれの空間をシーンとして保存・再生できる。
- シーンは、エディタ画面のAssets > Scenesから切り替えられる。
- Flowerシーンでは、ハンドジェスチャーで植物を配置できる。
- IndicateControllerシーンでは、入力が画面に表示される。
- HISシーンでは、動画のストリーミング再生を行っている。
- DynamicModelシーンでは、サーバから3Dモデルをダウンロードし、空間に配置する。
- Stickmanシーンでは、MediaPipeが出力する関節座標群を受け取り、シーン中の棒人間の姿勢に反映する。

# NRSDKの、ハンドジェスチャーによる入力
- NRSDKのAPIは、NRKernal名前空間にある。
 - c#ソースコードの最初に、using NRKernal;と記述することで、使用できる。
- 手の動きを反映するには、NRHand_R/Lプレハブを、NRInput > Right/Left の子オブジェクトとしてシーンに配置し、NRInput  ゲームオブジェクトのNRInput > Input Source TypeをHandsに変更する。
- ハンドジェスチャーの使い方は、https://xreal.gitbook.io/nrsdk/development/hand-trackingが参考になる
- 手から出るレイは、NRHand_R/LプレハブのNRHandPointer_R/L子オブジェクトのRayCasterコンポーネントで使用する。

# ストリーミングについて
- m3u8ファイルによるストリーミングは、HISPlayerというunitypackageから行う。
 - m3u8は、一般的な動画ストリーミング用フォーマットである。
- HISPlayerは無料デモ版を使っている。動画ストリーミングを製品に組み込むなら、ライセンスに注意
- https://github.com/HISPlayer/Unity_Video_Player/releases/tag/v3.4.1
- ストリーミングツールには、NexPlayerというunitypackageもある。特段比較をしていないため、動画ストリーミングを製品に組み込むなら要検討
- リアルタイムのストリーミングには、Flutterというフレームワークがあるらしい

# 外部サーバからのリソース取得について
- アプリケーション実行時に、AR空間に配置する3Dオブジェクトとそのアニメーション(以下、リソース)を外部サーバから受け取るとする
- リソースは、画像かAssetBundleという形式でないと、動的に読み込むことができない
- DynamicModelでは、別リポジトリのBuildAssetBundleがビルドしたAssetBundleをロードして、シーンに配置している。
- サーバからのAssetBundleのロード、リソースの取り出し、配置は、ModelBundleLoader.csが行っている。
 - UnityWebRequestAssetBundle.GetAssetBundleで、サーバからAssetBundleを取得するリクエストを作る。
 - UnityWebRequest.SendWebRequestで、サーバにリクエストを送る。
 - DownloadHandlerAssetBundle.GetContentで、レスポンスからAssetBundleを得る。
 - AssetBundle.LoadAssetで、AssetBundleからリソースを取り出す。

# Stickmanについて
- MediaPipeは、人間が写った写真や動画から、関節(特徴点)群の三次元座標を予測することができる。
- MediaPipeのPythonコードから、文字列として、関節座標群を受け取るとして、その姿勢を反映した棒人間を生成している。
- 各関節をグラフ構造のノードとして、骨格を形成している。
- StickmanNodeは、関節=ノードを定義している。
- StickmanCreaterは、MediaPipe出力の文字列を元に、StickmanNodeを生成し、組み合わせて、棒人間を作っている。
