using UnityEditor;
using System.IO;
using UnityEngine;
	
class ImportAssetScript
{
     static void PerformImport()
     {
		AssetDatabase.ImportAsset(Path.Combine(Application.dataPath, "model.fbx"), ImportAssetOptions.Default);
     }
}