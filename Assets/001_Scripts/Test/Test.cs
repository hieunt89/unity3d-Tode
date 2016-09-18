using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
	public List<Dictionary<int, string>> enemyIndexes;

	void Start () {
		var path = "Data/TowerData";
		TextAsset[] files = Resources.LoadAll <TextAsset> (path) as TextAsset[];
		Debug.Log (path);
		Debug.Log (files.Length);
//		Texture2D[] tex = Resources.LoadAll <Texture2D> (path) as Texture2D[];
//		Debug.Log (path);
//		Debug.Log (tex);
	}
}
