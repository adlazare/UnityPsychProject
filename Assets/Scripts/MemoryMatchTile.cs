using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMatchTile : MonoBehaviour {

	private Rigidbody thisRigidBody;
	private BoxCollider thisBoxCollider;
	public Vector3 TileEndPosition;

	// Use this for initialization
	void Start () {
		thisRigidBody = GetComponent<Rigidbody>();
		thisBoxCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (MainSceneController.gamePhase) 
		{
		case MainSceneController.GamePhase.TilesFallOutOfCeiling:
			thisRigidBody.useGravity = true;
			StartCoroutine(MoveTilesIntoPosition());
			break;
		}

	}

	private IEnumerator MoveTilesIntoPosition() {
		yield return new WaitForSeconds(5f);
		thisRigidBody.useGravity = false;
		Vector3 TileStartPosition = transform.position;
		for (float f = 0f; f < 1f; f += 0.05f){
			transform.position = Vector3.Lerp(TileStartPosition, TileEndPosition, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1f);
	}
}
