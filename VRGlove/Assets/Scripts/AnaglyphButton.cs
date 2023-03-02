using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.InteropServices;


public class AnaglyphButton : MonoBehaviour
{
    bool enableFg;
    GameObject camera;

    void Start()
    {
        camera = GameObject.Find("CameraLeft(Main)");
        enableFg = false;
    }
     public void OnClick()
    {
        enableFg ^= true;
        Debug.Log("anaglyph btn="+enableFg);

        if (enableFg)
        {
            GetComponentInChildren<Text>().text = "ノー立体メガネ";
            //camera.GetComponent<Anaglyph>().anaglyph_fg = true;
            camera.GetComponent<Anaglyph>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Text>().text = "立体メガネ";
            //camera.GetComponent<Anaglyph>().anaglyph_fg = false;
            camera.GetComponent<Anaglyph>().enabled = false;
        }
    }
}
