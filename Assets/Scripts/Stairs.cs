using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

	public static bool StairsPlaced = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown() {
		if (StairsPlaced == false) {
			StartCoroutine(MoveStairs());
			StairsPlaced = true;
			MainSceneController.mainSceneController.StairParticleSystem.SetActive(false);
		}
	}

	public IEnumerator MoveStairs () {
		Vector3 StartPosition = transform.position;
		Quaternion StartRotation = transform.rotation;
		Vector3 EndPosition = new Vector3(64.455f, 0.33f, 31.25f);
		Quaternion EndRotation = Quaternion.Euler(0, -90, 0);
		for (float f = 0f; f < 1f; f += 0.05f){
			transform.position = Vector3.Lerp(StartPosition, EndPosition, f);
			transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		transform.position = EndPosition;
		transform.rotation = EndRotation;
		yield return new WaitForSeconds(1f);
	}
}
