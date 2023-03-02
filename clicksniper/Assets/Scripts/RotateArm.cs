using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public Transform rightArm;

    // Start is called before the first frame update
    void Start()
    {
        // rightArm = transform.GetChild(0).GetChild(0).GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        // rightArm.Rotate(new Vector3(0, 0, 60f));
        if (rightArm != null) {
            if (Input.GetKey (KeyCode.UpArrow)) {
            rightArm.Rotate(0f, 0f, 2f, Space.World);
            }
            if (Input.GetKey (KeyCode.DownArrow)) {
            rightArm.Rotate(0f, 0f, -2f, Space.World);
            }
            if (Input.GetKey (KeyCode.RightArrow)) {
            rightArm.Rotate(0f, 2f, 0f, Space.World);
            }
            if (Input.GetKey (KeyCode.LeftArrow)) {
            rightArm.Rotate(0f, -2f, 0f, Space.World);
            }
        
        }
    }
}
