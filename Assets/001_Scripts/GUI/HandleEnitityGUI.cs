using UnityEngine;
using System.Collections;
using Entitas;

public abstract class HandleEnitityGUI : MonoBehaviour {

	public virtual void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener <Entity> (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public virtual void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener <Entity> (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public abstract void HandleEntityClick (Entity e);

	public abstract void HandleEmptyClick ();
}
