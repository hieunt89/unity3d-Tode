using UnityEngine;
using System.Collections;

public enum Effect{
	Root,
	Stun,
	MoveSpeedSlow,
	ArmorReduce
}

public class SkillEffect {
	public string skillId;
	public Effect effect;
	public float value;
	public float duration;
}
