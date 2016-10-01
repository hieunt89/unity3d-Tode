using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class CurrentSelected : IComponent {
	public Entity e;
}
