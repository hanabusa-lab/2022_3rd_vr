using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anaglyph : MonoBehaviour {
	public bool anaglyph_fg;
    public Material mt_anaglyph;
	// Use this for initialization
	void Start () {
		anaglyph_fg = false;
		
	}
	// Update is called once per frame
	void Update () {
    	
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
		if(anaglyph_fg){
        	Graphics.Blit(source, destination, mt_anaglyph);
		}else{
			Graphics.Blit(source, destination);
		}
    }
}
