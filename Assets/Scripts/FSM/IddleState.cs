using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleState : State
{
    public IddleState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }
    public override void DoState()
    {
        iPC.anim.SetBool("moving", false);
        iPC.fSMDebugger.SetText("Iddle");

        // CAMBIOS DE ESTADO

        // Primero se comprueba si recibio daño
        if (iPC.damaged)
        {
            iPC.SetState(new HurtState(iPC));
            return;
        }
        // Si esta cayendo
        if (!iPC.allowedAirDelay)
        {
            iPC.SetState(new FallState(iPC));
            return;
        }
        // Si se esta intentando mover
        if (InputManager.Instance.Horizontal != 0)
        {
            iPC.SetState(new MoveState(iPC));
            return;
        }
        // Si intenta saltar
        if (iPC.canJump)
        {
            iPC.SetState(new JumpState(iPC));
            return;
        }
    }
}
