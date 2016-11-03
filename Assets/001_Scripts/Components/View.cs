using UnityEngine;
using System.Collections;
using Entitas;

public class View : IComponent {
	public GameObject go;

	Animator[] anim;
	public Animator[] Anim{
		get{ 
			if (anim == null) {
				anim = go.GetComponentsInChildren<Animator>();
			}
			return anim;
		}
	}

	Bounds bounds;
	public Bounds ColliderBound{
		get{ 
			if (bounds == null) {
				bounds = go.GetComponent<Collider> ().bounds;
			}
			return bounds;
		}
	}
}
