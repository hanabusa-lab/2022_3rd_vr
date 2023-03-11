using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    public GameObject AvatarCamera;
    public GameObject AvatarPerspectiveCamera;
    public GameObject MainCamera;
    private int cameraNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        AvatarCamera.SetActive(true);
        AvatarPerspectiveCamera.SetActive(false);
        MainCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void ChangeCamera(){
        cameraNum = cameraNum + 1;
        if(cameraNum > 2){
            cameraNum = 0;
        }
        if(cameraNum == 0){
            AvatarCamera.SetActive(true);
            AvatarPerspectiveCamera.SetActive(false);
            MainCamera.SetActive(false);
        }else if(cameraNum == 1){
            AvatarCamera.SetActive(false);
            AvatarPerspectiveCamera.SetActive(true);
            MainCamera.SetActive(false);
        }else {
            AvatarCamera.SetActive(false);
            AvatarPerspectiveCamera.SetActive(false);
            MainCamera.SetActive(true);
        }
    }
}
