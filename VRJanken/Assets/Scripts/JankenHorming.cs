using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankenHorming : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        time += Time.deltaTime;
        
        if(time > 20f){
            Transform tmpTransform = this.transform;
            Vector3 localPos = tmpTransform.position;
            localPos.x = tmpTransform.position.x + (17.34669f - tmpTransform.position.x) * 0.001f;
            localPos.y = tmpTransform.position.y + (6.8f - tmpTransform.position.y) * 0.001f;
            localPos.z = tmpTransform.position.z + (-4.411526f - tmpTransform.position.z) * 0.001f;
            this.transform.position = localPos;
        
        }
        

        // transform.LookAt(player.transform);
        // this.GetComponent<Rigidbody>().velocity = transform.forward * power;
    }
}
