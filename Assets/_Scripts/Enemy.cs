using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variable instanves
    public float moveSpeed;

    public Rigidbody2D rbe;

    public Transform player;

    public Vector2 movement;

    public float lockPos;

    public int health;

    protected bool knockback;

    public Transform mainChar;
    private bool takingDamage;
    private GameObject playerObject;
    public Player playerScript;

    // Singleton 
    public static Enemy _instance;
    public static Enemy Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Enemy>();
                
                if (_instance == null)
                {
                    GameObject container = new GameObject("Enemy");
                    _instance = container.AddComponent<Enemy>();
                }
            }
        
            return _instance;
        }
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        // Lock enemy to point at enemy transform
        rbe  = this.GetComponent<Rigidbody2D>();
        // Set player transform reference
        mainChar = GameObject.FindGameObjectWithTag("Player").transform;
        // Set health
        health = 2;
        // Set status booleans
        takingDamage = false;
        knockback = false;
        // Set player game object 
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // Set player script
        playerScript = playerObject.GetComponent<Player>();
    }


    // Update is called once per frame
    public virtual void Update()
    {
        // If enemy isn't being knocked back
        if (!knockback){
            // Move enemy towards player
            Vector3 direction = mainChar.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rbe.rotation = angle;
            direction.Normalize();
            movement = direction;
        }

       transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,lockPos,lockPos);

       // If health is 0, destroy instance 
       if (health <=0)
       {
           playerScript.updateXP(0.2f);
           Destroy(this.gameObject);
       }
    }
    
    protected void moveCharacter(Vector2 direction)
    {
        // If enemy isn't being knocked back
        if (!knockback){
            // Move enemy towards player
            rbe.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        }
    }

    public void setHealth(int h){
        // Set health
        health = h;
    }
   
    private IEnumerator Flasher() 
         { 
             // Set status boolean
             takingDamage = true;
             // Flash opacity four times in a loop
             for (int i = 0; i < 4; i++)
             {
              GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.3f);
              yield return new WaitForSeconds(.1f);
              GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
              yield return new WaitForSeconds(.1f);
              GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.3f);
              yield return new WaitForSeconds(.1f);
              GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
              yield return new WaitForSeconds(.1f);
             }
             // Reset status boolean
             takingDamage = false;
          }

    protected void takeDamage(){
        // Decrement health
        health -= 1;
        // Flash opacity
        StartCoroutine(Flasher());
    }

    protected virtual void FixedUpdate()
    {
        // Move enemy
        moveCharacter(movement);
    }

    private IEnumerator Knockback(){
        // Set status boolean
        knockback = true;
        // Get rigid body references
        Rigidbody2D playerBody = playerObject.GetComponent<Rigidbody2D>();
        Rigidbody2D rgb = gameObject.GetComponent<Rigidbody2D>();
        // Ensure enemy isn't kinematic
        rgb.isKinematic = false;
        // Set direction
        Vector2 difference = transform.position - playerBody.transform.position;
        // Normalize and set force vector
        difference = difference.normalized / 4;
        // Add force
        rgb.AddForce(difference, ForceMode2D.Impulse);
        // Add delay before resetting status boolean
        yield return new WaitForSeconds(0.1f);
        // Reset status boolean
        knockback = false;
    }

    private void OnCollisionEnter2D(Collision2D col){
        // Collision with arrow
        if (col.gameObject.tag == "projectile"){
            // If enemy isn't taking damage
            if (!takingDamage){
                // Take damage
                takeDamage();
            }
        }
        // Collision with player
        if (col.gameObject.tag == "Player"){
            // If player's melee attack is active and enemy isn't taking damage
            if (playerScript.getMeleeState() && !takingDamage){
                // Take damage
                takeDamage();
                // If gold armour is active
                if (col.gameObject.GetComponent<Rigidbody2D>() != null && playerScript.IsGold())
                {
                    // Initiate enemy knockback
                   StartCoroutine(Knockback());
                }
            }
        }
    }
}
