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
		var paths = DataManager.Instance.GetMapData ("map0").Paths;
		if(paths == null){
			return;
		}

		for (int i = 0; i < paths.Count; i++) {
			var path = paths [i];
			List <Vector3> wayPoints = new List<Vector3> ();
			List <float> pathLength = new List<float> ();
			for (int j = 0; j < path.Points.Count; j++) {
				wayPoints.Add (path.Points [j]);
				if(j > 0){
					var length = (path.Points[j] - path.Points[j - 1]).magnitude;
					pathLength.Add (length);
				}
			}

			if (wayPoints.Count > 0) {
				_pool.CreateEntity ().AddPath (wayPoints).AddId (path.Id).AddPathLength(pathLength);
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