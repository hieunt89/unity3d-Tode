using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor {

	private const float directionScale = 0.5f;

	private BezierSpline spline;
	private Transform handleTransform;
	private Quaternion handleRotation;

	public override void OnInspectorGUI () {
//		DrawDefaultInspector();
		spline = target as BezierSpline;

		if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount) {
			DrawSelectedPointInspector();
		}

		if (GUILayout.Button("Add Curve")) {
			Undo.RecordObject(spline, "Add Curve");
			spline.AddCurve();
			EditorUtility.SetDirty(spline);
		}
	}

	private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Move Point");
			EditorUtility.SetDirty(spline);
			spline.SetControlPoint(selectedIndex, point);
		}

//		EditorGUI.BeginChangeCheck();
//		BezierControlPointMode mode = (BezierControlPointMode) EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
//		if (EditorGUI.EndChangeCheck()) {
//			Undo.RecordObject(spline, "Change Point Mode");
//			spline.SetControlPointMode(selectedIndex, mode);
//			EditorUtility.SetDirty(spline);
//		}
	}

	private void OnSceneGUI () {
		spline = target as BezierSpline;
		handleTransform = spline.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		Vector3 p0 = ShowPoint(0);
		for (int i = 1; i < spline.ControlPointCount; i += 3) {
			Vector3 p1 = ShowPoint(i);
			Vector3 p2 = ShowPoint(i + 1);
			Vector3 p3 = ShowPoint(i + 2);

			Handles.color = Color.gray;
			Handles.DrawLine(p0, p1);
			Handles.DrawLine(p2, p3);

//			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

			Handles.color = Color.white;
			Vector3 lineStart = spline.GetPoint(0f);
			Handles.color = Color.green;
			Handles.DrawLine(lineStart, lineStart + spline.GetDirection(0f));
			int steps = stepsPerCurve * spline.CurveCount;
			for (int j = 1; j <= steps; j++) {
				Vector3 lineEnd = spline.GetPoint(j / (float)steps);
				Handles.color = Color.white;
				Handles.DrawLine(lineStart, lineEnd);
				Handles.color = Color.green;
				Handles.DrawLine(lineEnd, lineEnd + spline.GetDirection(j / (float)steps));
				lineStart = lineEnd;
			}
			p0 = p3;
		}

		ShowDirections();
	}
	private const int stepsPerCurve = 10;
	private void ShowDirections () {
		Handles.color = Color.green;
		Vector3 point = spline.GetPoint(0f);
		Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
		int steps = stepsPerCurve * spline.CurveCount;
		for (int i = 1; i <= steps; i++) {
			point = spline.GetPoint(i / (float)steps);
			Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
		}
	}

	private const float handleSize = 0.1f;
	private const float pickSize = 0.1f;

	private int selectedIndex = -1;


//	private static Color[] modeColors = {
//		Color.white,
//		Color.yellow,
//		Color.cyan
//	};

	private Vector3 ShowPoint (int index) {
		Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));

		float size = HandleUtility.GetHandleSize(point);
		if (index == 0) {
			size *= 1.25f;
		}

//		Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
		Handles.color = Color.yellow;
//		if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap)) {
//			selectedIndex = index;
//			Repaint();
//		}
//		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck();
//			point = Handles.DoPositionHandle(point, handleRotation);
			point = Handles.FreeMoveHandle(point, Quaternion.identity, .2f, Vector3.one, Handles.CircleCap);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(spline, "Move Point");
				EditorUtility.SetDirty(spline);
				spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
			}
//		}
		return point;
	}
}