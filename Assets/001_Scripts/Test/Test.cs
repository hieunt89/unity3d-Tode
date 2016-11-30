using UnityEngine;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour {
	DateTime mTime;

//	void Start(){
//		mTime = DateTime.Now;
//		for (int i = 0; i < 10; i++) {
//			Debug.Log (mTime.ToString ());
//			Debug.Log (mTime.GetHashCode ());
//
//		}
//	}
	void Update () {
		if(Input.GetMouseButton(0)) {
			Guid x = Guid.NewGuid ();
			Debug.Log (x.ToString());
		}
	}
}
