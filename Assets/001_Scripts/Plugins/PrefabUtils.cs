using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public interface IInjectPrefabUtils {
	void SetPrefabUtils (IPrefabUtils prefabUtils);
}

public interface IPrefabUtils {
	void SavePrefab (GameObject obj);
	void SavePrefabs (GameObject[] objects);
}

public class PrefabUtils : IPrefabUtils {
	public const string path = "Assets/Resources/Prefabs/";

	public void SavePrefab (GameObject obj)
	{
		CheckDirectory ();

		var finalPath = path +  obj.name + ".prefab";

		if (AssetDatabase.LoadAssetAtPath (finalPath, typeof(GameObject))) {
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				    "The prefab is already exists. Do you want to overwrite it?",
				    "Yes", "No")) {
				CreateNewPrefab (obj, finalPath);
			}
		} else {
			CreateNewPrefab (obj, finalPath);
		}
	}

	public void  SavePrefabs (GameObject[] objects)
	{
		CheckDirectory ();

		foreach (var obj in objects) {
			var finalPath = path +  obj.name + ".prefab";
			if (AssetDatabase.LoadAssetAtPath (finalPath, typeof(GameObject))) {
				if (EditorUtility.DisplayDialog ("Are you sure?", 
					    "The prefab is already exists. Do you want to overwrite it?",
					    "Yes", "No")) {
					CreateNewPrefab (obj, finalPath);
				}
			} else {
				CreateNewPrefab (obj, finalPath);
			}
		}
	}

	private void CheckDirectory () {
		if (!Directory.Exists (path)) {
			Directory.CreateDirectory (path);
		}
	}

	private void CreateNewPrefab (GameObject obj, string finalPath) {
		Debug.Log (finalPath);
		var prefab = PrefabUtility.CreateEmptyPrefab (finalPath);
		PrefabUtility.ReplacePrefab (obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
	}
}
