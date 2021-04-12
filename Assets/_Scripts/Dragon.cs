using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy 
{
    public GameObject dragon;
    private bool isLaunching;
    public Animator anim;
    public GameObject projectile;
    
    
    // Start is called before the first frame update
    new void Start(){
        
        // Superclass start method call
        base.Start();
        //start boss health at 10 HP
        setHealth(10);
        isLaunching = false;
        anim = gameObject.GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isLaunching == false)
        {
            StartCoroutine(launchFireball());
        }
        
    }
    private new void takeDamage(){
        // Decrement health
        health -= 1;
       
    }
   private IEnumerator launchFireball(){
        isLaunching = true;
        
        for( int i = 0; i < 5; i++){
            yield return new WaitForSeconds(.5f);
            Vector3 dir3 = mainChar.position - transform.position;
            Vector2 dir2 = new Vector3(dir3.x, dir3.y);
            Projectile fireball = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>(); //creating arrow and ref to script
            fireball.Setup(dir2, ChooseProjDirection());  //set up arrow with direction
        }
        yield return new WaitForSeconds(3f);
        isLaunching = false;
    }
    
    Vector3 ChooseProjDirection()
    {
        Vector3 dir3 = mainChar.position - transform.position;
        float temp = Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void OnTriggerEnter2D(Collider2D col){
        if (health > 1)
        {
            if (col.gameObject.tag == "energyball"||col.gameObject.tag == "projectile" )
            {
                takeDamage();//dragon can only be killed by the energy ball
            }
        }
        else if (health <= 1)
        {
            if (col.gameObject.tag == "energyball")
            {
                Destroy(gameObject);
                playerScript.updateXP(1f);
            }
        }
        
    }

}
