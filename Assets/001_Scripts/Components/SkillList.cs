using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillList : IComponent {
	public List<Tree<string>> skillTrees;
}
