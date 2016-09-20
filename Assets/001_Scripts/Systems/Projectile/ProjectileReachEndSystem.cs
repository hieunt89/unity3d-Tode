using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class ProjectileReachEndSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			if(e.target.e.hasEnemy){
				DamageEnemy (e, e.target.e);
			}

			e.IsMarkedForDestroy (true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Projectile, Matcher.ReachedEnd).NoneOf(Matcher.Tower).OnEntityAdded();
		}
	}

	#endregion

	void DamageEnemy(Entity projectile, Entity enemy){
		int damage = Random.Range (projectile.attackDamage.minDamage, projectile.attackDamage.maxDamage);
		float reduction = GetDamageReduction (projectile.attack.attackType, enemy.armor.armorList);
		damage = Mathf.CeilToInt(damage * reduction);
		int hpLeft = Mathf.Clamp(enemy.hp.value - damage, 0, enemy.hpTotal.value);
		enemy.ReplaceHp (hpLeft);
	}

	float GetDamageReduction(AttackType atkType, List<ArmorData> armors){
		float result = 0f;
		for (int i = 0; i < armors.Count; i++) {
			if (armors [i].Type == atkType) {
				result = 1 - armors[i].Reduction*0.01f;
				break;
			}
		}
		return result;
	}
}
