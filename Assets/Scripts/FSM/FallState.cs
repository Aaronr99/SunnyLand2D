
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{ 
    public FallState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }

    public override void DoState()
    {
        iPC.anim.SetBool("falling", true);
        iPC.anim.SetBool("moving", false);
        iPC.anim.SetBool("jumping", false);
        // Se puede mover y acelerar la caida cuando cae
        iPC.Move();
        iPC.AccelerateFall();
        iPC.fSMDebugger.SetText("Falling");

        // Primero se comprueba si recibio daño
        // todos los frames menos si hay algo debajo
        if (iPC.damaged && !iPC.InGround())
        {
            iPC.SetState(new HurtState(iPC));
            return;
        }

        if (iPC.InGround())
        {
            iPC.anim.SetBool("falling", false);
            // Si tiene un enemigo debajo
            if (iPC.InGround().GetComponent<Enemy>())
            {
                // Vibrar la camera
                iPC.CameraShake();
                // Comunica al enemigo que lo ataco
                iPC.InGround().GetComponent<Enemy>().TakeDamg();
                // Vuelve a Saltar
                iPC.anim.SetBool("falling", false);
                // Hacer el sonido de daño
                iPC.EnemyHurtSound();
                iPC.SetState(new JumpState(iPC));
                return;
            }

            // Instancia los efectos de caida
            iPC.InstantiateJumpFX();
            iPC.FallSound();

            // Si esta saltando intentando saltar ni bien cae
            if (iPC.canJump)
            {
                iPC.SetState(new JumpState(iPC));
                return;
            }
            // Si esta quieto 
            if (InputManager.Instance.Horizontal == 0)
            {
                iPC.SetState(new IddleState(iPC));
                return;
            }
            // Si se esta moviendo
            else if (InputManager.Instance.Horizontal != 0)
            {
                iPC.SetState(new MoveState(iPC));
                return;
            }
        }
    }
}
