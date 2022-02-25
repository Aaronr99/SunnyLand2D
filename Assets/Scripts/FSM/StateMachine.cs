using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State actualState;

    public void SetState(State pState)
    {
        actualState = pState;
    }
}
