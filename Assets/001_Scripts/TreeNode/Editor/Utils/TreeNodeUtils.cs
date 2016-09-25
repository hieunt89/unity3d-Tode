using UnityEngine;
using UnityEditor;

public static class TreeNodeUtils {

	public static void DrawGrid (Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor) {
		int widthDivs = Mathf.CeilToInt (viewRect.width - gridSpacing);
		int heightDivs = Mathf.CeilToInt (viewRect.height - gridSpacing);

		Handles.BeginGUI ();
		Handles.color = gridColor;

		for (int x = 0; x < widthDivs; x++) {
			Handles.DrawLine (new Vector3 (gridSpacing * x, 0f, 0f), new Vector3 (gridSpacing * x, viewRect.height, 0f));
		}

		for (int y = 0; y < widthDivs; y++) {
			Handles.DrawLine (new Vector3 (0f, gridSpacing * y, 0f), new Vector3 (viewRect.width, gridSpacing * y, 0f));
		}
		Handles.color = Color.white;
		Handles.EndGUI ();
	}
}
