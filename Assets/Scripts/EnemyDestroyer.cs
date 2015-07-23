using UnityEngine;
using System.Collections;

public class EnemyDestroyer : MonoBehaviour 
{
    public bool enemyDead = false;
    private bool hurtSamurai = false;                      //da li je povredio samuraja ili ne (da bi mu skinuo samo jedan zivot)

    private BoxCollider2D myBoxCollider;
    private CircleCollider2D myCircleCollider;

    private GameObject samurai;
    private SamuraiMain samuraiMain;
    private Animator samuraiAnimator;

    private Animator anim;

    private GameObject gamePlayScriptObject;            //gameplay skripti shalje informaciju da je enemy ubiven tj povecava br ubistava za 1
    private GameplayScript gamePlayScript;
    private bool murderReported;                        //da prijavi ubistvo samo jednom umesto da prijavljuje vise puta u update funkciji

    private GameObject skriptaInsta;
    private BloodInsta bloodInsta;

    private float hurtTime;
    public float startHurtTime;
    void Awake()
    {
        skriptaInsta = GameObject.Find("SkriptaInsta");
        bloodInsta = skriptaInsta.GetComponent<BloodInsta>();

        gamePlayScriptObject = GameObject.Find("Gameplay");
        gamePlayScript = gamePlayScriptObject.GetComponent<GameplayScript>();

        anim = GetComponent<Animator>();     
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        myCircleCollider = gameObject.GetComponent<CircleCollider2D>();

        samurai = GameObject.Find("Player");
        samuraiMain = samurai.GetComponent<SamuraiMain>();
        samuraiAnimator = samurai.GetComponent<Animator>();
        murderReported = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //kada se sudare i ukljucen je attackMode
        if (other.gameObject.tag == "Player" && samuraiMain.attackMode == true)
        {
            //Disable-ovanje kolajdera    
            myBoxCollider.enabled = false;
            myCircleCollider.enabled = false;

            //Indikacija da je enemy mrtav
            enemyDead = true;

            //ubacivanje animacije u dead (nema vracanja posle smrti)
            anim.SetBool("Dead", enemyDead);
        }

        //kada se sudare i nije ukljucen attackMode
        else if (other.gameObject.tag == "Player" && samuraiMain.attackMode == false)
        {
            //Disable-ovanje kolajdera i ukljucivanje isKinematic
            myBoxCollider.enabled = false;
            myCircleCollider.enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

            //Smanjivanje zivota samuraju
            if (hurtSamurai == false)
            {
                samuraiMain.lives = samuraiMain.lives - 1;
                hurtSamurai = true;
                
            }
            samuraiAnimator.SetBool("Hurt", true);
        }
        
        //kada udara u druge kolajdere...
        else return;
    }
   
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            samuraiAnimator.SetBool("Hurt", false);


        //Kada ziv prodje pored samuraja
        if (other.gameObject.tag == "Player" && enemyDead == false)
        {
            //Opet ukljucuje kolajdere i disable-uje isKinematic
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            myBoxCollider.enabled = true;
            myCircleCollider.enabled = true;

            //Ukljucuje porebnu animaciju
            anim.SetBool("Winner", true);
        }
    }

    void Update()
    {
        if (enemyDead)
        {
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            if (murderReported == false)
            {
                gamePlayScript.kills++;
                murderReported = true;

                bloodInsta.RandomInsta();               //instancira se krv! samo jednom!
            }
            
        }
        if (samuraiAnimator.GetBool("Hurt") == true)
        {
            if (hurtTime > 0)
            {
                hurtTime = hurtTime - Time.deltaTime;
            }
            else
            {
                hurtTime = startHurtTime;
                samuraiAnimator.SetBool("Hurt", false);
            }
                        
        }

    }
}
