using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebCamScript : MonoBehaviour {

	WebCamTexture webCamTexture;

	public RawImage rawImage;
	private int capturedImages = 1;

	string filePath;

	// Use this for initialization
	void Start () {

		webCamTexture = new WebCamTexture ();
		rawImage.material.mainTexture = webCamTexture;
		webCamTexture.Play ();
	}

	public void StartCamera() {
		rawImage.gameObject.SetActive (true);
		webCamTexture.Play ();
		Debug.Log ("WebCam is playing");
	}

	public void TakePicture() {
		StartCoroutine (CaptureTextureAsPNG());
	}

	public void TakeScreenShot() {
		filePath = "SavedScreen" + capturedImages + ".png";
		ScreenCapture.CaptureScreenshot (filePath);
		Debug.Log (filePath + " has been saved");
	}

	IEnumerator CaptureTextureAsPNG() {

		yield return new WaitForEndOfFrame ();

		Texture2D _TextureFromCamera = new Texture2D (rawImage.material.mainTexture.width, rawImage.material.mainTexture.height);

		_TextureFromCamera.SetPixels ((rawImage.material.mainTexture as WebCamTexture).GetPixels ());
		_TextureFromCamera.Apply ();

		byte[] bytes = _TextureFromCamera.EncodeToPNG ();

		filePath = "SavedScreen" + capturedImages + ".png";
		capturedImages++;

		File.WriteAllBytes (filePath, bytes);

		Debug.Log (filePath + " has been saved");

		rawImage.gameObject.SetActive (false);
		webCamTexture.Stop ();

	}
}
