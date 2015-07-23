using UnityEngine;
using System.Collections;

public class LogoScript : MonoBehaviour 
{

    private int faze;                                   //faza desavanja

    public float startTimeOut;
    public float braExpansion;
    public float braNarrowing;

    public float aplhaParameter;
    public float endTextAlpha;

    public float cactusAlphaSpeed;
    public float timeFlashing;
    public float minCactusAlpha;
    public float timeStill;

    private bool textMaxTrigger;
    private bool cactusTrigger;
    
    public GameObject leftBacket;
    private Transform leftBracketTran;
    private SpriteRenderer leftBracketSpr;

    public GameObject rightBacket;
    private Transform rightBracketTran;
    private SpriteRenderer rightBracketSpr;

    public GameObject text;
    private SpriteRenderer textSpr;

    public GameObject cactus;
    private SpriteRenderer cactusSpr;

    void Awake()
    {
        leftBracketTran = leftBacket.GetComponent<Transform>();
        leftBracketSpr = leftBacket.GetComponent<SpriteRenderer>();

        rightBracketTran = rightBacket.GetComponent<Transform>();
        rightBracketSpr = rightBacket.GetComponent<SpriteRenderer>();

        textSpr = text.GetComponent<SpriteRenderer>();

        cactusSpr = cactus.GetComponent<SpriteRenderer>();
    }

	void Start () 
    {
        textMaxTrigger = false;
        cactusTrigger = false;
        faze = 0;
        textSpr.color = new Color(1, 1, 1, 0);                                                           //text desert code je u kadru ali je prvo nevidljiv
        cactusSpr.color = new Color(cactusSpr.color.r, cactusSpr.color.g, cactusSpr.color.b, 0);         //cactus takodje
        leftBracketSpr.color = new Color(1, 1, 1, endTextAlpha);
        rightBracketSpr.color = new Color(1, 1, 1, endTextAlpha);
	}
	
	void Update () 
    {
        switch (faze)
        {
            case 0:     //vreme na pocetku
                if (startTimeOut > 0)
                {
                    startTimeOut = startTimeOut - Time.deltaTime;
                }
                else faze++;
                break;

            case 1:     //sirenje zagrada
                if (rightBracketTran.position.x < 8)
                {
                    leftBracketTran.position = new Vector3(leftBracketTran.position.x - braExpansion, 0, 0);
                    rightBracketTran.position = new Vector3(rightBracketTran.position.x + braExpansion, 0, 0);
                }
                else faze++;
                break;

            case 2:     //skupljanje zagrada
                if (rightBracketTran.position.x > 6.0f)
                {
                    leftBracketTran.position = new Vector3(leftBracketTran.position.x + braNarrowing, 0, 0);
                    rightBracketTran.position = new Vector3(rightBracketTran.position.x - braNarrowing, 0, 0);
                }
                else faze++;
                break;

            case 3:
            case 4:     //pojavljivanje texta (a ide do 1 zatim se smanjuje do endTextAlpha) i kaktusa
                if (textSpr.color.a < 1 && textMaxTrigger == false)
                {
                    textSpr.color = new Color(1, 1, 1, textSpr.color.a + aplhaParameter);
                    if (textSpr.color.a >= 1)
                        textMaxTrigger = true;
                }
                else if (textSpr.color.a > endTextAlpha && textMaxTrigger == true)
                {
                    textSpr.color = new Color(1, 1, 1, textSpr.color.a - aplhaParameter);
                }
                else faze++;

                if (cactusSpr.color.a < 0.6)
                {
                    cactusSpr.color = new Color(cactusSpr.color.r, cactusSpr.color.g, cactusSpr.color.b, cactusSpr.color.a + cactusAlphaSpeed);
                }
                else if (cactusSpr.color.a >= minCactusAlpha && cactusTrigger == false)
                {
                    faze++;
                    cactusTrigger = true;
                }

                break;

            case 5:     //kaktus dobija alpha 1 na odredjen vreme (timeStill vreme)
                if (timeStill > 0)
                {
                    timeStill = timeStill - Time.deltaTime;
                    cactusSpr.color = new Color(cactusSpr.color.r, cactusSpr.color.g, cactusSpr.color.b, 1);
                }
                else faze++;
                break;

            case 6:     //kaktus treperi
                if (timeFlashing > 0)
                {
                    float rand = Random.Range(1, 100);
                    rand = rand % (1 - minCactusAlpha);
                    cactusSpr.color = new Color(cactusSpr.color.r, cactusSpr.color.g, cactusSpr.color.b, minCactusAlpha + rand);
                    timeFlashing = timeFlashing - Time.deltaTime;
                }
                else faze++;
                break;

            case 7:     //prelazak u meni
                Application.LoadLevel(1);
                break;

            default:
                break;
        }

	
	}
}
