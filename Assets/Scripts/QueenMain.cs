using UnityEngine;
using System.Collections;

public class QueenMain : MonoBehaviour 
{
    public int lives = 5;

    public float attackSpeed = 17;
    public float walkSpeed = 3;
    public float moveSpeed;

    public float timeToAttack;                              //vreme od pristupanja QueenStop kolajderu do novog napada
    private bool waitOnAttack = false;

    public float timeToAttackMin = 0.5f;
    public float timeToAttackMax = 3;    

    public bool collideEnable = true;                       //da bi se po JEDNOM ulazilo u OnEnterCollide2D metodu, inace ulazi svaki frejm i jebe parametre koji treba da budu podeseni jednom!
    

    private GameObject samurai;
    private SamuraiMain samuraiMain;

    private GameObject gamePlay;
    private GameplayScript gameplayScript;

    public float nextLevelDelay = 2;

    void Awake()
    {
        samurai = GameObject.Find("Player");
        samuraiMain = samurai.GetComponent<SamuraiMain>();

        gamePlay = GameObject.Find("Gameplay"); 
        gameplayScript = gamePlay.GetComponent<GameplayScript>();
    }

	void Start () 
    {
        //kraljica se krece walk brzinom do prvog QueenStop kolajdera
        moveSpeed = -walkSpeed; 
        
        //nameshta prvo random vreme    
        timeToAttack = Random.Range(timeToAttackMin, timeToAttackMax);
	}

    void Update()
    {
        if(lives == 0)
        {
            //salje informaciju gameplay skripti da je kraljica ubivena tako da moze da ide na next level  
            gameplayScript.nextTrigger = true;
        }
    }
	
	void FixedUpdate () 
    {
        Attack();

        Move(moveSpeed);

        if (lives == 0)
        {
            Destroy(gameObject);
        }

	}

    private void Attack()
    {
        if (waitOnAttack == true)
        {
            moveSpeed = 0;
            if (timeToAttack > 0)
            {
                timeToAttack = timeToAttack - Time.deltaTime;
            }
            else
            {
                timeToAttack = Random.Range(timeToAttackMin, timeToAttackMax);
                waitOnAttack = false;
                moveSpeed = -attackSpeed;
            }
        }
        else return;

    }

    private void Move(float speed)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "QueenStop" && collideEnable == true)
        {
            waitOnAttack = true;
            collideEnable = false;
        }
        else if (other.gameObject.tag == "HelpCollider")
        {
            collideEnable = true;
        }
        else if (other.gameObject.tag == "Player" && collideEnable == true)
        {
            collideEnable = false;
            moveSpeed = walkSpeed;

            //ako je samuraj u attack modu skida zivot queen-u
            if (samuraiMain.attackMode == true)
                lives--;

            //ako samuraj nije u attackModu skida mu zivot
            else
                samuraiMain.lives--;
        }
        else if(other.gameObject.tag == "PlayerHelpCollider" && collideEnable == true)
        {
            collideEnable = false;
            moveSpeed = walkSpeed;
        }
        else return;
    }

    
}
