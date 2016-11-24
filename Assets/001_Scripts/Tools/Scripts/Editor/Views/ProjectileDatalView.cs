using UnityEngine;
using System.Collections;
using UnityEditor;

public class ProjectileDatalView : ViewBase {
	int projectileindex = 1;

	public override void UpdateView ()
	{
		base.UpdateView ();

		Debug.Log ("update projectile detail view");


		GUI.SetNextControlName ("DummyFocus");
		GUI.Button (new Rect (0,0,0,0), "", GUIStyle.none);

		GUILayout.BeginHorizontal ("box");

		if (GUILayout.Button("<", GUILayout.ExpandWidth(false))) 
		{
			if (projectileindex > 1)
			{	
				projectileindex --;
				GUI.FocusControl ("DummyFocus");
			}

		}
		if (GUILayout.Button(">", GUILayout.ExpandWidth(false))) 
		{
			if (projectileindex < currentWindow.projectileList.projectiles.Count) 
			{
				projectileindex ++;
				GUI.FocusControl ("Dummy");
			}
		}

		GUILayout.Space(100);

		if (GUILayout.Button("Add", GUILayout.ExpandWidth(false))) 
		{
//			AddProjectileData();
		}
		if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false))) 
		{
//			DeleteProjectileData (projectileindex - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button("Back", GUILayout.ExpandWidth(false))) 
		{
//			viewIndex = 0;
		}
		GUILayout.EndHorizontal ();


		if (currentWindow.projectileList.projectiles == null)
			Debug.Log("wtf");
		if (currentWindow.projectileList.projectiles.Count > 0) 
		{
			GUILayout.BeginHorizontal ();
			projectileindex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", projectileindex, GUILayout.ExpandWidth(false)), 1, currentWindow.projectileList.projectiles.Count);
			//Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
			EditorGUILayout.LabelField ("of   " +  currentWindow.projectileList.projectiles.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal ();
			GUILayout.Space(10);

			EditorGUILayout.LabelField ("id",  currentWindow.projectileList.projectiles[projectileindex-1].intId.ToString());
			currentWindow.projectileList.projectiles[projectileindex-1].Name = EditorGUILayout.TextField ("Name", currentWindow.projectileList.projectiles[projectileindex-1].Name);
			currentWindow.projectileList.projectiles[projectileindex-1].View = (GameObject) EditorGUILayout.ObjectField ("Projectile GO", currentWindow.projectileList.projectiles[projectileindex-1].View, typeof(GameObject), true);
			currentWindow.projectileList.projectiles[projectileindex-1].Type = (ProjectileType) EditorGUILayout.EnumPopup ("Type", currentWindow.projectileList.projectiles[projectileindex-1].Type);

			if (currentWindow.projectileList.projectiles[projectileindex-1].Type == ProjectileType.homing) {
				currentWindow.projectileList.projectiles[projectileindex-1].Duration = 0f;
			}

			GUI.enabled = currentWindow.projectileList.projectiles[projectileindex-1].Type == ProjectileType.homing;
			currentWindow.projectileList.projectiles[projectileindex-1].TravelSpeed = EditorGUILayout.FloatField ("Travel Speed", currentWindow.projectileList.projectiles[projectileindex-1].TravelSpeed);
			GUI.enabled = true;

			GUI.enabled = currentWindow.projectileList.projectiles[projectileindex-1].Type == ProjectileType.throwing || currentWindow.projectileList.projectiles[projectileindex-1].Type == ProjectileType.laser;
			currentWindow.projectileList.projectiles[projectileindex-1].Duration = EditorGUILayout.FloatField ("Duration", currentWindow.projectileList.projectiles[projectileindex-1].Duration);
			GUI.enabled = true;

			GUI.enabled = currentWindow.projectileList.projectiles[projectileindex-1].Type == ProjectileType.laser;
			currentWindow.projectileList.projectiles[projectileindex-1].MaxDmgBuildTime = EditorGUILayout.FloatField ("Time to reach maxDmg", currentWindow.projectileList.projectiles[projectileindex-1].MaxDmgBuildTime);
			currentWindow.projectileList.projectiles[projectileindex-1].TickInterval = EditorGUILayout.FloatField ("Tick interval", currentWindow.projectileList.projectiles[projectileindex-1].TickInterval);
			GUI.enabled = true;

			GUILayout.Space(10);

		} 
		else 
		{
			GUILayout.Label ("This Projectile List is Empty.");
		}
	}

	public override void ProcessEvent ()
	{
		base.ProcessEvent ();
	}
}
