using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : State
{
    private bool assigned;
    private Vector3 screenCenter;
    private float timer;
    public DieState(PlayerController pPlayerController) : base(pPlayerController)
    {
    }
    public override void DoState()
    {
        if (!assigned)
        {
            Camera cam = Camera.main;
            // Se guarda el centro de la pantalla
            screenCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.55f, cam.nearClipPlane));
            // Ejecuta una animacion de muerte
            iPC.anim.SetTrigger("die");
            iPC.boxCollider.enabled = false;
            iPC.rBody.simulated = false;
            iPC.rBody.gravityScale = 0;
            assigned = true;
            iPC.DieSound();
        }
        // Espera un segundo para que ejecute la animacion
        timer += Time.deltaTime;
        if( timer > 1f)
        {
            // Si todavia no esta cerca del centro de la pantalla
            if (Vector3.Distance(iPC.transform.position, screenCenter) < 0.01f)
            {
                // Mover al jugador al centro de la pantalla
                iPC.transform.position = Vector3.MoveTowards(iPC.transform.position, screenCenter, 4 * Time.deltaTime);
            }
        }    
    }
}
