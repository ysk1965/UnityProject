using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApplyChangePrefabs {

	[MenuItem("GameObject/Apply Prefab Changes", false, 0)]
	public static void ApplyChanges()
	{
		foreach (GameObject obj in Selection.gameObjects)
		{
			GameObject prefab_root = PrefabUtility.FindPrefabRoot(obj);
			Object prefab_src = PrefabUtility.GetCorrespondingObjectFromSource(prefab_root);
			if(prefab_src != null)
			{
				PrefabUtility.ReplacePrefab(prefab_root, prefab_src,  ReplacePrefabOptions.ConnectToPrefab);
				Debug.Log("Updating prefab : "+AssetDatabase.GetAssetPath(prefab_src));
			}
			else
			{
				Debug.Log("Selected has no prefab");
			}
		}
	}
}
