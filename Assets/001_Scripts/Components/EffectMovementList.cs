using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EffectMovementList : IComponent {
	public Dictionary<SkillEffect, float> efToDuration;
}
