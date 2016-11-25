using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;


public class GameAssetUtils : IDataUtils {
	public void CreateData<T> (T data)
	{
	}

	public T LoadData<T> ()
	{
		return default (T);
	}

	public List<T> LoadAllData<T> ()
	{
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

	public void DeleteData (string path)
	{
	}
}
