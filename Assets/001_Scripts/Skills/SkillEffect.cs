using UnityEngine;
using System.Collections;

public enum Effect{
	Root,
	Stun,
	HpReduce,
	HpBuff,
	MoveSpeedSlow,
	ArmorReduce,
	ArmorBuff
}

public class SkillEffect {
	public string skillId;
	public Effect effect;
	public float value;
	public float duration;
	public float interval;
}
