using System.Collections.Generic;

[System.Serializable]
public class PathData {
	[UnityEngine.SerializeField] public int pathId;
	[UnityEngine.SerializeField] public List<WayPointData> points;

	public PathData (int _pathId, List<WayPointData> _points){
		pathId = _pathId;
		points = _points;
	}
}
