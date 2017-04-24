using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour {
    public void ChangeToMainLevel()
    {
        Application.LoadLevel(1);
    }

    public void ChangeToTutorial()
    {
        Application.LoadLevel(2);
    }

    public void ChangeToTitleScreen()
    {
        Application.LoadLevel(0);
    }
}
