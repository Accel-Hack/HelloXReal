using UnityEditor;
using System.IO;
using UnityEngine;
	
class ImportAssetScript
{
     static void PerformImport()
     {
          string path = Path.Combine(Application.dataPath, "model.fbx");

          // Import model.fbx from local file. This step produces model.fbx.meta.
		AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);

     }
}