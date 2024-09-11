#! /bin/bash

cp /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/AssetBundleBuilder/model.fbx \
    /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/Assets/

/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath ~/Documents/HelloXReal/HelloXReal \
    -executeMethod ImportAssetScript.PerformImport \
    -logFile /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/AssetBundleBuilder/import_log.txt

sed -e 's/  assetBundleName:/  assetBundleName: external/g' /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/Assets/model.fbx.meta

/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath ~/Documents/HelloXReal/HelloXReal \
    -executeMethod CreateAssetBundles.BuildAllAssetBundles \
    -logFile /Users/akaskakotaro/Documents/HelloXReal/HelloXReal/AssetBundleBuilder/build_log.txt