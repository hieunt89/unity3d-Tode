using UnityEngine;
using System.Collections;
using Entitas;

public class SkillEffectWatch : IComponent {
	public Entity target;
	public float duration;
	public float interval;
	public SkillEffect effect;
}
