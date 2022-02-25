using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    public HurtState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }
    public override void DoState()
    {
        iPC.fSMDebugger.SetText("takingDmg");
        // Si ya no tiene vidas
        if (!GameplayManager.Instance.HasLifes)
        {
            iPC.SetState(new DieState(iPC));
            return;
        }
        // El sonido de golpe se reproduce una sola vez cuando se efectuo el daño
        if (iPC.damaged)
        {
            // Vibrar la camera
            iPC.CameraShake();
            iPC.HurtSound();
        }
        iPC.anim.SetTrigger("hurt");
        // Lo empuja un poco hacia arriba y mucho hacia la direccion contraria
        iPC.rBody.velocity = new Vector2(iPC.dmgDir * 1.2f, 1) * 6;
        // Resetear el boolean 
        iPC.damaged = false;        

        // Se comprueba si esta cayendo
        if (!iPC.allowedAirDelay)
        {
            iPC.SetState(new FallState(iPC));
            return;
        }
    }
}
