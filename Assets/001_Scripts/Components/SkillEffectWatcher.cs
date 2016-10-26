using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGenerator;

public class SkillEffectWatcher : IComponent {
	public Entity target;
	public SkillEffect effect;

	float changes;
	public float GetChanges(){
		return changes;
	}
	public void SetChanges(float value){
		changes = value;
	}
}
