using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;


public class SkillEditorWindow: EditorWindow {

	CombatSkillData combatSkill;
	SummonSkillData summonSkill;
	bool toggleProjectile;
	SkillType skillType;

	List<CombatSkillData> existCombatSkills;

	List<SummonSkillData> existSummonSkills;

	List<ProjectileData> existProjectiles;
	List<string> projectileIds;
	int projectileIndex;

	bool toggleSkillEffect;

	IDataUtils dataUtils;

	[MenuItem ("Tode/Skill Editor &S")]
	public static void ShowWindow () {
		var skillEditorWindow = EditorWindow.GetWindow <SkillEditorWindow> ("Skill Editor", true);
		skillEditorWindow.minSize = new Vector2 (400, 600);
	}

	void LoadExistData () {
		existCombatSkills = dataUtils.LoadAllData <CombatSkillData> ();

		existSummonSkills = dataUtils.LoadAllData <SummonSkillData> ();

		existProjectiles = dataUtils.LoadAllData <ProjectileData> ();
	}

	void SetupProjectileIndex () {

		// get project ids from exist projectiles
		if (existProjectiles.Count > 0) {
			projectileIds = new List<string> ();
			for (int i = 0; i < existProjectiles.Count; i++) {
				projectileIds.Add(existProjectiles[i].Id);
			}

			// setup projectileindex popup
			for (int i = 0; i < projectileIds.Count; i++) {
				if (combatSkill.projectileId.Equals (projectileIds[i])) {
					projectileIndex = i;
					continue;
				}
			}
		} else {
			projectileIndex = 0;
		}



	}

	void OnEnable () {
		dataUtils = DIContainer.GetModule <IDataUtils> ();

		LoadExistData ();
		SetupProjectileIndex ();
	}

	void OnFocus () {
		LoadExistData ();
		SetupProjectileIndex ();

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



	private void DrawCombatSkillEditor () {
		EditorGUI.BeginChangeCheck ();
		combatSkill.id = EditorGUILayout.TextField ("Id", combatSkill.id);
		combatSkill.name = EditorGUILayout.TextField ("Name", combatSkill.name);
		combatSkill.cooldown = EditorGUILayout.FloatField ("Cooldown", combatSkill.cooldown);
		combatSkill.castRange = EditorGUILayout.FloatField ("Cast Range", combatSkill.castRange);
		combatSkill.castTime = EditorGUILayout.FloatField ("Cast Time", combatSkill.castTime);
		combatSkill.goldCost = EditorGUILayout.IntField ("Cost", combatSkill.goldCost);
	
		projectileIndex = EditorGUILayout.Popup ("Projectile", projectileIndex, projectileIds.ToArray());
		combatSkill.projectileId = projectileIds[projectileIndex];

		GUILayout.BeginVertical ("box");
		toggleProjectile = EditorGUILayout.Foldout (toggleProjectile, "Projectile Detail");
		if (toggleProjectile) {
			EditorGUILayout.LabelField ("Name", existProjectiles[projectileIndex].Name);
			EditorGUILayout.LabelField ("Type", existProjectiles[projectileIndex].Type.ToString ());
			EditorGUILayout.LabelField ("TravelSpeed", existProjectiles[projectileIndex].TravelSpeed.ToString ());
			EditorGUILayout.LabelField ("Duration", existProjectiles[projectileIndex].Duration.ToString ());
			EditorGUILayout.LabelField ("MaxDmgBuildTime", existProjectiles[projectileIndex].MaxDmgBuildTime.ToString ());
			EditorGUILayout.LabelField ("TickInterval", existProjectiles[projectileIndex].TickInterval.ToString ());
		}
		GUILayout.EndVertical ();

		combatSkill.aoe = EditorGUILayout.FloatField ("AOE", combatSkill.aoe);
		combatSkill.attackType = (AttackType) EditorGUILayout.EnumPopup ("Attack Type", combatSkill.attackType);
		combatSkill.damage = EditorGUILayout.IntField ("Damage", combatSkill.damage);


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
//			Debug.Log (combatSkill.effectList.Count);
			dataUtils.CreateData (combatSkill);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			var data = dataUtils.LoadData <CombatSkillData> ();
			if(data == null){
				combatSkill = new CombatSkillData ("combatskill" + existCombatSkills.Count);
				SetupProjectileIndex ();
			}
		}
		if (GUILayout.Button("Reset")){
			combatSkill = new CombatSkillData ("combatskill" + existCombatSkills.Count);
			projectileIndex = 0;
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
			dataUtils.CreateData (summonSkill);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Load")){
			var data = dataUtils.LoadData <SummonSkillData> ();
			if(data == null){
				summonSkill = new SummonSkillData ("summonskill" + existSummonSkills.Count);
			}
		}
		if (GUILayout.Button("Reset")){
			summonSkill = new SummonSkillData ("summonskill" + existSummonSkills.Count);
		}
	}
}
