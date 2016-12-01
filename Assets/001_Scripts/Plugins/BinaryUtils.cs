using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
//
//public class BinaryUtils : IDataUtils {
//	public const string databasePath = "/Resources/Data/Trees/";
//	public const string dataExtension = ".txt";
//
//	public void CreateData<T> (T data)
//	{
//		BinaryFormatter bf = new BinaryFormatter ();
//
//		var dataType = typeof(T).GetField("treeType").GetValue (data).ToString ();
//		if (!Directory.Exists (Application.dataPath + databasePath + dataType)) {
//			Directory.CreateDirectory (Application.dataPath + databasePath + dataType);
//		}
//
//		FieldInfo field = typeof(T).GetField("id");
//		string dataID = (string) field.GetValue(data); 
//
//		FileStream file = File.Create (Application.dataPath + databasePath + dataType + "/" + dataID + dataExtension);
//		bf.Serialize (file, data);
//		file.Close ();
//		AssetDatabase.Refresh ();
//	}
//
//	public T LoadData<T> ()
//	{
//		T data = default(T);
//		string treePath = EditorUtility.OpenFilePanel ("Load Tree", Application.dataPath + databasePath, "txt");
//		int appPathLength = Application.dataPath.Length;
//		string finalPath = treePath.Substring (appPathLength - dataExtension.Length - 2);
//
//		BinaryFormatter bf = new BinaryFormatter ();
//		FileStream file = File.Open (finalPath, FileMode.Open);
//		data = (T) bf.Deserialize (file);
//		file.Close ();
//		return data;
//	}
//
//	public List<T> LoadAllData<T> ()
//	{
//		var trees = new List<T> ();
//		var dataTypes = Enum.GetNames (typeof (TreeType));
//		for (int typeIndex = 0; typeIndex < dataTypes.Length; typeIndex++) {
//			if (Directory.Exists (Application.dataPath + databasePath + dataTypes[typeIndex])) {
//				BinaryFormatter bf = new BinaryFormatter ();
//				
//				var texts = Resources.LoadAll<TextAsset> ("Data/Trees/" + dataTypes[typeIndex]);
//				for (int i = 0; i < texts.Length; i++) {
//					Stream s = new MemoryStream(texts[i].bytes);
//					T tree = (T) bf.Deserialize (s);
//					trees.Add ( tree);
//				}
//			}
//		}
//		return trees;
//	}
//
//	public void DeleteData (string path)
//	{
//		throw new System.NotImplementedException ();
//	}
//}