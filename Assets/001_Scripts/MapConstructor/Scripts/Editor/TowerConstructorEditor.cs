using UnityEditor;

[CustomEditor (typeof(TowerConstructor))]
public class TowerConstructorEditor : Editor {

	public override void OnInspectorGUI (){
		DrawDefaultInspector();
	}
}
