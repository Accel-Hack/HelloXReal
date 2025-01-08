# Overview
複数人が映った動画に対してYOLOXとMediaPipeを適用し、全員の姿勢を推定する。  
この時得たデータをエンコードし、Unity製ARアプリケーションに渡す。  
そのエンコードのルールを示す。

# Data
`src/data.py`が、データ構造とエンコード方法を定義している。  
VideoDataクラスが、動画1つの情報を丸々格納している。  
VideoDataの実体は、FrameDataのリストである。FrameDataクラスは、動画1フレーム毎の情報を格納している。  
FrameDataの実体は、PersonFrameDataのリストである。PersonFrameDataクラスは、あるフレームに映っている、1人の位置と姿勢の情報を格納している。  

# Encode protocol
PersonFrameData 1つは、2byte整数(4 + 3 * 33)個にエンコードされる。  
動画1辺の長さの最大値を65536に限れば、x, y座標をそれぞれ2byte整数で表せる。  
初めの4つは、YOLOXが検出する、人物の矩形領域の対角の座標を表す。  
以降は、MediaPipeの出力であり、関節座標の3次元データが、33点分記録される。
いずれもリトルエンディアンで、前4個は符号なし、後3 * 33個は符号ありである。

FrameDataは、PersonFrameDataをエンコードしたものを、単に並べたものとしてエンコードされる。

VideoDataは、FrameDataをエンコードしたものを、並べたものとしてエンコードされる。  
ただし、フレームの区切りに、2byte分の1 (b'\x11\x11') をPersonFrameData 1個分並べたものが挿入される。  
コーディングの都合上、データの最後にもこの1の塊が挿入される。