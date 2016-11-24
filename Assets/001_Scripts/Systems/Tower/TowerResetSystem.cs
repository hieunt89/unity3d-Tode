using UnityEngine;
using System.Collections;
using Entitas;
public class TowerResetSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.hasAttacking) {
				e.RemoveAttacking ();
			}
			if (e.hasAttackCooldown) {
				e.RemoveAttackCooldown ();
			}
			if (e.hasCoroutineQueue) {
				e.RemoveCoroutineQueue ();
			}
			if (e.hasCoroutine) {
				e.RemoveCoroutine ();
			}
			if (e.hasSkillEntityRefs) {
				for (int j = 0; j < e.skillEntityRefs.skillEntities.Count; j++) {
					e.skillEntityRefs.skillEntities [j].IsActive(false).IsMarkedForDestroy (true);
				}
				e.RemoveSkillEntityRefs ();
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
