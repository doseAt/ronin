using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

    void OnGUI()
    {
        if(GUI.Button(new Rect(20, 20 , 100 , 100), "RESTART LEVEL"))
            Application.LoadLevel("Main");
        if (GUI.Button(new Rect(140, 140, 100, 100), "Main Menu"))
            Application.LoadLevel("MainMenu");
    }

	
}
