using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiate : MonoBehaviour
{
    //instance of Enemy GameObject
    public GameObject Enemy;
    //instance of of EnemyBoss Gameobject
    public GameObject EnemyBoss;
    //Private instance array of spawn positions
    private Vector3[] spawnPos;
    //Time delay between enemy spawn
    public float spawnDelay = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Spawn positions  for 10  base enemies
        spawnPos = new Vector3[10];
        spawnPos[0] = new Vector3(5,5,0);
        spawnPos[1] = new Vector3(7,6,0);
        spawnPos[2] = new Vector3(7,8,0);
        spawnPos[3] = new Vector3(5,9,0);
        spawnPos[4] = new Vector3(9,5,0);
        spawnPos[5] = new Vector3(11,6,0);
        spawnPos[6] = new Vector3(11,8,0);
        spawnPos[7] = new Vector3(9,9,0);
        spawnPos[8] = new Vector3(13,5,0);
        spawnPos[9] = new Vector3(13,9,0);
        
        //Beging enemy and enemy boss  coroutine instantiations
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(BossCoroutine());
    }
   
    // Base enemy instantiation with 3 second intervals
     IEnumerator SpawnCoroutine(){

        
        
         for(int i=0; i<spawnPos.Length; i++){
                Instantiate(Enemy,spawnPos[i],Quaternion.identity);
                yield return new WaitForSeconds(3f);
            }
     }
     //Enemy boss instantiation with 15 second delay from start
     IEnumerator BossCoroutine(){
         yield return new WaitForSeconds(15f);
         Instantiate(EnemyBoss,spawnPos[1],Quaternion.identity);

     }


    
}
