using UnityEngine;
using System.Collections;

public class SamuraiInput : MonoBehaviour 
{

    private SamuraiMain samuraiMain;
    public bool attackEnable = true;
    public bool inputEnable = true;



    void Awake()
    {
        samuraiMain = GetComponent<SamuraiMain>();
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && samuraiMain.grounded==true && inputEnable==true)
            samuraiMain.Jump();
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && attackEnable==true && inputEnable==true) 
            samuraiMain.Attack();
    }
}
