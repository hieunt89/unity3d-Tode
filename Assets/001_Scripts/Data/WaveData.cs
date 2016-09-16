using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] public string id;
	[SerializeField] private List<WaveGroupData> groups;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

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
