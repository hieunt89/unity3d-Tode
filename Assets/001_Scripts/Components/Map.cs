using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class Map : IComponent{
	public int width;
	public int height;

	public GameObject view;
}
