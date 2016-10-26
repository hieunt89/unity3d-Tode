using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public interface IGenericWindow {
	void OnGUI ();
	void ResetGUI ();
}

public class GenericWindow <T> : IGenericWindow where T : class {
	
	T _data;

	public void OnGUI () {
		if (_data == null){
			_data = DataManager.Instance.LoadData <T> ();
			return;
		}

		FieldInfo[] fields = _data.GetType().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		for (int i = 0; i < fields.Length; i++) {

			// TODO: using objectfield
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
		}
//		GUI.enabled = CheckFields ();
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (_data);
		}
//		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			_data = DataManager.Instance.LoadData <T> ();
		}
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

	[MenuItem("Generic Editor/Open Editor &T")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow <GenericEditorWindow> ("Generic Editor",true);
	}

	void OnGUI () {

//		EditorGUILayout.
		if (genericWindow == null) {
			if (GUILayout.Button("CustomData")) {
				Type = typeof(CustomData);
			}
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

	}
}
