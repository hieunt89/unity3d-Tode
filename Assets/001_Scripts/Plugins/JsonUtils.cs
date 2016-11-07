using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;

public interface IInjectDataUtils {
	void SetDataUtils (IDataUtils dataUtils);
}

public interface IDataUtils {
	void CreateData<T> (T data);
	T LoadData<T> ();
	List<T> LoadAllData <T> ();
	void DeleteData (string path);
}

public class JsonUtils : IDataUtils{
//	const string ConstantString.DataPath = "Assets/Resources/Data/";

	public void CreateData<T> (T data) {

		var jsonString = JsonUtility.ToJson(data, true);
		Debug.Log(jsonString);

		var dataPath = ConstantString.DataPath + data.GetType().Name;

		if (!Directory.Exists (dataPath)) {
			Directory.CreateDirectory (dataPath);
		}

		FieldInfo field = typeof(T).GetField("id");
		string id = (string) field.GetValue(data);

		var finalPath = dataPath + "/"  + id + ".json";

		if (AssetDatabase.LoadAssetAtPath (finalPath, typeof(GameObject))) {
			if (EditorUtility.DisplayDialog ("Are you sure?", 
				id + ".json" + " is already exists. Do you want to overwrite it?",
				"Yes", "No")) {
				File.WriteAllText (finalPath, jsonString);
			}
		} else {
			File.WriteAllText (finalPath, jsonString);
		}
		AssetDatabase.Refresh();
	}

	public T LoadData<T> () {
		var path = EditorUtility.OpenFilePanel("Load " +  typeof(T).Name, ConstantString.DataPath +  typeof(T).Name, "json");

		var reader = new WWW("file:///" + path);
		while(!reader.isDone){
		}
		Debug.Log (reader.text);
		return (T) JsonUtility.FromJson (reader.text, typeof(T));
	}
		
	public List<T> LoadAllData <T> () {
		var list = new List<T> ();
		var path = "Data/" + typeof(T).Name;
		TextAsset[] files = Resources.LoadAll <TextAsset> (path) as TextAsset[];
		if (files != null && files.Length > 0) {
			for (int i = 0; i < files.Length; i++) {
				T data = (T)JsonUtility.FromJson (files [i].text, typeof(T));
				list.Add (data);
			}
		}
		return list;
	}

//	public T LoadDataById <T> (string id) {
//		var path = "Data/" + typeof(T).ToString() + "/" + id;
//		TextAsset file = Resources.Load <TextAsset> (path) as TextAsset;
//		return (T)JsonUtility.FromJson (file.text, typeof(T));
//	}

	public void DeleteData (string path)
	{
		AssetDatabase.DeleteAsset (path);
	}
}
