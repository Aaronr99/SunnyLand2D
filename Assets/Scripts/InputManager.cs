using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // Controla todos los inputs permitidos por el juego
    #region Singleton
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
    }
    #endregion

    public bool JumpKeyDown
    {
        get
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.UpArrow))
            {
                return true;
            }
            return false;
        }
    }

    public bool JumpKey
    {
        get
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.UpArrow))
            {
                return true;
            }
            return false;
        }
    }


    public int Horizontal
    {
        get
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                return 1;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                return -1;
            }
            return 0;
        }
    }

    public int Down
    {
        get
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                return -1;
            }
            return 0;
        }
    }

    public bool EscapeKeyDown
    {
        get
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Insert))
            {
                return true;
            }
            return false;
        }
    }
}
