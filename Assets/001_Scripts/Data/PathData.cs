using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PathData {
	[SerializeField] public string id;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	[SerializeField] private List<Vector3> points;

	public List<Vector3> Points {
		get {
			return points;
		}
		set {
			points = value;
		}
	}

	public PathData (string _pathId, List<Vector3> _points){
		id = _pathId;
		points = _points;
	}
}
