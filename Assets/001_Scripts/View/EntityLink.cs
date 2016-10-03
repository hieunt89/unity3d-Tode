using UnityEngine;
using System.Collections;
using Entitas;

public class EntityLink : MonoBehaviour {
	Entity link;

	public Entity Link {
		get {
			return link;
		}
	}

	public void RegisterLink(Entity e){
		this.link = e;
	}
}
