using UnityEngine;
using System.Collections;

public enum SkillType{
	None,
	Combat,
	Summon
}

public class SkillData {
	public string id;
	public int intId;
	public string name;
	public string description;
	public string spritePath;
//	public SkillType type;
	public int goldCost;

	public SkillData ()
	{
	}
	
//	public SkillData (string id, string name, string description, string spritePath, SkillType type, int goldCost)
//	{
//		this.id = id;
//		this.name = name;
//		this.description = description;
//		this.spritePath = spritePath;
//		this.type = type;
//		this.goldCost = goldCost;
//	}
	
}
