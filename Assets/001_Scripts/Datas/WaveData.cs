using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] public int waveId;
    [SerializeField] public List<WaveGroup> groups;

    public WaveData (int _waveId, List<WaveGroup> _groups){
        waveId = _waveId;
        groups = _groups;
    }
}
