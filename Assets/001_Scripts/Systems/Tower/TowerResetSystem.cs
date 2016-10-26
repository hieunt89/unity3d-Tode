using UnityEngine;
using System.Collections;
using Entitas;
public class TowerResetSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.isAttacking) {
				e.IsAttacking (false);
			}
			if (e.hasAttackCooldown) {
				e.RemoveAttackCooldown ();
			}
			if (e.hasCoroutine) {
				e.RemoveCoroutine ();
			}
			if (e.hasSkillEntityList) {
				for (int j = 0; j < e.skillEntityList.skillEntities.Count; j++) {
					e.skillEntityList.skillEntities [j].IsActive(false).IsMarkedForDestroy (true);
				}
				e.RemoveSkillEntityList ();
			}

			e.IsTowerReset (false);
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.TowerReset).OnEntityAdded();
		}
	}
	#endregion
	
}
