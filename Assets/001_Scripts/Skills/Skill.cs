using UnityEngine;
using System.Collections;

public enum SkillType{
	Summon,
	Combat
}

public abstract class Skill {
	public string id;
	public float cooldown;
	public float range;
}
