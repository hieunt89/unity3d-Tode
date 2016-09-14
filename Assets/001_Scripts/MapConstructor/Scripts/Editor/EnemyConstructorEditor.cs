using UnityEditor;

[CustomEditor (typeof(EnemyConstructor))]
public class EnemyConstructorEditor : Editor {

	public override void OnInspectorGUI (){
		DrawDefaultInspector();
	}
}
