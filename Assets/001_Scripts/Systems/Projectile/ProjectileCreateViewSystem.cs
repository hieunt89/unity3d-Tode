using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileCreateViewSystem : IReactiveSystem {
	#region Constructor
	GameObject projectileViewParent;
	public ProjectileCreateViewSystem(){
		projectileViewParent = GameObject.Find ("ProjectileView");
		if(projectileViewParent == null){
			projectileViewParent = new GameObject ("ProjectileView");
			projectileViewParent.transform.position = Vector3.zero;
		}
	}
	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		GameObject go;
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			go = Lean.LeanPool.Spawn (Resources.Load<GameObject> ("Projectile/" + e.projectile.projectileId));
			go.transform.position = e.position.value;
			go.transform.rotation = Quaternion.LookRotation(e.destination.value - e.position.value);
			go.transform.SetParent (projectileViewParent.transform, false);
			e.AddView (go);
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.ProjectileMark).NoneOf(Matcher.MarkedForNoView).OnEntityAdded ();
		}
	}
	#endregion


}
