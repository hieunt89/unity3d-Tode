using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;


public class GameData : IDataUtils {

	public void CreateData<T> (T data) where T : ScriptableObject {
//		T asset = ScriptableObject.CreateInstance<T> ();
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath ("Assets/Resources/Data/" + typeof(T).Name + ".asset");
		AssetDatabase.CreateAsset (data, assetPathAndName);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = data;
	}

	public T LoadData<T> () where T : class {
		var path = "Assets/Resources/Data/" + typeof(T).Name + ".asset";
		var data = AssetDatabase.LoadAssetAtPath (path, typeof(T)) as T;
		return data;

	}

	public List<T> LoadAllData<T> ()  {
		List<T> list = new List<T> ();

		if (typeof(T) == typeof(TowerData)) {
			var data = AssetDatabase.LoadAssetAtPath (ConstantString.TowerDataPath, typeof(TowerList)) as TowerList;
			list = (List<T>) (object) data.towers;
			return list;
		}

		if (typeof(T) == typeof(CharacterData)) {
			var data = AssetDatabase.LoadAssetAtPath (ConstantString.CharacterDataPath, typeof(CharacterList)) as CharacterList;
			list = (List<T>) (object) data.characters;
			return list;
		}
		if (typeof(T) == typeof(ProjectileData)) {
			var data = AssetDatabase.LoadAssetAtPath (ConstantString.ProjectileDataPath, typeof(ProjectileList)) as ProjectileList;
			list = (List<T>) (object) data.projectiles;
			return list;
		}
		return default (List<T>);
	}

	public void DeleteData (string path) {
	}
}
