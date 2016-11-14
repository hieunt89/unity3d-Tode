using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyWatchHpSystem : ISetPool, IReactiveSystem, IEnsureComponents {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}
	#endregion

	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf(Matcher.Hp, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.isWound == false && (e.hp.value < e.hpTotal.value)) {
				e.IsWound (true);
			} else {
				e.IsWound (false);
			}

			if(e.hp.value <= 0){
				if(e.hasGold){
					_pool.ReplaceGoldPlayer (_pool.goldPlayer.value + e.gold.value);
				}
				e.IsActive(false).IsAttackable(false).IsTargetable(false).IsInteractable(false).IsMovable(false);
				if (e.hasCoroutineQueue) {
					e.RemoveCoroutineQueue ();
				}
				if (e.hasCoroutine) {
					e.RemoveCoroutine ();
				}
				if(e.hasViewSlider){
					Lean.LeanPool.Despawn (e.viewSlider.bar.gameObject);
				}
				e.AddCoroutineTask (StartDying (e));
			}

		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Enemy, Matcher.Hp).OnEntityAdded ();
		}
	}
	#endregion

	IEnumerator StartDying(Entity e){
		if (!e.hasDyingTime) {
			e.IsMarkedForDestroy (true);
			yield break;
		}

		e.AddDying (0f);
		while(e.dying.timeSpent < e.dyingTime.value){
			e.ReplaceDying (e.dying.timeSpent + _pool.tick.change);
			yield return null;
		}

		e.IsMarkedForDestroy (true);
	}
}
