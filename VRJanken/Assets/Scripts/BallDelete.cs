using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BallDelete : MonoBehaviour
{
    public float span = 5f;
    private float currentTime = 0f;

   
    void Update () {
        currentTime += Time.deltaTime;

        if(currentTime > span){
            Destroy(gameObject);
        }
        
    }

    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Delete")){
            Destroy(gameObject);
        } 
    }
    public void EndGame(){
         Destroy(gameObject);
    }
    
}