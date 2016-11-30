using UnityEngine;
using System.Collections;
using Entitas;

public abstract class HandleTowerSelectGUI : MonoBehaviour {

	public virtual void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.TOWER_SELECT, HandleTowerClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public virtual void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.TOWER_SELECT, HandleTowerClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public abstract void HandleTowerClick (Entity e);

	public abstract void HandleEmptyClick ();
}
