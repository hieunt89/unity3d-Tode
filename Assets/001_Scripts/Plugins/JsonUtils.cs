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
	void SaveData<T> (T data);
	T LoadData<T> ();
	List<T> LoadAllData <T> ();
}

public class JsonUtils : IDataUtils{
	const string dataDirectory = "Assets/Resources/Data/";

	public void SaveData<T> (T data) {

		var jsonString = JsonUtility.ToJson(data);
		Debug.Log(jsonString);

		if (!Directory.Exists (dataDirectory + data.GetType().Name)) {
			Directory.CreateDirectory (dataDirectory + data.GetType().Name);
		}

		FieldInfo field = typeof(T).GetField("id");
		string id = (string) field.GetValue(data);

		File.WriteAllText (dataDirectory + data.GetType().Name + "/"  + id + ".json", jsonString);

		AssetDatabase.Refresh();
	}

	public T LoadData<T> () {
		var path = EditorUtility.OpenFilePanel("Load " +  typeof(T).ToString(), dataDirectory +  typeof(T).ToString(), "json");

		var reader = new WWW("file:///" + path);
		while(!reader.isDone){
		}
		Debug.Log (reader.text);
		return (T) JsonUtility.FromJson (reader.text, typeof(T));
	}
		
	public List<T> LoadAllData <T> () {
		var list = new List<T> ();
		var path = "Data/" + typeof(T).ToString();
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
}
