using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    public JumpState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }

    private bool alreadyJumped;
    private float delay;
    public override void DoState()
    {
        iPC.fSMDebugger.SetText("Jumping");

        // Si todavia no salto 
        if (!alreadyJumped)
        {
            iPC.anim.SetBool("moving", false);
            iPC.anim.SetBool("jumping", true);
            iPC.jumping = true;
            // Salta
            iPC.rBody.velocity = Vector2.up * iPC.jumpForce;
            delay = 0f;
            alreadyJumped = true;
            iPC.canJump = false;
            iPC.jumpTimeCounter = iPC.jumpTime;
            // Si no esta repruciendo el sonido de daño
            // reproduce el de salto
            if (!iPC.HurtSoundPlaying())
            {
                iPC.JumpSound();
            }
        }
        // Se puede mover horizontalmente mientras salta
        iPC.Move();
        // Puede saltar mas alto si mantiene el boton de salto
        if (InputManager.Instance.JumpKey && iPC.jumpTimeCounter > 0)
        {
            iPC.rBody.velocity = Vector2.up * iPC.jumpForce;
            iPC.jumpTimeCounter -= Time.deltaTime;
        }

        // Primero se comprueba si recibio daño
        if (iPC.damaged)
        {
            iPC.jumping = false;
            iPC.anim.SetBool("jumping", false);
            iPC.SetState(new HurtState(iPC));
            return;
        }

        delay += Time.deltaTime;

        // Si se acabo el tiempo disponible para saltar empieza a caer
        if (delay > iPC.jumpTime + 0.1f)
        {
            iPC.jumping = false;
            iPC.SetState(new FallState(iPC));
        }
    }


}
