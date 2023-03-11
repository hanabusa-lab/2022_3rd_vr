using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public Transform rightArm;
    // public float data[];
    public HandCntrl handCntrl;

/*
     [Serializable]
    public class GloveData
    {
        public string dat1;
        public string dat2;
    }
*/
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
            // Vector3 kero;  //①仮の変数宣言
            // kero = gameObject.transform.eulerAngles; //◆現在の向きを代入
            // kero.x = kero.x +1;  //②変数keroのx軸の向きを1増やして代入
            // rightArm.eulerAngles = kero; //③向きに変数keroを代入

            //GloveData angle = JsonUtility.FromJson<GloveData>(json);

            Vector3 worldAngle = rightArm.eulerAngles;
            worldAngle.x = 180f;//handCntrl.anglex;
            worldAngle.y = -handCntrl.angley;//handCntrl.angley;
            worldAngle.z = 180f-handCntrl.anglez;
            rightArm.eulerAngles = worldAngle;


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
