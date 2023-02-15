using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCntrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb =this.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
        rb.velocity = new Vector3(0f, 0f, -2f);
        //this.GetComponent<RigidBody>.velocity = new Vector3(0f, 0f, -1f);
        this.transform.position = new Vector3(0f, 0f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible(){
        Destroy(this.gameObject);
        Debug.Log("destroyed"); 
    }

    private void OnCollisionEnter(Collision collision)
	{
		// 当たった相手のRigidbodyのx軸方向に力を加える。
		// 今回はx軸のマイナス方向に力を加えて跳ね返す。
        //VRGloveに当たった場合には、落ちる。
        if(collision.gameObject.tag =="VRGlove"){
	        Rigidbody rb =this.transform.GetComponent<Rigidbody> ();//.set_velocityvelocity();
    	    rb.AddForce(0f, 0f, 1f, ForceMode.Impulse);
            rb.useGravity = true;
        }
    }
}
