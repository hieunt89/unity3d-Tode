using Entitas;
using System.Collections.Generic;

[System.Serializable]
public class Wave : IComponent {
	public List<WaveGroup> groups;
}
