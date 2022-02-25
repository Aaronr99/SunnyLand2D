using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public MoveState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }

    private float waitForInput = 0;
    private float runSoundTimer = 0;
    public override void DoState()
    {
        iPC.anim.SetBool("moving", true);
        iPC.fSMDebugger.SetText("Moving");

        iPC.Move();
        // Ejecuta el ruido de pasos cada medio segundo
        runSoundTimer += Time.deltaTime;
        if (runSoundTimer > 0.51f)
        {
            iPC.RunAudio();
            runSoundTimer = 0;
        }
        // Se controla cuanto tiempo paso desde que se dejo apretar el boton de movimiento
        // para evitar que si el input esta en 0 se vuelva a Iddle instantaneamente
        if (InputManager.Instance.Horizontal == 0)
        {
            waitForInput += Time.deltaTime;
        }
        else
        {
            waitForInput = 0;
        }

        // CAMBIOS DE ESTADO

        // Primero se comprueba si recibio daño
        if (iPC.damaged)
        {
            iPC.SetState(new HurtState(iPC));
            return;
        }

        // Se comprueba si esta cayendo
        if (!iPC.allowedAirDelay)
        {
            iPC.SetState(new FallState(iPC));
            return;
        }
        // Si todavia no esta cayendo y apreta el boton de saltar
        else if (iPC.canJump)
        {
            iPC.SetState(new JumpState(iPC));
            return;
        }

        // Si no se esta apretando el boton de movimiento se vuelve a Iddle
        if (waitForInput > 0.1f)
        {
            iPC.anim.SetBool("moving", false);
            iPC.SetState(new IddleState(iPC));
            iPC.moveSpeed = 0;
            return;
        }
    }
}
