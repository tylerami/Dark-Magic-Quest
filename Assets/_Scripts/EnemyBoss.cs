using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{   
    //GameObject boss instance
    public GameObject boss;

    //Scale up per frme
    private Vector3 scaleChange;
    //Scale down variable
    private Vector3 scaleDown;

    public GameObject wand;

    //Start method
    new void Start(){
        
        // Superclass start method call
        base.Start();
        //start boss health at 10 HP
        base.setHealth(8); 
        //Scale up  0.01 per frame in x,y,z scale
        scaleChange = new Vector3(0.01f,0.01f,0.01f);
        //Scale down when x||y||z >3
        scaleDown = new Vector3(2.00f,2.00f,2.00f);
        boss.transform.localScale += scaleChange;
           
        if( boss.transform.localScale.y > 3.0f){

            boss.transform.localScale -= scaleDown;
        }

    }

    //Overriden Update method
    public override void Update()
    {
        // Set direction
       Vector3 direction = mainChar.position - transform.position;
       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       rbe.rotation = angle;
       direction.Normalize();
       // Set movement
       movement = direction;
    
       transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,lockPos,lockPos);

        // Destroy instance if health is zero
       if (health <=0)
       {
           playerScript.updateXP(0.6f);
           Instantiate(wand, transform.position, Quaternion.identity);
           Destroy(this.gameObject);
       }
    }
 }

