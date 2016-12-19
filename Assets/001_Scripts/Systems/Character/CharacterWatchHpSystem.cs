using UnityEngine;
using System.Collections;
using Entitas;

public class CharacterWatchHpSystem : ISetPool, IReactiveSystem, IEnsureComponents {
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
			return Matcher.AllOf(Matcher.HpTotal, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.hp.value < e.hpTotal.value) {
				if (!e.isWound) {
					e.IsWound (true);
				}
			} else {
				if (e.isWound) {
					e.IsWound (false);
				}
			}

			if(e.hp.value <= 0){
				Die (e);
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Hp).OnEntityAdded ();
		}
	}
	#endregion

	void Die(Entity e){
		e.IsActive(false).IsAttackable(false).IsDamageable(false).IsTargetable(false).IsInteractable(false).IsMovable(false);
		if(e.hasEnemy && e.hasGold){
			_pool.ReplaceGoldPlayer (_pool.goldPlayer.value + e.gold.value);
		}
		if (e.hasCoroutineQueue) {
			e.RemoveCoroutineQueue ();
		}
		if (e.hasCoroutine) {
			e.RemoveCoroutine ();
		}
		if(e.hasViewSlider){
			Lean.LeanPool.Despawn (e.viewSlider.bar.gameObject);
			e.RemoveViewSlider ();
		}
		e.AddCoroutineTask (StartDying (e));
	}

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
