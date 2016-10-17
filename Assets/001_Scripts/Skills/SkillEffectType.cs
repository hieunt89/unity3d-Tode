using UnityEngine;
using System.Collections;

public enum EffectType{
	Root,
	Stun,
	MoveSpeedSlow,
	ArmorReduce
}

 public class SkillEffect {
	public string skillId;
	public EffectType effectType;
	public float value;
	public float duration;

	public SkillEffect ()
 	{
 	}
 	
}
