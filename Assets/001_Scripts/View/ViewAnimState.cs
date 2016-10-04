using UnityEngine;
using System.Collections;

public class ViewAnimState : StateMachineBehaviour {
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit (animator, stateInfo, layerIndex);
		animator.gameObject.GetComponent<EntityLink> ().Link.IsAttacking (false);
	}

}
