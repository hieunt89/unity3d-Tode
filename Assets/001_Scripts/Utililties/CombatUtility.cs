﻿using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System.Linq;

public class CombatUtility{
	public static int GetDamageAfterReduction(int damage, float reduceTo){
		return Mathf.CeilToInt(damage * reduceTo);
	}

	public static int RandomDamage(int maxDmg, int minDmg){
		return Random.Range (minDmg, maxDmg);
	}

	public static int RandomDamage(int maxDmg, int minDmg, AttackType atkType, List<ArmorData> enemyArmors){
		int damage = Random.Range (minDmg, maxDmg);
		float reduceTo = GetDamageReduction (atkType, enemyArmors);
		damage = Mathf.CeilToInt(damage * reduceTo);
		return damage;
	}

	public static float GetDamageReduction(AttackType atkType, List<ArmorData> armors){
		float result = 0f;
		for (int i = 0; i < armors.Count; i++) {
			if (armors [i].Type == atkType) {
				result = 1 - armors[i].Reduction*0.01f;
				break;
			}
		}
		return result;
	}

	public static int GetDamage(int maxDmg, int minDmg, float dmgScale, AttackType atkType, List<ArmorData> enemyArmors){
		int damage = Mathf.CeilToInt((maxDmg - minDmg) * dmgScale + minDmg);
		float reduction = GetDamageReduction (atkType, enemyArmors);
		damage = Mathf.CeilToInt(damage * reduction);
		return damage;
	}

	public static List<Entity> FindTargetsInRange(Entity attacker, Entity[] targets, float range){
		var targetableEnemies = new List<Entity> ();
		for (int i = 0; i < targets.Length; i++) {
			if( targets[i].position.value.IsInRange(attacker.position.value, range) ){
				targetableEnemies.Add (targets [i]);
			}
		}
		return targetableEnemies;
	}

	public static Entity FindTargetInRange(Entity attacker, Entity[] targets, float range){
		var targetableEnemies = new List<Entity> ();
		for (int i = 0; i < targets.Length; i++) {
			if( targets[i].position.value.IsInRange(attacker.position.value, range) ){
				targetableEnemies.Add (targets [i]);
			}
		}
		return ChooseTarget (targetableEnemies);
	}

	public static Entity ChooseTarget(List<Entity> targets){
		Entity chosenOne = null;
		if (targets.Count > 0) {
			float minDis = Mathf.Infinity;
			for (int i = 0; i < targets.Count; i++) {
				var target = targets [i];
				var finalDes = target.pathReference.e.path.wayPoints.LastOrDefault ();
				var distance = Vector3.Distance (target.position.value, finalDes);

				if (distance < minDis) {
					minDis = distance;
					chosenOne = target;
				}
			}
		}
		return chosenOne;
	}

	public static bool ReplaceEffectDurationIfDuplicated(ref Dictionary<SkillEffect, float> efToDur, SkillEffect newEf){
		foreach(KeyValuePair<SkillEffect, float> entry in efToDur)
		{
			if (entry.Key.skillId == newEf.skillId) {
				efToDur[entry.Key] = newEf.duration;
				return true;
			}
		}
		return false;
	}
}