using UnityEngine;

public class InputController : MonoBehaviour {

	void Update() {
		var input = UnityEngine.Input.GetMouseButton (0);
		if (input) {
			var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero, 100);
			if (hit.collider != null) {
				var pos = hit.collider.transform.position;
//				if (!Pools.pool.HasEntity 
//				Pools.pool.CreateEntity()
//					.AddInput((int)pos.x, (int)pos.y);
			}
		} 
	}
}