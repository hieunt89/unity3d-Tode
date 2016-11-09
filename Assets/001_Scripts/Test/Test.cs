using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class Test : MonoBehaviour {
	public GameObject nestedCube;

	void Start () {
		Debug.Log (transform.InverseTransformPoint(nestedCube.transform.position));
	}
}
