using UnityEngine;
using System.Collections;
using Entitas;

public class CoroutineQueueSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.hasCoroutineQueue && e.coroutineQueue.Queue.Count > 0) {
				e.AddCoroutineTask (e.coroutineQueue.Queue.Dequeue());
			}
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Coroutine.OnEntityRemoved ();
		}
	}
	#endregion
}
