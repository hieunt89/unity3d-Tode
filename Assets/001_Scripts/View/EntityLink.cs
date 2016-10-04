using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EntityLink {
	private static EntityLink instance = null;
	public static EntityLink Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EntityLink();
			}
			return instance;
		}
	}

	Dictionary<GameObject, Entity> goToEntity;

	public EntityLink(){
		goToEntity = new Dictionary<GameObject, Entity> ();
	}

	public Entity GetEntity(GameObject go){
		if (goToEntity.ContainsKey (go)) {
			return goToEntity [go];
		} else {
			return null;
		}
	}

	public void AddLink(GameObject go, Entity e){
		goToEntity.Add (go, e);
	}

	public bool RemoveLink(GameObject go){
		if (goToEntity.ContainsKey (go)) {
			goToEntity.Remove (go);
			return true;
		} else {
			return false;
		}
	}
}
