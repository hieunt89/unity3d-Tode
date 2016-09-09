using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor {
	SerializedProperty lookAtPoint;

    void OnEnable () {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI () {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI () {
        var t = (target as LookAtPoint);
        EditorGUI.BeginChangeCheck();

        Vector3 pos = Handles.PositionHandle(t.lookAtPoint, Quaternion.identity);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Move point");
            t.lookAtPoint = pos;
            t.Update ();
        }
    }
}
