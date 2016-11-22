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
		for (int i = 0; i < entities.Count; i++) {
			entities [i].AddCoroutineTask(CreateProjectileView(entities [i]));
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.ProjectileMark).OnEntityAdded ();
		}
	}
	#endregion

	IEnumerator CreateProjectileView(Entity e){
		var r = Resources.LoadAsync<GameObject> (ConstantString.ResourcesPrefab + e.projectile.projectileId);
		while(!r.isDone){
			yield return null;
		}

		if(r.asset == null){
			if (GameManager.debug) {
				Debug.Log ("Fail to load asset " + e.projectile.projectileId + " from Resources");
			}
			yield break;
		}
			
		GameObject go = Lean.LeanPool.Spawn ( r.asset as GameObject );
		go.transform.position = e.position.value;
		go.transform.SetParent (projectileViewParent.transform, false);

		e.AddView (go);

	}
}
