using UnityEngine;

[System.Serializable]
public class SummonSkillData : SkillData {
	public float cooldown;
	public float castRange;
	public float castTime;
	public string summonId;
	public int summonCount;
	public float duration;

	public SummonSkillData ()
	{
	}
	
	public SummonSkillData (string _skillId)
	{
		id = _skillId;
	}
	
}
