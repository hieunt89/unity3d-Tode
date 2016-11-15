using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;

public class Test : MonoBehaviour  {

	void Start () {
		string [] names = Enum.GetNames(typeof (TreeType));
	}
}
