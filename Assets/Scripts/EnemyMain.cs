using UnityEngine;
using System.Collections;

public class EnemyMain : MonoBehaviour {

    public float enemySpeed;                 //brzinu nameshta skripta gameplay kada instancira, tako da je ovde vrednost nebitna

    public LayerMask whatIsGround;
    private Transform groundCheck;
    private float groundRadius = 0.5f;
    public bool grounded = false;

    public float timeTillJump;
    public float timeTillJumpMin = 0.5f;
    public float timeTillJumpMax = 1.5f;
    public float jumpForce = 100f;

    private Animator anim;
    private float distance;

    public GameObject samurai;
    private EnemyDestroyer enemyDestroyer;

    void Awake()
    {
        enemyDestroyer = GetComponent<EnemyDestroyer>();
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
    }

	void Start ()
    {
        timeTillJump = Random.Range(timeTillJumpMin, timeTillJumpMax);
        anim.SetFloat("Distance", 100);
	}
	
	void Update () 
    {
        //racuna distancu od samuraja i na osnovu toga podesava anim
        distance = Distance2D(samurai);
        anim.SetFloat("Distance", distance);
	}

    void FixedUpdate()
    {
        //Kretanje enemy-a u levo
        GetComponent<Rigidbody2D>().velocity = (new Vector2(-enemySpeed, GetComponent<Rigidbody2D>().velocity.y));

        //Proveravanje da li je na zemlji i rad animatora na osnovu toga
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        //Random skok pod uslovom da je enemy ziv
        if(enemyDestroyer.enemyDead==false) 
            RandomJump();   
    }

    public void Jump()
    {
        anim.SetBool("Ground", false);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }

    private void RandomJump()
    {
        if (timeTillJump < 0 && grounded == true)
        {
            Jump();
        }

        else if (timeTillJump >= 0 && grounded == true)
        {
            timeTillJump = timeTillJump - Time.deltaTime;
        }

        else if (timeTillJump < 0 && grounded == false)
        {
            timeTillJump = Random.Range(timeTillJumpMin, timeTillJumpMax);
        }
    }

    private float Distance2D (GameObject other)
    {
        return Mathf.Sqrt((other.transform.position.x - transform.position.x) * (other.transform.position.x - transform.position.x)
                            + (other.transform.position.y - transform.position.y) * (other.transform.position.y - transform.position.y));
    }
}
