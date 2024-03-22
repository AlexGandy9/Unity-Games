using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public int openingDirection;
    private ObstacleTemplates obsTemplates;
    private int rand;
    // Start is called before the first frame update
    void Start()
    {
        obsTemplates = GameObject.FindGameObjectWithTag("Obstacles").GetComponent<ObstacleTemplates>();
        Spawn();
    }

    // Update is called once per frame
    void Spawn(){
        if (this.gameObject.name.Contains("Trap")){
            rand = Random.Range(0, 20);
            if (rand == 0){
                rand = Random.Range(0, obsTemplates.traps.Length);
                Instantiate(obsTemplates.traps[rand], transform.position, obsTemplates.traps[rand].transform.rotation);
            }
        }else if (this.gameObject.name.Contains("Cupboard")){
            rand = Random.Range(0, 40);
            if (rand == 0){
                if (openingDirection == 1){
                    Instantiate(obsTemplates.cupboardD, transform.position, obsTemplates.cupboardD.transform.rotation);
                }else if (openingDirection == 2){
                    Instantiate(obsTemplates.cupboardT, transform.position, obsTemplates.cupboardT.transform.rotation);
                }else if (openingDirection == 3){
                    Instantiate(obsTemplates.cupboardL, transform.position, obsTemplates.cupboardL.transform.rotation);
                }else if (openingDirection == 4){
                    Instantiate(obsTemplates.cupboardR, transform.position, obsTemplates.cupboardR.transform.rotation);
                }
            }
        }else if (this.gameObject.name.Contains("Obs")){
            rand = Random.Range(0, 5);
            if (rand == 0){
                rand = Random.Range(0, obsTemplates.obs.Length);
                Instantiate(obsTemplates.obs[rand], transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.Euler(0, Random.Range(0, 360f), 0));
            }
        }
        Destroy(this.gameObject);
    }
}
