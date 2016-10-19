using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

public interface IGenericWindow {
	void OnFocus ();
	void OnGUI ();
	void ResetGUI ();
}

public class GenericWindow <T> : IGenericWindow where T : class {
	
	T _data;
	List<T> existData;

	public void OnFocus () {
	}

	public void OnGUI () {
		if (existData == null) {
			existData = DataManager.Instance.LoadAllData <T> ();
		}
		if (_data == null)
			_data = DataManager.Instance.LoadData <T> ();
		FieldInfo[] fields = _data.GetType().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		for (int i = 0; i < fields.Length; i++) {
//			Debug.Log (fields[i].Attributes + " / " + fields[i].FieldType + " / " + fields[i].Name);
			var typeCode = Type.GetTypeCode(fields[i].FieldType);
			switch (typeCode) {
			case TypeCode.String:
				EditorGUI.BeginChangeCheck ();
				var _textValue = EditorGUILayout.TextField (fields [i].Name, fields [i].GetValue (_data).ToString ());
				if (EditorGUI.EndChangeCheck ()) {
					_data.GetType ().GetField (fields [i].Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue (_data, _textValue);
				}
				break;
			case TypeCode.Int32:
				if (fields[i].FieldType == typeof(AttackType)) {
					EditorGUI.BeginChangeCheck ();
					var _enumValue = EditorGUILayout.EnumPopup (fields[i].Name, (AttackType) fields[i].GetValue(_data));
					if (EditorGUI.EndChangeCheck ()) {
						_data.GetType ().GetField (fields [i].Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue (_data, _enumValue);
					}
				} else {
					EditorGUI.BeginChangeCheck ();
					var _intValue = EditorGUILayout.IntField (fields[i].Name, (Int32) fields[i].GetValue(_data));
					if (EditorGUI.EndChangeCheck ()) {
						_data.GetType ().GetField (fields [i].Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue (_data, _intValue);
					}
				}


				break;
			case TypeCode.Single:
				EditorGUI.BeginChangeCheck ();
				var _floatValue = EditorGUILayout.FloatField (fields[i].Name, (float) fields[i].GetValue(_data));
				if (EditorGUI.EndChangeCheck ()) {
					_data.GetType ().GetField (fields [i].Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue (_data, _floatValue);
				}
				break;
			}
		}

		if (GUILayout.Button ("Save")) {
			DataManager.Instance.SaveData <T> (_data);
		}
		if (GUILayout.Button ("Load")) {
			_data = DataManager.Instance.LoadData <T> ();
		}
//		if (GUILayout.Button ("Save")) {
//
//		}
	}

	public void ResetGUI () {
		Debug.Log ("Reset");
		_data = null;
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

	[MenuItem("Generic Editor/Open Editor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow <GenericEditorWindow> ("Generic Editor",true);
	}

	void OnGUI () {

//		EditorGUILayout.
		if (genericWindow == null) {
			if (GUILayout.Button("TowerData")) {
				Type = typeof(TowerData);
			}
			if (GUILayout.Button("CharacterData")) {
				Type = typeof(CharacterData);
			}
			if (GUILayout.Button("ProjectileData")) {
				Type = typeof(ProjectileData);
			}
			if (GUILayout.Button("CombatSkillData")) {
				Type = typeof(CombatSkillData);
			}
			if (GUILayout.Button("SummonSkillData")) {
				Type = typeof(SummonSkillData);
			}
			return;
		}
		(genericWindow as IGenericWindow).OnGUI ();
		if (GUILayout.Button("Reset")) {
			(genericWindow as IGenericWindow).ResetGUI ();
			genericWindow = null;
		}

		Repaint ();
	}
}
