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
	}

	public IEnumerator GoToRiver () {
		// Lerp to start position
		Vector3 OriginalPosition = transform.position;
		Vector3 EndPosition = new Vector3(2.03f, 0.34f, 30.129f);
		Quaternion OriginalClipboardRotation = transform.rotation;
		Quaternion EndClipboardRotation = Quaternion.Euler(100f, 192f, 96f);
		thisRigidBody.useGravity = false;
		for (float f = 0f; f < 1f; f += 0.05f){
			transform.position = Vector3.Lerp(OriginalPosition, EndPosition, f);
			transform.rotation = Quaternion.Lerp(OriginalClipboardRotation, EndClipboardRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		transform.position = EndPosition;
		transform.rotation = EndClipboardRotation;
		thisRigidBody.useGravity = true;
		// Hop to river
		thisRigidBody.velocity = new Vector3(4.5f, 2, 0);

		while (transform.position.x < 21.5f) {
			if (thisRigidBody.velocity.magnitude < 0.1f) {
				thisRigidBody.velocity = new Vector3(2.5f, 4, 0);
				thisRigidBody.angularVelocity = new Vector3(0, 0, -4);
			}
			yield return new WaitForSeconds(0.016f);
		}
		thisRigidBody.velocity = Vector3.zero;
		thisRigidBody.angularVelocity = Vector3.zero;
		MainSceneController.mainSceneController.GoToNextPhase();
	}

	public IEnumerator SwimAcrossRiver () {
		thisRigidBody.useGravity = false;
		transform.localScale = new Vector3(.06f, .06f, .06f);
		transform.position = new Vector3(25, 0.9f, 30);
		transform.rotation = Quaternion.Euler(90, 150, 230);
		yield return new WaitForSeconds(1f);

		// Lerp to other side of river
		Vector3 OriginalPosition = transform.position;
		Vector3 EndPosition = new Vector3(37f, 0.9f, 30f);
		Quaternion OriginalClipboardRotation = transform.rotation;
		Quaternion EndClipboardRotation = Quaternion.Euler(90, 150, 230);
		for (float f = 0f; f < 1f; f += 0.025f){
			transform.position = Vector3.Lerp(OriginalPosition, EndPosition, f);
			transform.rotation = Quaternion.Lerp(OriginalClipboardRotation, EndClipboardRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		thisRigidBody.velocity = Vector3.zero;
		thisRigidBody.angularVelocity = Vector3.zero;
		transform.position = EndPosition;
		transform.rotation = EndClipboardRotation;
		MainSceneController.mainSceneController.GoToNextPhase();
		transform.position = new Vector3(25, 1f, 35);
		thisRigidBody.useGravity = true;
	}
}
