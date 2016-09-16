using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[ExecuteInEditMode]
[System.Serializable]
public class MapConstructor : MonoBehaviour
{
	public float pointSize = 1f;

	public float maxPointSize = 2f;

	public Color baseColor = Color.gray;

	public Color pathColor = Color.gray;

	public Color wayPointColor = Color.gray;

	public Color towerPointColor = Color.gray;

	[SerializeField] private MapData map;

	public MapData Map {
		get {
			return this.map;
		}
		set {
			map = value;
		}
	}
}
	