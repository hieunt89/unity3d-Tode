using UnityEngine;

[System.Serializable]
public class SummonSkillData : SkillData {
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
