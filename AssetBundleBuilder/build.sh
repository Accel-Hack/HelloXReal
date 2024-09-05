#! /bin/bash

/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath /Users/akaskakotaro/Documents/HelloXReal/HelloXReal \
    -executeMethod BuildAssetBundles.BuildAllAssetBundles \
    -assetPath model.fbx \
    -logFile /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/AssetBundleBuilder/build_log.txt

