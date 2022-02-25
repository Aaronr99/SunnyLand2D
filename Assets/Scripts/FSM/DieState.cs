using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    private bool assigned;
    public DieState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }
    public override void DoState()
    {
        if (!assigned)
        {
            // Ejecuta una animacion de muerte
            iPC.anim.SetTrigger("die");
            iPC.boxCollider.enabled = false;
            iPC.rBody.simulated = false;
            iPC.rBody.gravityScale = 0;
            assigned = true;
            iPC.DieSound();
        }   
    }
}
