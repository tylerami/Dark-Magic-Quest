using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //declaration of all required variables
    public Vector2 TeleportPosition;
    public Vector2 CaveExit;
    public Vector2 HutEnter;
    public Vector2 HutExit;
    public Vector2 bossSpawn;
    public Rigidbody2D rb;

    public Text healthText;
    public Text movementText;
    public Text shieldText;
    public Text teleportationText;
    public Text infoText;

    public Text LevelText;
    public Image XPBar;
    private bool increasingXP;
    private float xp;
    private float XPincrease;

    private int health;
    private int movement;
    private int shield;
    private int level;
    private float teleportationCooldown;
    private bool teleportationEnabled;
    private bool takingDamage;
    private bool isBlue;
    private bool isGold;
    private int gameLevel = 1;
    
    Animator anim;
    Animator energyBallAnimator;
    
    private float input_x;
    private float input_y;
    private bool isWalking;
    private bool isMelee = false;
    private bool isShoot = false;
    private bool isCharge = false;
    private bool chargeReady = false;
    private bool canMove = true;
    private bool castSpells = false;
    public GameObject boss;
    public GameObject projectile;
    public GameObject energyBall;
    private EnergyBall ball;

    public AnimatorOverrideController blue;
    public AnimatorOverrideController gold;

    
    public void Blue(){

        GetComponent<Animator>().runtimeAnimatorController = blue as RuntimeAnimatorController;
    }

    void Start()
    {
        //sets all variables needed that the begining including gui stats and current armor
        // GetComponent<ChangeSkin>().Gold();
        health = 10;
        movement = 3;
        shield = 3;
        level = 1;
        xp = 0.0f;
        teleportationCooldown = 0;
        teleportationEnabled = true;
        anim = GetComponent<Animator>();
        this.takingDamage = false;
        isBlue = false;
        isGold = false;
        chargeReady = false;
        infoText.text = "";
        XPincrease = 0f;
        LevelText.text = level.ToString();
        XPBar.fillAmount = 0.0f;
        updateXP(0.0f);
    }
        
    // Update is called once per frame
    void Update()
    {
        //reduces cooldown on teleport after its used
        setGUIText();
        if (teleportationCooldown > 0){
            teleportationCooldown -= Time.deltaTime;
        }
        GetInput();
        if (canMove){
            MovePlayer();
            CheckTeleport();
        }
        //if players health hits zero the level will restart
        if (health <= 0){
            SceneManager.LoadScene("Scene1");
        }

        checkXP();
        GameObject eball = GameObject.FindGameObjectWithTag("energyball");
        if (eball != null && ball == null ){
            Rigidbody2D body = eball.GetComponent<Rigidbody2D>();
                if (body.velocity.magnitude < 1.0f){
                       Destroy(eball);
                }
        }
    }

    void checkXP(){
        if (increasingXP == true){
            XPBar.fillAmount += XPincrease * Time.deltaTime;
            if (XPBar.fillAmount >= 1.0f && XPBar.fillAmount <= xp + XPincrease){
                    XPincrease = xp + XPincrease - 1.0f;
                    xp = 0f;
                    level += 1;
                    LevelText.text = level.ToString();
                    XPBar.fillAmount = 0f;
            }
            if (XPBar.fillAmount >= xp + XPincrease){
                xp = xp + XPincrease;
                XPincrease = 0f;
                increasingXP = false;
            }
        }
    }


    void setGUIText(){
        //sets above variables to the gui
        healthText.text = health.ToString() + " / 10";
        movementText.text = movement.ToString() + " / 10";
        shieldText.text = shield.ToString() + " / 10";
        teleportationText.text = ((int) teleportationCooldown).ToString();
    }

    public void updateXP(float newXP){
        increasingXP = true;
        XPincrease += newXP;
    }

    private void GetInput()
    {
        //gets the inputs for movement and attacks
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown("return") && !isMelee)
        {
            StartCoroutine(AttackCo());
        }
        else if(Input.GetButtonDown("Second Weapon") && !isShoot)
        {
            StartCoroutine(SecondAttackCo());
        }
        if (castSpells){
            checkSpellCast();
        }
        
        isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;
    }

    private void checkSpellCast(){
        if(Input.GetKeyDown(KeyCode.RightShift) && isCharge == false){
                 isCharge = true;
                 StartCoroutine(chargeEnergyball());
        }


        if(Input.GetKeyUp(KeyCode.RightShift)){
            canMove = true;
            if (chargeReady == true && isCharge == true && ball != null){
                    energyBallAnimator = ball.GetComponent<Animator>();
                    energyBallAnimator.SetBool("release", true);
                    StartCoroutine(launchEnergyball());
            } else {
                    GameObject obj1 = GameObject.FindGameObjectWithTag("energyball");
                    Destroy(obj1);
                    isCharge = false;
                    GameObject obj2 = GameObject.FindGameObjectWithTag("energyball");
                    Destroy(obj2);
                    isCharge = false;
              }
            chargeReady = false;
        }
        if (ball != null && Input.GetKey(KeyCode.RightShift) == false){
            GameObject obj1 = GameObject.FindGameObjectWithTag("energyball");
            Destroy(obj1);
            isCharge = false;
        }
    }

      private IEnumerator chargeEnergyball(){
        canMove = false;
        float tempx = transform.position.x + anim.GetFloat("x") * 0.25f;
        float tempy = transform.position.y + anim.GetFloat("y") * 0.25f;
        Vector3 temp = new Vector3(tempx, tempy, transform.position.z);
        ball = Instantiate(energyBall, temp, Quaternion.identity).GetComponent<EnergyBall>();
        if (ball != null && Input.GetKey(KeyCode.RightShift) == false){
            GameObject obj1 = GameObject.FindGameObjectWithTag("energyball");
                    Destroy(obj1);
                    isCharge = false;
        }
        yield return new WaitForSeconds(0.32f);
        if (ball != null && Input.GetKey(KeyCode.RightShift) == false){
            GameObject obj1 = GameObject.FindGameObjectWithTag("energyball");
                    Destroy(obj1);
                    isCharge = false;
        }
        if (ball != null && Input.GetKey(KeyCode.RightShift)){
            energyBallAnimator = ball.GetComponent<Animator>();
            energyBallAnimator.SetBool("grow", false);
            chargeReady = true;
        }
        
    }

    private IEnumerator launchEnergyball(){
        canMove = true;
        Vector2 temp = new Vector2(anim.GetFloat("x"), anim.GetFloat("y"));
        ball.Setup(temp, ChooseProjDirection());
        ball = null;
        energyBallAnimator = null;
        chargeReady = false;
        yield return new WaitForSeconds(2f);
        isCharge = false;
    }



    private IEnumerator AttackCo(){
        //sets a cooldown for melee attack 
        anim.SetBool("melee", true);
        isMelee = true;
        yield return null;
        anim.SetBool("melee", false);
        yield return new WaitForSeconds(.05f);
        isMelee = false;
    }

    private IEnumerator SecondAttackCo()
    {
        //fires arrows, if the blue armor is equipped fire 3 arrows instead
        //anim.SetBool("shoot", true);
        isShoot = true;
        yield return null;
        MakeArrow();
        if (isBlue){
            MultiArrow();
        }
       // anim.SetBool("shoot", false);
        yield return new WaitForSeconds(.33f);
        isShoot = false;
    }

    private void MakeArrow()
    {
        Vector2 temp = new Vector2(anim.GetFloat("x"), anim.GetFloat("y")); //use movement to do arrow direction
        Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>(); //creating arrow and ref to script
        arrow.Setup(temp, ChooseProjDirection());  //set up arrow with direction
    }

    private void MultiArrow(){
        Vector2 temp1 = new Vector2(anim.GetFloat("x"), anim.GetFloat("y")); //use movement to do arrow direction
        Arrow arrow1 = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>(); //creating arrow and ref to script
        float tempFloat1 = Mathf.Atan2(anim.GetFloat("y"), anim.GetFloat("x")) * Mathf.Rad2Deg;
        Vector3 upDirection = new Vector3(20, 0, tempFloat1);
        arrow1.Setup(temp1, upDirection);
        Vector2 temp2 = new Vector2(anim.GetFloat("x"), anim.GetFloat("y")); //use movement to do arrow direction
        Arrow arrow2 = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>(); //creating arrow and ref to script
        float tempFloat2 = Mathf.Atan2(anim.GetFloat("y"), anim.GetFloat("x")) * Mathf.Rad2Deg;
        Vector3 downDirection = new Vector3(-20, 0, tempFloat2);
        arrow2.Setup(temp2, downDirection);
    }

  
    
    //use the directions to shoot where facing
    Vector3 ChooseProjDirection()
    {
        float temp = Mathf.Atan2(anim.GetFloat("y"), anim.GetFloat("x")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    


    public bool getMeleeState(){
        return isMelee;
    }
//checks if teleport is available if so searches for the space bar as well as a direction 
//moves character 1 unit in the direction being held
    private void CheckTeleport(){
        if (Mathf.Floor(teleportationCooldown) == 0){
            teleportationEnabled = true;
        }
        else {teleportationEnabled = false;}
        Vector3 pos = transform.position;
        if ((Input.GetKey(KeyCode.A) || Input.GetKey("left")) && Input.GetKeyDown("space") && teleportationEnabled)
        {
            teleportationCooldown = 15;
            pos.x += -1;
            transform.position = pos;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey("right")) && Input.GetKeyDown("space") && teleportationEnabled)
        {
            teleportationCooldown = 15;               
            pos.x += 1;
            transform.position = pos;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey("up")) && Input.GetKeyDown("space") && teleportationEnabled)
        {
            teleportationCooldown = 15;
            pos.y += 1;
            transform.position = pos;
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey("down")) && Input.GetKeyDown("space") && teleportationEnabled)
        {
            teleportationCooldown = 15;
            pos.y += -1;
            transform.position = pos;
        }
    }
    
    
    private void MovePlayer()
    //player movement, speed is affected by the movement stat
    {
        anim.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            float multiplier = (float) movement / 6f;
            anim.SetFloat("x", input_x);
            anim.SetFloat("y", input_y);
           // rb.velocity = new Vector2(input_x, input_y).normalized;
           Vector3 pos = transform.position;
           pos.x += input_x * Time.deltaTime * multiplier;
           pos.y += input_y * Time.deltaTime * multiplier;
           transform.position = pos;
           

//transform.position += new Vector3(input_x, input_y, 0.0f).normalized * Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(0.0f,0.0f);
        }
    }

    public void takeDamage(){
        float multiplier = (float) shield / 2f; //takes damage in respect to the sheild stat of the player
        float damage = 6f - multiplier;
        Debug.Log(damage);
        health -= (int) damage;
        StartCoroutine(Flasher());
    }
    //after the player takes damage flash temporarily and cannot take damage
    private IEnumerator Flasher() 
         {
             takingDamage = true;
             for (int i = 0; i < 5; i++)
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
             takingDamage = false;
          }

    public int getGameLevel(){
        return gameLevel;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "DoorSilo" && isWalking && level >= 2)//if the player hits the isTrigger with a certain tag its will teleport to a set location
        {
            transform.position = TeleportPosition;
            gameLevel = 2;
            Debug.Log("Entering " + collider.ToString());
        } else if (collider.tag == "DoorSilo"){
            StartCoroutine(displayMessage("Must be level 2 to enter!"));
        }
        if (collider.tag == "Hut_Enter" && isWalking)
        {
            transform.position = HutEnter;
            Debug.Log("Entering " + collider.ToString());
        }
        if (collider.tag == "Hut_exit" && isWalking)
        {
            transform.position = HutExit;
            Debug.Log("Entering " + collider.ToString());
        }

        if (collider.tag == "CaveDoor" && isWalking)
        {
            transform.position = CaveExit;
            Debug.Log("Entering " + collider.ToString());
        }
        if (collider.tag == "wand" && isWalking){
            GameObject wand = GameObject.FindGameObjectWithTag("wand");
            Destroy(wand);
            castSpells = true;
        }
        if (collider.tag == "SpawnDragon" && isWalking)
        {
            Instantiate(boss, bossSpawn, Quaternion.identity);
            Debug.Log("Boss has spawned " + collider.ToString());
        }
        
        //Debug.Log(collider.tag);
        Debug.Log("Trigger Detected!");
    }

    public IEnumerator displayMessage(string message){
        infoText.text = message;
        yield return new WaitForSeconds(1.5f);
        infoText.text = "";
    }

    public bool IsGold(){
        return isGold;
    }

    public void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "enemy" && !isMelee && !takingDamage){//player will take damage oncollision with enemy if not attacking or flashing
            takeDamage();
            /* Rigidbody2D hit = col.gameObject.GetComponent<Rigidbody2D>();
             Rigidbody2D rgb = gameObject.GetComponent<Rigidbody2D>();
                if (hit != null && IsGold())
                {
                    print("hi2");
                    hit.isKinematic = false;
                    Vector2 difference = transform.position - hit.transform.position;
                    difference = difference.normalized * 5;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                    hit.isKinematic = true;
                }*/
        }
        if (col.gameObject.tag == "BlueArmor" && isWalking)//colliding with blue armor will change stats and animations
        {
            GetComponent<Animator>().runtimeAnimatorController = blue;
            //collider.transform.position = TeleportPosition;
            isBlue = true;
            isGold = false;
            movement = 8;
            shield = 6;
            Debug.Log("Armor " + GetComponent<Collider>().ToString());
        }
        if (col.gameObject.tag == "GoldArmor" && isWalking)//colliding with gold armor will change stats and animations
        {
            GetComponent<Animator>().runtimeAnimatorController = gold;
            //collider.transform.position = TeleportPosition;
            movement = 6;
            shield = 8;
            isGold = true;
            isBlue = false;
            Debug.Log("Armor " + GetComponent<Collider>().ToString());
            Debug.Log("Collision Detected!");
        }
    }

    //test push 
}
