using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public interface IInjectPrefabUtils {
	void SetPrefabUtils (IPrefabUtils prefabUtils);
}

public interface IPrefabUtils {
	void CreatePrefab (GameObject obj);
//	void CreatePrefabs (GameObject[] objects);
	GameObject InstantiatePrefab (string path);
	void DeletePrefab (string path);
}

public class PrefabUtils : IPrefabUtils {

	public void CreatePrefab (GameObject obj)
	{
		CheckDirectory ();

		var finalPath = ConstantString.PrefabPath +  obj.name + ".prefab";

		if (AssetDatabase.LoadAssetAtPath (finalPath, typeof(GameObject))) {
			if (EditorUtility.DisplayDialog ("Are you sure?", 
					obj.name + ".prefab" + " is already exists. Do you want to overwrite it?",
				    "Yes", "No")) {
				CreateNewPrefab (obj, finalPath);
			}
		} else {
			CreateNewPrefab (obj, finalPath);
		}
	}

//	public void CreatePrefabs (GameObject[] objects)
//	{
//		CheckDirectory ();
//
//		foreach (var obj in objects) {
//			var finalPath = ConstantString.PrefabPath +  obj.name + ".prefab";
//			if (AssetDatabase.LoadAssetAtPath (finalPath, typeof(GameObject))) {
//				if (EditorUtility.DisplayDialog ("Are you sure?", 
//					    "The prefab is already exists. Do you want to overwrite it?",
//					    "Yes", "No")) {
//					CreateNewPrefab (obj, finalPath);
//				}
//			} else {
//				CreateNewPrefab (obj, finalPath);
//			}
//		}
//	}

	private void CheckDirectory () {
		if (!Directory.Exists (ConstantString.PrefabPath)) {
			Directory.CreateDirectory (ConstantString.PrefabPath);
		}
	}

	private void CreateNewPrefab (GameObject obj, string finalPath) {
		var prefab = PrefabUtility.CreateEmptyPrefab (finalPath);
		PrefabUtility.ReplacePrefab (obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
//		PrefabUtility.CreatePrefab (finalPath, obj, ReplacePrefabOptions.Default);
	}

	public GameObject InstantiatePrefab(string path) {	
		var go = AssetDatabase.LoadMainAssetAtPath(path);
		if (go != null) {
			EditorGUIUtility.PingObject (go);
				
			if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab) {
				var clone = PrefabUtility.InstantiatePrefab(go as GameObject) as GameObject; 
				return clone;
			} 
		}
		return null;
	}

	public void DeletePrefab (string path) {
		AssetDatabase.DeleteAsset (path);
	}
}
