using UnityEngine;
using System.Collections;

public enum SkillType{
	None,
	Combat,
	Summon
}

public abstract class SkillData {
	public string id;
	public string name;
	public float cooldown;
	public float castRange;
	public float castTime;
	public int goldCost;

	public static readonly NullSkillData Null = new NullSkillData();
	public class NullSkillData : SkillData{
		public NullSkillData(){
			goldCost = Mathf.Infinity;
		}
	}
}
