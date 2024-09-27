#! /bin/bash

cp "$1" /Users/akaskakotaro/BuildAssetBundle/Assets/model.fbx

/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath ~/BuildAssetBundle \
    -executeMethod ImportModelAndAnimation.Perform \
    -logFile /Users/akaskakotaro/BuildAssetBundle/import_log.txt \

/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath ~/BuildAssetBundle \
    -executeMethod SetAnimation.Perform \
    -logFile /Users/akaskakotaro/BuildAssetBundle/setanim_log.txt \
    -animationName "$2"

sed -i '' 's/assetBundleName:.*/assetBundleName: external/' /Users/akaskakotaro/BuildAssetBundle/Assets/model.prefab.meta


/Users/akaskakotaro/2022.3.43f1/Unity.app/Contents/MacOS/Unity \
    -quit \
    -batchmode \
    -projectPath ~/BuildAssetBundle \
    -executeMethod CreateAssetBundles.BuildAllAssetBundles \
    -logFile /Users/akaskakotaro/BuildAssetBundle/build_log.txt