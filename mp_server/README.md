# Objective
後続のエンジニアが本サーバを利用・拡張できるようにする。  

# Overview
本プログラムは、`../HelloXReal`  で開発しているAndroidアプリケーションが利用するサーバである。  
flaskを使用して、Pythonで記述されている。  
主な機能は、以下である。  
- Androidアプリケーションから動画のアップロードを受け付ける。
- 動画がアップロードされたとき、mediapipeを走らせ、関節座標群の時系列データ(以下、シーケンス)を作り、保存する。
- Androidアプリケーションのリクエストに応じて、シーケンスや、シーケンスの一覧を返す。

# Execution
本ドキュメントがあるディレクトリにて、
1. (初回のみ)`mkdir videos`, `mkdir mediapipe`
2. (初回のみ)`python -m venv .venv`
3. `source .venv/bin/activate`  
4. (初回のみ)`pip install -r requirements.txt`
5. `python src`

# Struct
- src : ソースコード = Pythonスクリプト
  - \_\_init\_\_.py : プロジェクト認識用
  - \_\_main\_\_.py : エントリポイント。Flaskを動かす。それぞれのhttpリクエストに対する処理を記述する。
  - video_mediapipe.py : アップロードされた動画ファイルからシーケンスを作り、保存する。
- videos : アップロードされた動画を、保存している。
- mediapipe : シーケンスを、対応する動画と同名のテキストファイルに保存している。
- pose_landmarker_full.task : ファイル。Mediapipeの学習済モデル。
- requirements.txt
- README.md

# Mediapipe
学習済モデル(拡張子task)をロードする必要がある。  
[このページ](https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker/index)からダウンロードできる。  

参考になるサイトを置く。  
MediaPipeのプログラムで使用する個々のクラスについて、その役割を理解していないため、その説明はできない。  
理解せずとも、次のドキュメントをなぞれば、使用することはできると思う。  

mediapipeによる3次元姿勢推定は、以下のコードが参考になる。  
https://colab.research.google.com/github/googlesamples/mediapipe/blob/main/examples/pose_landmarker/python/%5BMediaPipe_Python_Tasks%5D_Pose_Landmarker.ipynb  

↑で登場するdetectメソッドの返値は、PoseLandmarkerResultクラスのインスタンス。  
そのプロパティであるpose_landmarksは、各ランドマーク(関節)の3次元座標(ローカル座標)などのステータスが、[このページ](https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker/index)のランドマーク番号順に含まれている。  
pose_world_landmarksは、pose_landmarksのワールド座標版である。