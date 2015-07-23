using UnityEngine;
using System.Collections;

public class MenuControllerScript : MonoBehaviour 
{
    public void Achievement()
    {
        Application.LoadLevel(5);
    }

    public void Credits()
    {
        Application.LoadLevel(4);
    }

    public void Game()
    {
        Application.LoadLevel(2);
    }
}

