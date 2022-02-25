using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerController iPC;

    protected State(PlayerController pPlayerController)
    {
        this.iPC = pPlayerController;
    }

    public virtual void DoState()
    {
       
    } 
}
