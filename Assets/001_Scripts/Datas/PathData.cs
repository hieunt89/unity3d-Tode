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

	[UnityEngine.SerializeField] private List<WayPointData> points;

	public List<WayPointData> Points {
		get {
			return points;
		}
		set {
			points = value;
		}
	}

	public PathData (int _pathId, List<WayPointData> _points){
		pathId = _pathId;
		points = _points;
	}
}
