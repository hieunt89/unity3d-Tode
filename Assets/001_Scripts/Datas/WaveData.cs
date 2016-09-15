using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] private int waveId;

	public int WaveId {
		get {
			return waveId;
		}
		set {
			waveId = value;
		}
	}

	[SerializeField] private List<WaveGroup> groups;

	public List<WaveGroup> Groups {
		get {
			return groups;
		}
		set {
			groups = value;
		}
	}

    public WaveData (int _waveId, List<WaveGroup> _groups){
        waveId = _waveId;
        groups = _groups;
    }
}
