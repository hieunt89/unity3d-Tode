using UnityEngine;
using System.Collections;
using Entitas;
using Lean;

public class MapNavigationSystem : IInitializeSystem, ITearDownSystem {
	Transform camera;
	#region IInitializeSystem implementation
	public void Initialize ()
	{
		LeanTouch.OnSoloDrag += SoloDrag;
		camera = Camera.main.transform;
	}
	#endregion

	#region ITearDownSystem implementation

	public void TearDown ()
	{
		LeanTouch.OnSoloDrag -= SoloDrag;
	}

	#endregion

	void SoloDrag(Vector2 deltaDrag){
		if(LeanTouch.GuiInUse){
			return;
		}

		var camOnPlane = Vector3.ProjectOnPlane (camera.position, Vector3.up);
		var camDisFromPlane = Vector3.Project (camera.position, Vector3.down).magnitude;
		var camForwardOnPlane = Vector3.ProjectOnPlane (camera.forward, Vector3.up);

		Debug.DrawLine (camOnPlane, camForwardOnPlane, Color.red, Mathf.Infinity);

		if (Mathf.Abs(deltaDrag.x) > Mathf.Abs(deltaDrag.y)) {
			//move vertical

		}else{
			//move horizontal

		}
	}

	bool CheckCamera(Vector3 newPos){
		RaycastHit hit;
		if (Physics.Raycast (newPos, camera.forward, out hit)) {
			return true;
		} else {
			return false;
		}
	}
}
