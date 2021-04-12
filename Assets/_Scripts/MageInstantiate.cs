using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageInstantiate : MonoBehaviour
{
    //instance of Enemy GameObject
    public GameObject Mage;
    //Private instance array of spawn positions
    private Vector3[] spawnPos;
    //Time delay between enemy spawn
    public float spawnDelay = 15f;

    public GameObject player;
    private Player playerScript;
    private bool spawnStarted = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update(){
        Player playerScript = player.GetComponent<Player>();

        if (playerScript.getGameLevel() == 2 && spawnStarted == false){
            spawnStarted = true;
            spawnPos = new Vector3[3];
            spawnPos[0] = new Vector3(18, 7, 0);
            spawnPos[1] = new Vector3(20, 5, 0);
            spawnPos[2] = new Vector3(22, 7, 0);
            StartCoroutine(SpawnCoroutine());
        }

    }

        
   
    // Base enemy instantiation with 3 second intervals
     IEnumerator SpawnCoroutine(){
         for(int i=0; i<spawnPos.Length; i++){
                Instantiate(Mage,spawnPos[i],Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
     }
}
