using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] public string id;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	[SerializeField] private List<WaveGroupData> groups;

	public List<WaveGroupData> Groups {
		get {
			return groups;
		}
		set {
			groups = value;
		}
	}

    public WaveData (string _id, List<WaveGroupData> _groups){
        id = _id;
        groups = _groups;
    }
}
