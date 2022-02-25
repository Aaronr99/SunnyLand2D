using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDestroy : StateMachineBehaviour
{
    // Destruy el GO cuando termina la animacion
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
