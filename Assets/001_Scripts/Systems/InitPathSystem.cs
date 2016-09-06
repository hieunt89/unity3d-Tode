using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class InitPathSystem : IInitializeSystem, ISetPool {
	Pool _pool;

	#region ISetPool implementation
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}
	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		var paths = GameObject.FindGameObjectsWithTag ("Path");

		for (int i = 0; i < paths.Length; i++) {

			List <Vector3> wayPoints = new List<Vector3> ();

			for (int j = 0; j < paths[i].transform.childCount; j++) {
				wayPoints.Add (paths [i].transform.GetChild (j).position);
			}

			if (wayPoints.Count > 0) {
				_pool.CreateEntity ().AddPath (wayPoints).AddId ("path_"+i);
			}
		}
	}
	#endregion
}