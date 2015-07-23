using UnityEngine;
using System.Collections;

public class BloodInsta : MonoBehaviour 
{

    private int[] alreadyCalled = new int[10];

    public GameObject[] bloods;

    void Start()
    {
        alreadyCalled[0]=0;
        alreadyCalled[1]=0;
        alreadyCalled[2]=0;
        alreadyCalled[3]=0;
        alreadyCalled[4]=0;
        alreadyCalled[5]=0;
        alreadyCalled[6]=0;
        alreadyCalled[7]=0;
        alreadyCalled[8]=0;
        alreadyCalled[9]=0;

    }
	
	public void RandomInsta ()
    {
        int i;

        //odredjujemo random i
        i = Random.Range(1, 17);

        //ako je to i bilo u poslednjih 10 puta, trazimo novu vrednost;
        while (Search(i, alreadyCalled) == true)
            i = Random.Range(0 , 16);
      
        ShiftArrayAndPutIn(i);

        bloods[i].SetActive(true);	
	}

    private bool Search(int x, int[] alreadyCalled)
    {
        for(int i=0; i<10;i++)
        {
            if (x == alreadyCalled[i])
                return true;
        }
        return false;
    }

    public void ShiftArrayAndPutIn(int x)
    {
        for (int i = 9; i > 0; i--)
        {
            alreadyCalled[i] = alreadyCalled[i - 1];
        }
        alreadyCalled[0] = x;
    }
}
