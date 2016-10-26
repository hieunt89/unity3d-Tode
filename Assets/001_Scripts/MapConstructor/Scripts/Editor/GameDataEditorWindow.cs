using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public interface IGameDataWindow {
	void OnGUI ();
	void ResetGUI ();
}

public class GameDataWindow <T> : IGameDataWindow where T : class {
	
	T _data;

	public void OnGUI () {
		if (_data == null)
			_data = DataManager.Instance.LoadData <T> ();
		FieldInfo[] fields = _data.GetType().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		// TODO: use object field
		for (int i = 0; i < fields.Length; i++) {
			EditorGUILayout.PropertyField (fields[i].Name, fields[i].GetValue (_data));
		}

//		for (int i = 0; i < fields.Length; i++) {
////			Debug.Log (fields[i].Attributes + " / " + fields[i].FieldType + " / " + fields[i].Name);
//			var typeCode = Type.GetTypeCode(fields[i].FieldType);
//			switch (typeCode) {
//			case TypeCode.String:
//				EditorGUILayout.TextField(fields[i].Name, fields[i].GetValue(_data).ToString ());
//				break;
//			case TypeCode.Int32:
//				if (fields[i].FieldType == typeof(AttackType)) {
//					EditorGUILayout.EnumPopup (fields[i].Name, (AttackType) fields[i].GetValue(_data));
//				} else {
//					EditorGUILayout.IntField (fields[i].Name, (Int32) fields[i].GetValue(_data));
//				}
//				break;
//			case TypeCode.Single:
//				EditorGUILayout.FloatField (fields[i].Name, (float) fields[i].GetValue(_data));
//				break;
//			}
//		}
	}

	public void ResetGUI () {
		_data = null;
	}
}

public class GameDataEditorWindow : EditorWindow {

	Type _type = null;
	object genericWindow = null;

	public Type Type {
		get { return _type.GetGenericArguments()[0]; }
		set {
			_type = typeof(GameDataWindow <>).MakeGenericType(value);

			genericWindow = System.Activator.CreateInstance(_type);
		}
	}

	[MenuItem("Game Data/Open Editor &T")]
	public static void ShowWindow()
	{
		var window = EditorWindow.GetWindow <GameDataEditorWindow> ("Game Data Editor",true);
		window.minSize = new Vector2 (500, 400);
		window.Show ();
	}

	void OnGUI () {

//		EditorGUILayout.
		if (genericWindow == null) {
			GUILayout.Label ("Select Game Data");
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
		(genericWindow as IGameDataWindow).OnGUI ();
		if (GUILayout.Button("Reset")) {
			(genericWindow as IGameDataWindow).ResetGUI ();
			genericWindow = null;
		}

	}
}
