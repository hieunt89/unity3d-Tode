using UnityEngine;
using System.Collections;

public class ItemListView : ViewBase {

	public ItemListView () : base () {
		Debug.Log ("construct item list view");
	}
	public override void UpdateView ()
	{
		base.UpdateView ();
		Debug.Log ("update item list view");


	}

	public override void ProcessEvent ()
	{
		base.ProcessEvent ();
	}
}
