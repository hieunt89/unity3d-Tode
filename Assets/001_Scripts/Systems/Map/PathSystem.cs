using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class PathSystem : IInitializeSystem, ISetPool{
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
			List <float> pathLength = new List<float> ();
			for (int j = 0; j < paths[i].transform.childCount; j++) {
				wayPoints.Add (paths [i].transform.GetChild (j).position);
				if(j > 0){
					var length = (paths [i].transform.GetChild (j).position - paths [i].transform.GetChild (j - 1).position).magnitude;
					pathLength.Add (length);
				}
			}

			if (wayPoints.Count > 0) {
				_pool.CreateEntity ().AddPath (wayPoints).AddId ("path_"+i).AddPathLength(pathLength);
			}

			if(GameManager.debug){
				for (int k = 0; k < wayPoints.Count - 1; k++) {
					Debug.DrawLine (wayPoints [k], wayPoints [k + 1], Color.green, Mathf.Infinity);
				}
			}
		}
	}
	#endregion
}