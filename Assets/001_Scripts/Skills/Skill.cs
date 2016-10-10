using UnityEngine;
using System.Collections;

public enum SkillType{
	None,
	Combat,
	Summon
}

public abstract class Skill {
	public string id;
	public string name;
	public float cooldown;
	public float castRange;
	public int expToNextLvl;
	public int cost;
}
