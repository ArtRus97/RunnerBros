using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunnerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;
    SpriteRenderer sprite;

    AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip coinClip;

    public GameController gameController;

    public float runSpeed;
    private float moveInput;

    public float jumpForce;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask groundLayer;
    
    // Start is called before the first frame update
    void Start(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        animator.runtimeAnimatorController = Resources.Load(PlayerPrefs.GetString("playerCharacter") + "Animator") as RuntimeAnimatorController;
        //tarkistetaan tallennustiedoista mikä hahmo on valittu ja laitetaan sen animaattori käyttöön
    }

    void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        //tarkistetaan koskeeko pelaaja maata
        if (!gameController.gamePaused)  { //tarkistetan että peli ei ole pysäytetty
            runSpeed = runSpeed + 0.0001f; //suurennetaan juoksunpeutta ajan myötä
            if(isGrounded && Input.GetButtonDown("Jump")){ //jos kosketaan maata ja painetaan hyppynappia
                isJumping = true; //kerrotaan että hypätään
                jumpTimeCounter = jumpTime; //otetaan käyttöön ennakkoon määritetty hyppyaika
                rigidbody2d.velocity = Vector2.up * jumpForce; //suurennetaan hahmon kiihtyvyyttä ylöspäin
                animator.SetBool("isJumping", true); //kerrotaan animaattorille että hypätään
                audioSource.PlayOneShot(jumpClip); //soitetaan hypppyääni
            }

            if(Input.GetButton("Jump") && isJumping){ //jos hyppynappi on vieläkin painettuna
                if(jumpTimeCounter > 0){ //tarkistetaan että hyppyaikaa on jäljellä
                    rigidbody2d.velocity = Vector2.up * jumpForce; //lisätään kiihtyvyyttä
                    jumpTimeCounter -= Time.deltaTime; //pienennetään hyppyaikaa ajan myötä
                } else {
                    isJumping = false; //määritetään että hyppääminen loppui
                }
            }

            if(Input.GetButtonUp("Jump")){ //jos hyppynapista päästetään irti
                isJumping = false; //lopetetaan hyppääminen
                animator.SetBool("isJumping", false); //otetaan hyppyanimaatio pois käytöstä
            }
        
            animator.SetFloat("speed", Mathf.Abs(rigidbody2d.velocity.x));
            //kerrotaan animaattorille kiihtyvyys jouksuanimaatioita varten
            if(rigidbody2d.velocity.y < 0){ 
                //tarkistetaan putoamiskiihtyvyys ja kerrotaan animaattorille että pudotaanko
                animator.SetBool("isFalling", true);
            } else{
                animator.SetBool("isFalling", false);
            }

            if(moveInput > 0){ //käännetään hahmo ympäri riippuen mihin liikutaan
                sprite.flipX = false;
            } else if(moveInput < 0){
                sprite.flipX = true;
            }

            if(rigidbody2d.velocity.y < -100){ //lopetaan peli jos pudotaan pois kentästä
                animator.SetBool("isFalling", false);
                Hit();
            }
        }
           
    }

    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");
        rigidbody2d.velocity = new Vector2(moveInput * runSpeed, rigidbody2d.velocity.y);
    }

    public Vector3 GetPosition(){
        return gameObject.transform.position;
    }

    public void Hit() { //pelaajan osumanotto komento
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
        //kerrotaan animaattori lopettamaan muut animaatiot
        audioSource.PlayOneShot(hitClip); //soitetaan osumaääni
        animator.Play("Disappearing"); //kerrotaan animttoori pelaamaan katoamis animaatio
        gameController.EndGame(); //kerrotaan pelinohjaajalle lopettaa peli
    }

    public void AddCoin(){ //kolikon lisäämis skripti
        audioSource.PlayOneShot(coinClip); //soitetaan kolikkoääni
        gameController.AddCoin(); //kerrotaan pelinohjaajalle lisäämään kolikko
    }

    IEnumerator Appear(){
        animator.Play("Appearing");

        yield return new WaitForSeconds(5);
    }
}
