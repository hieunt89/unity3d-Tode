using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PathData {
	[SerializeField] public string id;
	[SerializeField] private List<Vector3> controlPoints;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public List<Vector3> ControlPoints {
		get {
			return controlPoints;
		}
		set {
			controlPoints = value;
		}
	}
	public List<Vector3> WayPoints {
		get {
			// TODO: convert control points to waypoints
			return new List<Vector3> ();
		}
	}

	public PathData (string _pathId){
		id = _pathId;
		controlPoints = new List<Vector3> ();
		controlPoints.Add(new Vector3(1f, 0f, 0f));
		controlPoints.Add(new Vector3(2f, 0f, 0f));
		controlPoints.Add(new Vector3(3f, 0f, 0f));
		controlPoints.Add(new Vector3(4f, 0f, 0f));
	}

	public PathData (string _pathId, List<Vector3> _points){
		id = _pathId;
		controlPoints = _points;
	}

//	[SerializeField] private Vector3[] points;

	public int CurveCount {
		get {
			return (controlPoints.Count - 1) / 3;
		}
	}

	public int ControlPointCount {
		get {
			return controlPoints.Count;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return controlPoints[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - controlPoints[index];

			if (index > 0) {
				controlPoints[index - 1] += delta;
			}
			if (index + 1 < controlPoints.Count) {
				controlPoints[index + 1] += delta;
			}
		}
		controlPoints[index] = point;
		EnforceMode(index);
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = controlPoints.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return Bezier.GetPoint(
			controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t);
	}

	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = controlPoints.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return Bezier.GetFirstDerivative(
			controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t);
	}

	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	// TODO: move to map editor or Bezier 
//	public void AddCurve () {
//		Vector3 point = controlPoints[controlPoints.Count - 1];
////		Array.Resize(ref points, points.Length + 3);
//		point.x += 1f;
////		points[points.Length - 3] = point;
//		controlPoints.Add (point);
//		point.x += 1f;
////		points[points.Length - 2] = point;
//		controlPoints.Add (point);
//		point.x += 1f;
////		points[points.Length - 1] = point;
//		controlPoints.Add (point);
//
//		//		Array.Resize(ref modes, modes.Length + 1);
//		//		modes[modes.Length - 1] = modes[modes.Length - 2];
//		EnforceMode(controlPoints.Count - 4);
//	}

//	public void Reset () {
////		points = new Vector3[] {
////			new Vector3(1f, 0f, 0f),
////			new Vector3(2f, 0f, 0f),
////			new Vector3(3f, 0f, 0f),
////			new Vector3(4f, 0f, 0f)
////		};
//		controlPoints = new List<Vector3> ();
//		controlPoints.Add(new Vector3(1f, 0f, 0f));
//		controlPoints.Add(new Vector3(2f, 0f, 0f));
//		controlPoints.Add(new Vector3(3f, 0f, 0f));
//		controlPoints.Add(new Vector3(4f, 0f, 0f));
//
//		//		modes = new BezierControlPointMode[] {
//		//			BezierControlPointMode.Free,
//		//			BezierControlPointMode.Free
//		//		};
//	}

	//	public BezierControlPointMode GetControlPointMode (int index) {
	//		return modes[(index + 1) / 3];
	//	}
	//
	//	public void SetControlPointMode (int index, BezierControlPointMode mode) {
	//		modes[(index + 1) / 3] = mode;
	//		EnforceMode(index);
	//	}

	// TODO: move to map editor or Bezier 
	private void EnforceMode (int index) {
		//		int modeIndex = (index + 1) / 3;

		//		if (modeIndex == 0 || modeIndex == CurveCount) return;
		//		BezierControlPointMode mode = modes[modeIndex];
		//		if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1) {
		//			return;
		//		}

		//		int middleIndex = modeIndex * 3;
		int middleIndex =  ((index + 1) / 3) * 3;

		if (middleIndex == 0 || middleIndex == ControlPointCount - 1) return;

		int fixedIndex, enforcedIndex;

		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			//			if (fixedIndex < 0) {
			//				fixedIndex = points.Length - 2;
			//			}
			enforcedIndex = middleIndex + 1;
			//			if (enforcedIndex >= points.Length) {
			//				enforcedIndex = 1;
			//			}
		}
		else {
			fixedIndex = middleIndex + 1;
			//			if (fixedIndex >= points.Length) {
			//				fixedIndex = 1;
			//			}
			enforcedIndex = middleIndex - 1;
			//			if (enforcedIndex < 0) {
			//				enforcedIndex = points.Length - 2;
			//			}
		}

		Vector3 middle = controlPoints[middleIndex];
		Vector3 enforcedTangent = middle - controlPoints[fixedIndex];
		//		if (mode == BezierControlPointMode.Aligned) {
		enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, controlPoints[enforcedIndex]);
		//		}
		controlPoints[enforcedIndex] = middle + enforcedTangent;

	}
}
