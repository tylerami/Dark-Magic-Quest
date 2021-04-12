using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetimeCounter=3;
    public float lifetime=3;
    public float speed;              //reference
    public Rigidbody2D myRigidbody;  //reference

    //if the arrow flies for too long without hitting anything
    void Start()
    {
        lifetimeCounter = lifetime;
    }

    //destroy the object after time
    private void Update()
    {
        lifetimeCounter -= Time.deltaTime;
        if (lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
            lifetimeCounter += 3;
        }
    }
    
    //apply velocity
    public void Setup(Vector2 velocity, Vector3 direction)  //arguments for speed and direction
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);   //rotate
    }

    //collision with enemy to cause damage
    public void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "projectile"){
            Destroy(gameObject);}
    }
    //when the arrow hits the enemy, destroy enemy
   public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
