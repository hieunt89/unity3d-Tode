using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData {
	[SerializeField] public int id;
    [SerializeField] public List<WaveGroup> groups;

    public WaveData (int _id, List<WaveGroup> _groups){
        id = _id;
        groups = _groups;
    }
}
