using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayScript : MonoBehaviour 
{

    public int score;
    public int kills;
    public int minutes;
    public int seconds;

    private float secondsFloat;
    private float minutesFloat;

    public float enemyInstTime;                                             //random vreme instanciranja enemy-a
    public float enemyInstTimeMin=0.5f;                                     //min vreme instanciranja enemy-a
    public float enemyInstTimeMax=1.5f;                                     //max vreme instanciranja enemy-a

    public float minSpeed = 100;
    public float maxSpeed = 200;

    public GameObject enemy;

    public GameObject queen;
    private bool queenInst = false;                                         //pokazatelj da li je isntancirana kraljica(da se ne bi vise puta instancirala)

    private GameObject samurai;                     
    private SamuraiMain samuraiMain;

    public float restartMenuDelay = 1.5f;                                   //vreme od ubistva samuraja do pojavljivanja restart ekrana
    public float timeTillEnd = 120;                                         //vreme do kraja nivoa
    public float timeOut = 3;                                               //vreme izmedju prestanka instanciranja enemy-a i instanciranja kraljice

    public bool nextTrigger=false;

    public GameObject [] stripes = new GameObject [7];                      //niz GameObjekata linija koje oznacavaju broj udaraca
    private Image [] stripesRend = new Image [7];                            //Sprite Rend komponente strajpova
    private int lastFrameAttackNum;                                         //pomocna promenljiva za StripsAndHits funkciju

    public GameObject[] lives = new GameObject[3];                          //niz objekata smajlija koji oznacavaju broj zivota
    private Image[] livesRend = new Image[3];                                //Sprite Renderer komponente smajlija
    private int lastFrameLives;                                             //pomocna promenljiva za StripsAndLives metodu

    private bool paused;                                                    //bool koji kaze da li je igra pauzirana (paused=1) ili pichi (paused=0)

    
    public GameObject[] messanges = new GameObject [1];

    private SpriteRenderer extraLifeSprRen;
    public int extraLifeAfterThisKills = 20;                                //nakon ovoliko ubistava dobija se xtra zivot
    public float extraLifeAplhaSpeed = 1.5f;                                //za koliko ce alpha da dostigne 1 (u sekundama)
    private bool extraLifeMessageTrigger = false;                           //pomocna promenljiva za pozivanje ExtraLifeMessage metode
    private bool lifeGained = false;                                        //pomocna kojakaze da li je dobijen zivot da ne zove vise puta u update-u
    private int switchTrigger = 0;                                          //pomocna za switch u metodi ExtraLifeMessage

  
    public Text guiScoreText;                                              //Text komponenta guiScore


    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject backPausePanel;
    public GameObject frontPausePanel;
    public GameObject pauseText;

    void Awake()
    {
        samurai = GameObject.Find("Player");
        samuraiMain = samurai.GetComponent<SamuraiMain>();
      
        enemyInstTime = Random.Range(enemyInstTimeMin, enemyInstTimeMax);

        for (int i = 0; i < 7; i++)
        {
            stripesRend[i] = stripes[i].GetComponent<Image>();
        }

        for (int i = 0; i < 3; i++)
        {
            livesRend[i] = lives[i].GetComponent<Image>();
        }
        
        extraLifeSprRen = messanges[0].GetComponent<SpriteRenderer>();

        score = 0;
        kills = 0;
        seconds = 0;
        minutes = 0;
        minutesFloat = 0.0f;
        secondsFloat = 0.0f;


    }

	void Start () 
    {
        nextTrigger = false;
        lastFrameAttackNum = samuraiMain.attackNum;
        paused = false;
        lastFrameLives = samuraiMain.lives;
        resumeButton.SetActive(false);
        backPausePanel.SetActive(false);
        frontPausePanel.SetActive(false);
        pauseText.SetActive(false);
	}	

	void Update ()
    {
        //Random instanciranje neprijatelja
        InstEnemy();

        //Stopiranje igrice nakon sto je samurai mrtav
        Stop();

        //Stopiranje enemy-a i instanciranje kraljice
        //Queen();

        //pauza i next level nakon ubijanja kraljice
        Next();

        //Podesavanje alpha parametra za linije tj preostale napade
        StripsAndHits();

        //Podesavanje alpha parametra za linije tj preostale zivote
        StripsAndLives();

        //Dobijanje extra zivota kada se ubije odredjen broj (extraLifeAfterThisKills) neprijatelja
        if (kills % extraLifeAfterThisKills == 0 && lifeGained == false && kills>0)
            ExtraLife();
        if (kills % extraLifeAfterThisKills == 1 && lifeGained == true)
            lifeGained = false;
        ExtraLifeMessage(extraLifeMessageTrigger);
 
        //Racunanje vremena, killova, skora i njegovo updateovanje
        GuiScore();
	}

    /*private void Queen()
    {
        if (timeTillEnd > 0)
        {
            timeTillEnd = timeTillEnd - Time.deltaTime;
        }

        else if (timeTillEnd <= 0 && timeOut > 0)
        {
            timeOut = timeOut - Time.deltaTime;
        }

        else if (timeTillEnd <= 0 && queenInst == false)
        {
            queenInst = true;
            Instantiate(queen);
            samuraiAnimator.SetBool("Queen", true);
        }
        else return;
    }*/

    private void InstEnemy()
    {
        if (timeTillEnd > 0)
        {
            if (enemyInstTime >= 0)
            {
                enemyInstTime = enemyInstTime - Time.deltaTime;
            }
            else
            {
                GameObject clone;
                EnemyMain enemyScript;            

                clone = Instantiate(enemy) as GameObject;
                enemyScript = clone.GetComponent<EnemyMain>();
                enemyScript.enemySpeed = Random.Range(minSpeed, maxSpeed);
                enemyInstTime = Random.Range(enemyInstTimeMin, enemyInstTimeMax);                
            }
        }
    }

    private void Stop()
    {
        if (samuraiMain.lives <= 0)
        { 
             if(restartMenuDelay>0)
             {
                 restartMenuDelay = restartMenuDelay - Time.deltaTime;
             }

             //u ovu petlju ulazi nakon restartMenuDelay vremena (vreme od kad ubiju samuraja do kad udje u restart meni)
             else
             {
                 Application.LoadLevel("Restart");
             }

        }

    }

    public void Next()
    {
        if (nextTrigger == true)
        {

            if (restartMenuDelay > 0)
            {
                restartMenuDelay = restartMenuDelay - Time.deltaTime;
            }
            else
            {
                Application.LoadLevel("Restart");
            }
        }


           
    }

    private void StripsAndHits()
    {
        if (lastFrameAttackNum > samuraiMain.attackNum && samuraiMain.lives>0)
        {
            stripesRend[lastFrameAttackNum - 1].enabled = false;
            lastFrameAttackNum = samuraiMain.attackNum;
        }
        else if (lastFrameAttackNum < samuraiMain.attackNum && samuraiMain.lives>0)
        {
            stripesRend[lastFrameAttackNum].enabled = true;
            lastFrameAttackNum = samuraiMain.attackNum;
        }
        else return;
    }

    private void StripsAndLives()
    {
        if (lastFrameLives > samuraiMain.lives  && samuraiMain.lives >=0)
        {
            livesRend[lastFrameLives - 1].enabled = false;
            lastFrameLives = samuraiMain.lives;
        }
        else if (lastFrameLives < samuraiMain.lives)
        {
            livesRend[lastFrameLives].enabled = true;
            lastFrameLives = samuraiMain.lives;
        }
        else return;
    }

    public void Pause()
    {
        if (paused == true)
        {
            Time.timeScale = 1.0f;
            paused = false;

            pauseButton.SetActive(true);

            resumeButton.SetActive(false);
            backPausePanel.SetActive(false);
            frontPausePanel.SetActive(false);
            pauseText.SetActive(false);
        }
        else if (paused == false)
        {
            Time.timeScale = 0.0f;
            resumeButton.SetActive(true);
            backPausePanel.SetActive(true);
            frontPausePanel.SetActive(true);
            pauseText.SetActive(true);
            paused = true;
            pauseButton.SetActive(false);
        }
    }

    private void ExtraLife()
    {
        if (samuraiMain.lives < 3)
        {
            samuraiMain.lives++;
            lifeGained = true;
            extraLifeMessageTrigger = true;
        }
    }

    private void ExtraLifeMessage(bool trigger)
    {
        
        if (trigger == false)
            return;
        switch (switchTrigger)
        {
            case 0:
                if (extraLifeSprRen.color.a < 1)
                {
                    extraLifeSprRen.color = new Color(1, 1, 1, extraLifeSprRen.color.a + Time.deltaTime * (1 / extraLifeAplhaSpeed));
                }
                else switchTrigger++;
                break;
            case 1:
                if (extraLifeSprRen.color.a > 0)
                {
                    extraLifeSprRen.color = new Color(1, 1, 1, extraLifeSprRen.color.a - Time.deltaTime * (1 / extraLifeAplhaSpeed));
                }
                else switchTrigger++;
                break;
            case 2:
                extraLifeMessageTrigger = false;
                switchTrigger = 0;
                break;
        }

    }

    private void GuiScore()
    {
        if (samuraiMain.lives > 0)
        {
            secondsFloat = secondsFloat + Time.deltaTime;
            minutesFloat = minutesFloat + Time.deltaTime;
            seconds = (int)secondsFloat % 60;
            minutes = (int)minutesFloat / 60;
            score = kills * (seconds + minutes * 60);
        }
        guiScoreText.text = score.ToString();
    }
    
}
