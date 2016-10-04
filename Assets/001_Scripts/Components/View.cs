using UnityEngine;
using System.Collections;
using Entitas;

public class View : IComponent {
	public GameObject go;

	public Animator Anim{
		get{ 
			return go.GetComponent<Animator>();	
		}
	}

	public Bounds ColliderBound{
		get{ 
			return go.GetComponent<Collider> ().bounds;
		}
	}
}
