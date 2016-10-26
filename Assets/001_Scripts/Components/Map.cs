using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class Map : IComponent {
	public MapData data;
}
