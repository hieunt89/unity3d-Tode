using UnityEngine;
using System.Collections;
using Entitas;

public class SkillEffectWatcher : IComponent {
	public Entity target;
	public SkillEffect effect;

	float changes;
	public float Changes {
		get {
			return changes;
		}set{ 
			changes = value;
		}
	}
}
