using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class BuildAssetBundlesWithArgs
{
    // コマンドラインからAssetBundleをビルドするメソッド
    public static void BuildAllAssetBundlesWithArgs()
    {
        // コマンドライン引数を取得
        string[] args = Environment.GetCommandLineArgs();

        // 出力ディレクトリ
        string assetBundleDirectory = "../Assets/AssetBundles";

        // AssetBundleに含めるアセットのパス
        string assetPath = null;

        // コマンドライン引数を解析してアセットパスを取得
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-assetPath" && i + 1 < args.Length)
            {
                assetPath = args[i + 1];
                break;
            }
        }

        // アセットパスが指定されているか確認
        if (string.IsNullOrEmpty(assetPath) || !File.Exists(assetPath))
        {
            Debug.LogError("Error: No valid asset path provided.");
            return;
        }

        // 出力ディレクトリが存在しない場合は作成
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // アセットにアセットバンドル名を設定
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        if (importer != null)
        {
            importer.assetBundleName = Path.GetFileNameWithoutExtension(assetPath);
        }
        else
        {
            Debug.LogError("Error: Could not find asset at path: " + assetPath);
            return;
        }

        // AssetBundleをビルド
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            EditorUserBuildSettings.activeBuildTarget); //IDK if target is Mac or Android...

        // 完了メッセージ
        Debug.Log("AssetBundle built successfully for asset: " + assetPath);
    }
}
