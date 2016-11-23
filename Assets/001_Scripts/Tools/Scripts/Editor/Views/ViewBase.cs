using UnityEngine;
using System.Collections;

public class ViewBase {
	public Rect viewRect;

	public ViewBase ()
	{
		Debug.Log ("construct view base");
	}
	
	public virtual void UpdateView () {

	}
	public virtual void ProcessEvent () {

	}
}
