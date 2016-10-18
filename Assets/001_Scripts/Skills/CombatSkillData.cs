using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CombatSkillData : SkillData {
	public int projectileIndex;
	public string projectileId;
	public float aoe;
	public AttackType attackType;
	public int damage;
	public List<SkillEffect> effectList;

	public CombatSkillData ()
	{
	}

	public CombatSkillData (string _skillId)
	{
		id = _skillId;
	}
	
}
