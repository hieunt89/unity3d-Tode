using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SummonSkillEditorWindow : EditorWindow {

	SummonSkillData summonSkill;

	IDataUtils dataUtils;

	[MenuItem ("Tode/Summon Skill Editor")]
	public static void ShowWindow () {
		var summonSkillEditorWindow = EditorWindow.GetWindow <SummonSkillEditorWindow> ("Summon Skill Editor", true);
		summonSkillEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();
		summonSkill = new SummonSkillData (Guid.NewGuid().ToString());
	}

	void OnFocus () {
	}

	void OnGUI () {
		EditorGUI.BeginChangeCheck ();
		summonSkill.id = EditorGUILayout.TextField ("Id", summonSkill.id);
		summonSkill.name = EditorGUILayout.TextField ("Name", summonSkill.name);
		summonSkill.description = EditorGUILayout.TextField ("Description", summonSkill.description);
		summonSkill.spritePath = EditorGUILayout.TextField ("Sprite", summonSkill.spritePath);
		summonSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", summonSkill.cooldown);
		summonSkill.castRange = EditorGUILayout.FloatField ("Cast Range", summonSkill.castRange);
		summonSkill.castTime = EditorGUILayout.FloatField ("Cast Time", summonSkill.castTime);
		summonSkill.goldCost = EditorGUILayout.IntField ("Cost", summonSkill.goldCost);
		summonSkill.summonId = EditorGUILayout.TextField ("AOE", summonSkill.summonId);
		summonSkill.summonCount = EditorGUILayout.IntField ("Attack Type", summonSkill.summonCount);
		summonSkill.duration = EditorGUILayout.FloatField ("Damage", summonSkill.duration);

		GUILayout.Space(5);

		GUI.enabled = !String.IsNullOrEmpty (summonSkill.name);
		if (GUILayout.Button("Save")){
			dataUtils.CreateData (summonSkill);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			var data = dataUtils.LoadData <SummonSkillData> ();
			if (data == null) {
				summonSkill = new SummonSkillData (Guid.NewGuid().ToString ());

			} else {
				summonSkill = data;
			}
		}
		if (GUILayout.Button("Reset")){
			summonSkill = new SummonSkillData (Guid.NewGuid().ToString ());
		}
	}
}
