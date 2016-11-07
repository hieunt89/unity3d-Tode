using UnityEngine;
using System.Collections;
using Entitas;

public class View : IComponent {
	public GameObject go;

	Animator[] anim;
	public Animator[] Anim{
		get{
			return anim != null ? anim : go.GetComponentsInChildren<Animator>();
		}
	}

	Collider collider;
	public Collider Collider{
		get{
			return collider != null ? collider : go.GetComponent<Collider> ();
		}
	}
}
