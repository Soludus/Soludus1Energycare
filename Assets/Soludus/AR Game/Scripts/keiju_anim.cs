using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keiju_anim : MonoBehaviour {

	SkinnedMeshRenderer fairyRenderer;

	public bool blink;
	public float randCd;

	// Use this for initialization
	void Start () {
		fairyRenderer = GetComponent<SkinnedMeshRenderer> ();
		randCd = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (randCd < 0) {
			blink = true;
			randCd = Random.Range (1f, 5f);
		}

		if (blink) {
			StartCoroutine (Blink ());
			blink = false;
		}

		randCd -= Time.deltaTime;
	}

	IEnumerator Blink(){
		fairyRenderer.SetBlendShapeWeight (1, 0);
		float i = 0;
		while (i < 100) {
			i += Time.deltaTime * 300f;
			fairyRenderer.SetBlendShapeWeight (1, i);
			yield return null;
		}
		while (i > 0) {
			i -= Time.deltaTime * 300f;
			fairyRenderer.SetBlendShapeWeight (1, i);
			yield return null;
		}
		fairyRenderer.SetBlendShapeWeight (1, 0);
	}
}
