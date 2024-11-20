# Objective
後続のエンジニアがスムーズにプロジェクトに合流できるように、情報を提供する。

# Overview
XRealのARグラスを用いた、Androidアプリケーション開発のプロジェクトである。  
想定するユースケースは、空手道場の指導支援である。師範がARグラスを装着する。弟子の動きを撮影した動画をサーバにアップロードし、師範のARグラス上に、弟子の動きを立体的に再現する。弟子のモデルはAR空間上に存在し、師範は自由な角度・位置から弟子のモデルを見ることができる。  

# Project
本ドキュメントがあるディレクトリに、HelloXRealとmp_serverという2つのプロジェクトがある。  
HelloXRealは、Androidアプリケーション用のUnityプロジェクトである。UnityプロジェクトのルートをUnityで開くと、Unity Editorを介してプロジェクトを編集することができる。  
mp_serverは、httpサーバのプロジェクトである。Pythonのflaskで実装している。  

# Architecture
ARグラスと接続したAndroidアプリケーションと、httpサーバが連携してサービスを実現する。Androidアプリケーション(または別のアプリケーション(未実装))からhttpサーバに動画をアップロードする。サーバ上でMediaPipeを実行して、動画の人物の関節座標群の時系列データ(以下、シーケンス)を推論する。Androidアプリケーションから、再生したいシーケンスを選択して、サーバからダウンロード、AR空間上の棒人間の動きとして再生する。  

通信は、以下の3つのシナリオで行われる。  
1. App開始時、サーバ上のシーケンス一覧を取得
![Sequence List](./README_assets/Sequence%20diagram%20of%20HelloXReal.png)  

2. Androidアプリケーションからの動画のアップロード
![Upload Video](./README_assets/Sequence%20diagram%20of%20HelloXReal-2.png)  

3. シーケンスのダウンロード、再生
![Download Sequence](./README_assets/Sequence%20diagram%20of%20HelloXReal-3.png)
