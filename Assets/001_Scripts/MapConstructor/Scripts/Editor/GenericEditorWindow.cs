using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public interface IGenericWindow {
	void OnGUI ();
}

public class GenericWindow <T> : IGenericWindow {
	
	T _data;

	public void OnGUI () {
		if (_data == null)
			_data = DataManager.Instance.LoadData <T> ();
		FieldInfo[] fields = _data.GetType().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		for (int i = 0; i < fields.Length; i++) {
//			Debug.Log (fields[i].Attributes + " / " + fields[i].FieldType + " / " + fields[i].Name);
			var typeCode = Type.GetTypeCode(fields[i].FieldType);
			switch (typeCode) {
			case TypeCode.String:
				EditorGUILayout.TextField(fields[i].Name, fields[i].GetValue(_data).ToString ());
				break;
			case TypeCode.Int32:
				if (fields[i].FieldType == typeof(AttackType)) {
					EditorGUILayout.EnumPopup (fields[i].Name, (AttackType) fields[i].GetValue(_data));
				} else {
					EditorGUILayout.IntField (fields[i].Name, (Int32) fields[i].GetValue(_data));
				}
				break;
			case TypeCode.Single:
				EditorGUILayout.FloatField (fields[i].Name, (float) fields[i].GetValue(_data));
				break;
			}
//			GUILayout.Label (fields[i].Attributes + " / " + fields[i].FieldType + " / " + fields[i].Name);

		}
	}
}

public class GenericEditorWindow: EditorWindow {

	Type _type = null;
	object genericWindow = null;

	public Type Type {
		get { return _type.GetGenericArguments()[0]; }
		set {
			_type = typeof(GenericWindow <>).MakeGenericType(value);

			genericWindow = System.Activator.CreateInstance(_type);

		}
	}

	[MenuItem("Generic Editor/Open Editor &T")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow <GenericEditorWindow> ("Generic Editor",true);
	}

	void OnGUI () {
		if (genericWindow == null) {
			if ( GUILayout.Button("TowerData")) {
				Type = typeof(TowerData);
//				var data = DataManager.Instance.LoadData <TowerData> ();

//				Type = data.GetType ();
			}
			return;
		}
		(genericWindow as IGenericWindow).OnGUI ();
	}
}
