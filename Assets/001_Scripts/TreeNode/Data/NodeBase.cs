using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class NodeBase {

	public bool isSelected = false;
	public string nodeName;
	public Rect nodeRect;
	public TreeUI tree;
	

	protected GUISkin nodeSkin;

	[Serializable]
	public class NodeInput {
		public bool isOccupied = false;
		public NodeBase inputNode;
	}

	public class NodeOutput {
		public bool isOccupied = false;

		// TODO: there is multiple outputs
	}

	public virtual void InitNode () {
		Debug.Log ("Init Node");
	}

	public virtual void UpdateNode (Event _e, Rect _viewRect) {
		ProcessEvents (_e, _viewRect);
	}
	public virtual void UpdateNodeGUI (Event _e, Rect _viewRect, GUISkin _viewSkin) {
		ProcessEvents (_e, _viewRect);

		if (!isSelected) {
			GUI.Box (nodeRect, nodeName, _viewSkin.GetStyle ("NodeDefault"));
		} else {
			GUI.Box (nodeRect, nodeName, _viewSkin.GetStyle ("NodeSelected"));
		}
//		EditorUtility.SetDirty (this);
	}

	public virtual void DrawNodeProperties () {

	}

	private void ProcessEvents (Event e, Rect viewRect) {
		if (isSelected) {
			if (viewRect.Contains (e.mousePosition)) {
				if (e.type == EventType.MouseDrag) {
					nodeRect.x += e.delta.x;
					nodeRect.y += e.delta.y;
				}
			}
		}
	}
}
