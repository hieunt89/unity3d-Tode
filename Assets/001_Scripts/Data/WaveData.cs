using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] public string id;
	[SerializeField] private List<WaveGroupData> groups;
	[SerializeField] public float waveDelay;

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

	public WaveData (string _id, List<WaveGroupData> _groups, float waveDelay = 1f){
		this.id = _id;
		this.groups = _groups;
		this.waveDelay = waveDelay;
    }
}
