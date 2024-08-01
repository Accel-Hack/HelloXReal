# Setup

## Install
[Getting Started with NRSDK]に従う

- [NRSDK](https://developer.xreal.com/download)
- [Unity Hub](https://unity.com/ja/download)
- [Android Studio](https://developer.android.com/studio?hl=ja) (option)
  - ほしいのはAndroid SDK
  - UnityからもAndroid SDKをインストールできたのでいらないかも

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
