using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SetPic : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	public void PrintScreen()
    {
        string dpath = Application.dataPath;
        int num = dpath.LastIndexOf("/");
        dpath = dpath.Substring(0, num);
        StartCoroutine(ScreenShoot(dpath + "/Save/ReadPixels.png"));
    }
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator ScreenShoot(string filePath)
    {
        //Wait for graphics to render
        yield return new WaitForEndOfFrame();

        //Create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        //Put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        byte[] bytes = texture.EncodeToPNG();

         //Save our test image (could also upload to WWW)
        File.WriteAllBytes(filePath, bytes);

        //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject(texture);
        Debug.Log("PrintScreen");
    }

}
