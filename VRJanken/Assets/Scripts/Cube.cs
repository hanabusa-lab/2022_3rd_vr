using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class Cube : MonoBehaviour
{
    //画像リンクから画像をテクスチャにする
    Texture texture;
    //テクスチャをマテリアル化するので生成しておく
    [SerializeField]Material material;
    private int num = 0;
    private bool isAvatarChangeReq = false;
    private bool isAvatarChangeReqPrevious = false;
    //画像リンク
    //string url = "https://touhoucannonball.com/assets/img/character/img_008.jpg";
    string url = "http://127.0.0.1:5555/000.jpg";
    // string url = "https://drive.google.com/file/d/1jfq3PupcEhAYDnmBpBRZ16NKM0SimorF/view?usp=share_link";
    //string url = "http://drive.google.com/uc?export=view&id=1UTbyIf-WYuvqkj9J96bDUabXf2cCK59Q";

    public float span = 0.1f;
    private float currentTime = 0f;



    // https://drive.google.com/drive/folders/1oUusC4PUfioa5w-j3seRfGOkHDfnDyDW?usp=share_link
    // https://drive.google.com/file/d/1UTbyIf-WYuvqkj9J96bDUabXf2cCK59Q/view?usp=sharing

    void Start()
    {
        //先にマテリアルのシェーダを変更しておく
        string shader = "Legacy Shaders/Diffuse";
        material.shader = Shader.Find(shader);
        StartCoroutine(Connect());

        /*OnEnable();

        var reqfiles    = GoogleDriveFiles.List();
        reqfiles.Fields = new List<string> { "files(id, name, size, mimeType, createdTime)" };
        reqfiles.Q      = $"\'{"1JnVmQm2Omf1bbxwk56p52H8cNcQ7atyn"}\' in parents and trashed = false";
        reqfiles.Send().OnDone +=
    (filelist) =>
    {
        foreach (var file in filelist.Files)
        {
            Debug.Log($"{file.Name}");
        }
    };*/
}

    //テクスチャを読み込む
    private IEnumerator Connect()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError ||www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //textureに画像が入るよ
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //textureをマテリアルにセット
            material.SetTexture("_MainTex", texture);

            gameObject.GetComponent<Renderer>().material = material;
        }
    }

    public IEnumerator SearchNextTex()
    {
        if(isAvatarChangeReq){
            Debug.Log("isAvatarChangeReq");
            bool isNextTexFound = false;
        Debug.Log(num);
        while(num < 50){
            num = num + 1;
            string urlNext = "http://127.0.0.1:5555/" + num.ToString("000") + ".jpg";
            Debug.Log (urlNext);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlNext);
            yield return www.SendWebRequest();
            if (www.isNetworkError ||www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log("fail find new tex");
            }
            else
            {
                texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                material.SetTexture("_MainTex", texture);
                gameObject.GetComponent<Renderer>().material = material;
                isNextTexFound = true;
                Debug.Log("found new tex");
                break;
            }

        }
        
        if(isNextTexFound == false){
            num = 0;
            string urlNext = "http://127.0.0.1:5555/" + num.ToString("000") + ".jpg";
            Debug.Log (urlNext);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlNext);
            yield return www.SendWebRequest();
            if (www.isNetworkError ||www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                material.SetTexture("_MainTex", texture);
                gameObject.GetComponent<Renderer>().material = material;
            }
        }

        Debug.Log(isNextTexFound);
        isAvatarChangeReq = false;
        }else{
            Debug.Log("no AvatarChangeReq");
        }
        
        
    }


    public IEnumerator SearchPreviousTex()
    {
        if(isAvatarChangeReqPrevious){
            Debug.Log("isAvatarChangeReq");
            bool isPreviousTexFound = false;
        Debug.Log(num);
        while(num > 0){
            num = num - 1;
            string urlNext = "http://127.0.0.1:5555/" + num.ToString("000") + ".jpg";
            Debug.Log (urlNext);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlNext);
            yield return www.SendWebRequest();
            if (www.isNetworkError ||www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log("fail find new tex");
            }
            else
            {
                texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                material.SetTexture("_MainTex", texture);
                gameObject.GetComponent<Renderer>().material = material;
                isPreviousTexFound = true;
                Debug.Log("found new tex");
                break;
            }

        }
        
        if(isPreviousTexFound == false){
            num = 99;
            while(num > 0){
                num = num - 1;
                string urlNext = "http://127.0.0.1:5555/" + num.ToString("000") + ".jpg";
                Debug.Log (urlNext);
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(urlNext);
                yield return www.SendWebRequest();
                if (www.isNetworkError ||www.isHttpError)
                {
                    Debug.Log(www.error);
                    Debug.Log("fail find new tex");
                }
                else
                {
                    texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    material.SetTexture("_MainTex", texture);
                    gameObject.GetComponent<Renderer>().material = material;
                    isPreviousTexFound = true;
                    Debug.Log("found new tex");
                    break;
                }

            }
        }

        Debug.Log(isPreviousTexFound);
        isAvatarChangeReqPrevious = false;
        }else{
            Debug.Log("no AvatarChangeReq");
        }
        
        
    }

 

    void Update () {
        currentTime += Time.deltaTime;

        if(currentTime > span){
            Debug.LogFormat ("{0}秒経過", span);
            currentTime = 0f;
            StartCoroutine(SearchNextTex());
            StartCoroutine(SearchPreviousTex());
        }
    }

    public void ChangeAvatorNextTex()
    {
        isAvatarChangeReq = true;
    }

    public void ChangeAvatorPreviousTex()
    {
        isAvatarChangeReqPrevious = true;
    }

  
}