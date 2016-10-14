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
			List<Vector3> wayPoints = new List<Vector3> ();
			wayPoints.Add (GetPoint (0f));
			int stepPerCurve = 10; // this is a const data
			int steps = stepPerCurve * CurveCount;

			for (int stepIndex = 1; stepIndex <= steps; stepIndex++) {
				wayPoints.Add (GetPoint(stepIndex / (float)steps));
			}
			return wayPoints;
		}
	}

	public PathData (int _id, string _pathId){
		id = _pathId;
		controlPoints = new List<Vector3> ();
		controlPoints.Add(new Vector3(1f, _id, 0f));
		controlPoints.Add(new Vector3(2f, _id, 0f));
		controlPoints.Add(new Vector3(3f, _id, 0f));
		controlPoints.Add(new Vector3(4f, _id, 0f));
	}

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

	public Vector3 GetControlPoint (int _index) {
		return controlPoints[_index];
	}

	public void SetControlPoint (int _index, Vector3 _point) {
		if (_index % 3 == 0) {
			Vector3 delta = _point - controlPoints[_index];

			if (_index > 0) {
				controlPoints[_index - 1] += delta;
			}
			if (_index + 1 < controlPoints.Count) {
				controlPoints[_index + 1] += delta;
			}
		}
		controlPoints[_index] = _point;
		EnforceMode(_index);
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

	public void AddCurve () {
		if (controlPoints.Count >= 4) {
			Vector3 point = controlPoints [controlPoints.Count - 1];
			point.x += 1f;
			controlPoints.Add (point);
			point.x += 1f;
			controlPoints.Add (point);
			point.x += 1f;
			controlPoints.Add (point);

			EnforceMode(controlPoints.Count - 4);
		}
	}

	private void EnforceMode (int index) {
		int middleIndex =  ((index + 1) / 3) * 3;

		if (middleIndex == 0 || middleIndex == ControlPointCount - 1) return;

		int fixedIndex, enforcedIndex;

		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			enforcedIndex = middleIndex + 1;
		}
		else {
			fixedIndex = middleIndex + 1;
			enforcedIndex = middleIndex - 1;
		}

		Vector3 middle = controlPoints[middleIndex];
		Vector3 enforcedTangent = middle - controlPoints[fixedIndex];
		enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, controlPoints[enforcedIndex]);
		controlPoints[enforcedIndex] = middle + enforcedTangent;
	}
}
