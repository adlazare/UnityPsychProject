using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour {

	private Rigidbody thisRigidbody;
	private bool DoorOpened = false;

	// Use this for initialization
	void Start () {
		thisRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainSceneController.gamePhase == MainSceneController.GamePhase.WalkingIntoRoom){
			if (DoorOpened == false) {
				StartCoroutine(OpenMainDoor());
				DoorOpened = true;
			}
		}
	}

	private IEnumerator OpenMainDoor () {
		// Open & close door Using Lerp
		Quaternion OriginalRotation = thisRigidbody.transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(0, 15, 0);
		for (float f = 0f; f < 1f; f += 0.05f){
			thisRigidbody.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1.5f);
		for (float f = 0f; f < 1f; f += 0.05f){
			thisRigidbody.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
			yield return new WaitForSeconds(0.016f);
		}

	}
}
