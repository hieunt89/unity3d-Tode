using UnityEngine;
using System.Collections;
using Entitas;

public abstract class HandleTowerSelectGUI : MonoBehaviour {

	public virtual void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_SELECT, HandleTowerClick);
	}

	public virtual void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_SELECT, HandleTowerClick);
	}

	public abstract void HandleTowerClick (Entity e);

	public abstract void HandleEmptyClick ();
}
