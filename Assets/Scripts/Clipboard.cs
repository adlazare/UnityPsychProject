using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard : MonoBehaviour {

	private Rigidbody thisRigidBody;
	private BoxCollider thisBoxCollider;
	private bool StartedRunningAway = false;
	private bool HoppedOutOfMinigameDoor = false;

	// Use this for initialization
	void Start () {
		thisRigidBody = GetComponent<Rigidbody>();
		thisBoxCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (MainSceneController.gamePhase == MainSceneController.GamePhase.HallwayChase) {
			if (!StartedRunningAway) {
				thisRigidBody.velocity = new Vector3(0, 0, 4.5f);
				thisRigidBody.useGravity = true;
				StartedRunningAway = true;
			}
			if (transform.position.y < 0.19f) {
				thisRigidBody.velocity = new Vector3(0, 4, 2.5f);
				thisRigidBody.angularVelocity = new Vector3(2, 0, 0);
			}
			if (transform.position.z > 30) {
				thisRigidBody.velocity = new Vector3(0, 0, 0);
				thisRigidBody.angularVelocity = new Vector3(0, 0, 0);
				MainSceneController.mainSceneController.GoToNextPhase();
			}
		}

		if (MainSceneController.gamePhase == MainSceneController.GamePhase.ConcealedDoorsOpenAndClipboardRuns) {
			if (!HoppedOutOfMinigameDoor) {
				StartCoroutine(HopOutOfMinigameDoor());
//				thisRigidBody.velocity = new Vector3(0, 0, -5f);
//				thisRigidBody.useGravity = true;
//				HoppedOutOfMinigameDoor = true;
			}
//			if (transform.position.y < 5f && HoppedOutOfMinigameDoor == true) {
//				thisRigidBody.velocity = new Vector3(4, 0, 2.5f);
//				thisRigidBody.angularVelocity = new Vector3(2, 0, 0);
//			}
//			if (transform.position.x > 16 && HoppedOutOfMinigameDoor == true) {
//				thisRigidBody.velocity = new Vector3(0, 0, 0);
//				thisRigidBody.angularVelocity = new Vector3(0, 0, 0);
////				MainSceneController.mainSceneController.GoToNextPhase();
//			}
		}
	}

	private IEnumerator HopOutOfMinigameDoor () {
		thisRigidBody.useGravity = true;
		thisRigidBody.velocity = new Vector3(0, 0, -2.5f);
		yield return new WaitForSeconds(2f);
		HoppedOutOfMinigameDoor = true;
		MainSceneController.mainSceneController.GoToNextPhase();
	}
}
