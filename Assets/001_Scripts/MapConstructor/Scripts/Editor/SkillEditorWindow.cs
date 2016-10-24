using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;


public class SkillEditorWindow: EditorWindow {

	CombatSkillData combatSkill;
	SummonSkillData summonSkill;

	SkillType skillType;

	List<CombatSkillData> existCombatSkills;

	List<SummonSkillData> existSummonSkills;

	List<ProjectileData> existProjectiles;
	List<string> projectileIds;
	int projectileIndex;

	[MenuItem ("Window/Skill Editor &S")]
	public static void ShowWindow () {
		var skillEditorWindow = EditorWindow.GetWindow <SkillEditorWindow> ("Skill Editor", true);
		skillEditorWindow.minSize = new Vector2 (400, 600);
	}

	void OnFocus () {
//		Debug.Log ("On Focus");

		existCombatSkills = DataManager.Instance.LoadAllData <CombatSkillData> ();

		existSummonSkills = DataManager.Instance.LoadAllData <SummonSkillData> ();

		existProjectiles = DataManager.Instance.LoadAllData <ProjectileData> ();
		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}
		}
	}

	void OnEnable () {

	}

	void OnGUI () {
		skillType = (SkillType) EditorGUILayout.EnumPopup (skillType);
		switch (skillType) {
		case SkillType.None:
			break;
		case SkillType.Combat:
			if (combatSkill == null)
				combatSkill = new CombatSkillData ("combatskill" + existCombatSkills.Count);
			DrawCombatSkillEditor ();
			break;
		case SkillType.Summon:
			if (summonSkill == null)
				summonSkill = new SummonSkillData ("summonskill" + existSummonSkills.Count);
			DrawSummonSkillEditor ();
			break;
		}
		Repaint ();
	}
	bool toggleSkillEffect;
	private void DrawCombatSkillEditor () {
		

		EditorGUI.BeginChangeCheck ();
		var _combatSkillId = EditorGUILayout.TextField ("Id", combatSkill.id);
		var _combatSkillName = EditorGUILayout.TextField ("Name", combatSkill.name);
		var _combatSkillCoolDown = EditorGUILayout.FloatField ("Cooldown", combatSkill.cooldown);
		var _combatSkillCastRange = EditorGUILayout.FloatField ("Cast Range", combatSkill.castRange);
		var _combatSkillCastTime = EditorGUILayout.FloatField ("Cast Time", combatSkill.castTime);
		var _combatSkillCost = EditorGUILayout.IntField ("Cost", combatSkill.goldCost);
		projectileIndex = EditorGUILayout.Popup ("Project Id", projectileIndex, projectileIds.ToArray());
		var _combatSkillAOE = EditorGUILayout.FloatField ("AOE", combatSkill.aoe);
		var _combatSkillAttackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkill.attackType);
		var _combatSkillDamage = EditorGUILayout.IntField ("Damage", combatSkill.damage);

		// TODO: list skill effect

		if (EditorGUI.EndChangeCheck ()) {
			combatSkill.id = _combatSkillId;
			combatSkill.name = _combatSkillName;
			combatSkill.cooldown = _combatSkillCoolDown;
			combatSkill.castRange = _combatSkillCastRange;
			combatSkill.castTime = _combatSkillCastTime;
			combatSkill.goldCost = _combatSkillCost;
			combatSkill.projectileId = projectileIds[projectileIndex];
			combatSkill.aoe = _combatSkillAOE;
			combatSkill.attackType = _combatSkillAttackType;
			combatSkill.damage = _combatSkillDamage;
		}

		GUILayout.Space(5);
		toggleSkillEffect = EditorGUILayout.Foldout (toggleSkillEffect, "Skill Effect");
		if (toggleSkillEffect) {
			GUILayout.BeginVertical("box");
			for (int i = 0; i < combatSkill.effectList.Count; i++) {
				EditorGUI.BeginChangeCheck ();
				GUILayout.BeginHorizontal ();
				var _effectType = (EffectType) EditorGUILayout.EnumPopup ("Effect Type", combatSkill.effectList[i].effectType);
//				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Remove", GUILayout.MaxWidth (80))) {
					combatSkill.effectList.RemoveAt(i);
					continue;
				}
				GUILayout.EndHorizontal ();
				var _effectValue = EditorGUILayout.FloatField ("Value", combatSkill.effectList[i].value);
				var _effectDuration = EditorGUILayout.FloatField ("Duration", combatSkill.effectList[i].duration);
				if (EditorGUI.EndChangeCheck ()) {
					combatSkill.effectList[i].skillId = combatSkill.id;
					combatSkill.effectList[i].effectType = _effectType;
					combatSkill.effectList[i].value = _effectValue;
					combatSkill.effectList[i].duration = _effectDuration;
				}
				GUILayout.Space(5);

			}
			if (GUILayout.Button("Add New Skill Effect")){
				combatSkill.effectList.Add (new SkillEffect ()); 
			}
			GUILayout.EndVertical();
		}
		GUILayout.Space(5);

		GUI.enabled = !String.IsNullOrEmpty (combatSkill.name) && combatSkill.effectList != null; 
		if (GUILayout.Button("Save")){
			Debug.Log (combatSkill.effectList.Count);
			DataManager.Instance.SaveData (combatSkill);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			var data = DataManager.Instance.LoadData <CombatSkillData> ();
			if(data != null){
				combatSkill = data;
				projectileIndex = combatSkill.projectileIndex;
			}
		}
		if (GUILayout.Button("Reset")){
			combatSkill = new CombatSkillData ("combatskill" + existCombatSkills.Count);
		}
	}

	private void DrawSummonSkillEditor () {
		EditorGUI.BeginChangeCheck ();
		var _summonSkillId = EditorGUILayout.TextField ("Id", summonSkill.id);
		var _summonSkillName = EditorGUILayout.TextField ("Name", summonSkill.name);
		var _summonSkillCoolDown = EditorGUILayout.FloatField ("Cooldown", summonSkill.cooldown);
		var _summonSkillCastRange = EditorGUILayout.FloatField ("Cast Range", summonSkill.castRange);
		var _summonSkillCastTime = EditorGUILayout.FloatField ("Cast Time", summonSkill.castTime);
		var _summonSkillCost = EditorGUILayout.IntField ("Cost", summonSkill.goldCost);
		var _summonSkillSummonId = EditorGUILayout.TextField ("AOE", summonSkill.summonId);
		var _summonSkillSummonCount = EditorGUILayout.IntField ("Attack Type", summonSkill.summonCount);
		var _summonSkillDuration = EditorGUILayout.FloatField ("Damage", summonSkill.duration);

		// TODO: list skill effect

		if (EditorGUI.EndChangeCheck ()) {
			summonSkill.id = _summonSkillId;
			summonSkill.name = _summonSkillName;
			summonSkill.cooldown = _summonSkillCoolDown;
			summonSkill.castRange = _summonSkillCastRange;
			summonSkill.castTime = _summonSkillCastTime;
			summonSkill.goldCost = _summonSkillCost;
			summonSkill.summonId = _summonSkillSummonId;
			summonSkill.summonCount = _summonSkillSummonCount;
			summonSkill.duration = _summonSkillDuration;
		}
		GUILayout.Space(5);

		GUI.enabled = !String.IsNullOrEmpty (summonSkill.name);
		if (GUILayout.Button("Save")){
			DataManager.Instance.SaveData (summonSkill);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			var data = DataManager.Instance.LoadData <SummonSkillData> ();
			if(data != null){
				summonSkill = data;
			}
		}
		if (GUILayout.Button("Reset")){
			summonSkill = new SummonSkillData ("summonskill" + existSummonSkills.Count);
		}
	}
}
