using UnityEngine;
using System.Collections;

public class UnityInput : IInput {
	#region IInput implementation
	public float GetXMove ()
	{
		return Input.GetAxis ("Horizontal");
	}

	public float GetYMove ()
	{
		return Input.GetAxis ("Vertical");
	}
	#endregion
}
