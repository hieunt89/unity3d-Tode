using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.CodeGenerator;


[SingleEntity]
public class Tick : IComponent {
	public float time;
}
