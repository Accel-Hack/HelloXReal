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

- You can plant in Assets > Scenes > Flower scene.
- You can indicate controller status in Assets > Scenes > IndicateController scene.

- NRSDKのAPIは、using NRKernal; で使用できる。
- 手の動きを反映するには、NRHand_R/Lプレハブを、NRInput > Right/Left の子オブジェクトとしてシーンに配置し、NRInput  ゲームオブジェクトのNRInput > Input Source TypeをHandsに変更する
- ハンドジェスチャーの使い方は、https://xreal.gitbook.io/nrsdk/development/hand-trackingが参考になる
- 手から出るレイは、NRHand_R/LプレハブのNRHandPointer_R/L子オブジェクトのRayCasterコンポーネントで使用する
