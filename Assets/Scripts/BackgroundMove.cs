using UnityEngine;
using System.Collections;

public class BackgroundMove : MonoBehaviour 
{

    public float speed = 1.0f;                                          //brzina kretanja pozadine

    private Transform trans;                                            //transform komponenta pozadine
    private SpriteRenderer sprtRend;                                     //sprite pozadine (da bi odredili shirinu slike)

    public GameObject newBackGround;                                        //nova pozadina koja se instancira                                      

    public GameObject mainCamera;                                           //objekat mainCamera
    private Transform mainCameraTrans;                                      //Transform (komponenta objekta mainCamera)

    private float desnaTackaPozadine;
    private float desnaTackaKamere;
    private float levaTackaKamere;

    private bool instanciated = false;                                  //da ne ulazi u beskonacnu petlju instanciranja vec samo jednom

    private GameObject samurai;
    private SamuraiMain samuraiMain;

    public float timeToStopAfterDeath;
    private float helpTime = 0.0f;
    private float newSpeed;

    void Awake ()
    {
        samurai = GameObject.Find("Player");
        samuraiMain = samurai.GetComponent<SamuraiMain>();

        mainCameraTrans = mainCamera.GetComponent<Transform>();
        
        trans = GetComponent<Transform>();
        sprtRend = GetComponent<SpriteRenderer>();
        
    }

	void Start () 
    {
        newSpeed = speed;
	}

    void Update()
    {
      
        desnaTackaPozadine = (trans.position.x + sprtRend.bounds.size.x / 2) * 100;                                             //pixelWidth je u pixelima, position mnozimo sa 100 da bi dobili pixele
        desnaTackaKamere = (mainCameraTrans.transform.position.x * 100) + mainCameraTrans.GetComponent<Camera>().pixelWidth / 2 + 100;          //dodajem 100 da ne bi bilo bas na granici
        levaTackaKamere = (mainCameraTrans.transform.position.x * 100) - mainCameraTrans.GetComponent<Camera>().pixelWidth / 2 - 100;           //oduzimam 100 da ga ne bi brisao bas na granici kamere
        
        if (desnaTackaPozadine < desnaTackaKamere && instanciated==false)
        {
            instanciated = true;
            Instantiate(newBackGround, new Vector3(trans.position.x + sprtRend.bounds.size.x, trans.position.y, trans.position.z), new Quaternion(0, 0, 0, 0));
        }

        if (desnaTackaPozadine < levaTackaKamere)
        {
            Destroy(gameObject);
        }
    }

	void FixedUpdate () 
    {
        //pomeramo sliku u levo za speed*deltaTime pod uslovom da je samuraj ziv
        if(samuraiMain.lives>0)
            trans.position = new Vector3(trans.position.x - (Time.deltaTime * speed), trans.position.y, trans.position.z) ;
        else if(samuraiMain.lives<=0 && newSpeed > 0)
        {
            helpTime = helpTime + Time.deltaTime;
            newSpeed= -speed/timeToStopAfterDeath * helpTime + speed;
            trans.position = new Vector3(trans.position.x - (Time.deltaTime * newSpeed), trans.position.y, trans.position.z);
        }
        else return;
	}
}
