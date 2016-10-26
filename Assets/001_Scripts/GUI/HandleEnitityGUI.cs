using UnityEngine;
using System.Collections;

public abstract class HandleEnitityGUI : MonoBehaviour {

	public virtual void OnEnable(){
		Messenger.AddListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.AddListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.AddListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public virtual void OnDisable(){
		Messenger.RemoveListener (Events.Input.EMPTY_SELECT, HandleEmptyClick);
		Messenger.RemoveListener (Events.Input.ENTITY_SELECT, HandleEntityClick);
		Messenger.RemoveListener (Events.Input.ENTITY_DESELECT, HandleEmptyClick);
	}

	public abstract void HandleEntityClick ();

	public abstract void HandleEmptyClick ();
}
