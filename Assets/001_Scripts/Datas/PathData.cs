using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PathData {
	[UnityEngine.SerializeField] private int pathId;

	public int PathId {
		get {
			return pathId;
		}
		set {
			pathId = value;
		}
	}

	[UnityEngine.SerializeField] private List<Vector3> points;

	public List<Vector3> Points {
		get {
			return points;
		}
		set {
			points = value;
		}
	}

	public PathData (int _pathId, List<Vector3> _points){
		pathId = _pathId;
		points = _points;
	}
}
