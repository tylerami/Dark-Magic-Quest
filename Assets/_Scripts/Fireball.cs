using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    public Player player;
    public GameObject playerObj;

    new void Start(){
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();
    }

    public new void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag != "mage"){
            Destroy(gameObject);}
        if (col.gameObject.tag == "Player"){
            player.takeDamage();
        }
    }

    public new void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "energyball"){
            Destroy(gameObject);
        }
    }
}
