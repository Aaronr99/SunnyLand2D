using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FSMDebugger : MonoBehaviour
{
    public TMP_Text textBox;

    public void SetText(string pText)
    {
        textBox.text = pText;
    }
}
