using System.Collections.Generic;
using UnityEngine;

public class CombatUtility{
	public static int RandomDamage(int maxDmg, int minDmg, AttackType atkType, List<ArmorData> enemyArmors){
		int damage = Random.Range (minDmg, maxDmg);
		float reduction = GetDamageReduction (atkType, enemyArmors);
		damage = Mathf.CeilToInt(damage * reduction);
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
}