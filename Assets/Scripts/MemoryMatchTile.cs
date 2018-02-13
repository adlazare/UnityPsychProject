using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMatchTile : MonoBehaviour {

	private Rigidbody thisRigidBody;
	private BoxCollider thisBoxCollider;
	private GameObject PreviousTile;
	public GameObject DissolveParticleSystem;
	public GameObject Tile1ParticleSystem;
	public Vector3 TileEndPosition;
	public static int ClickNum = 1;
	public static int MatchCount = 0;
	public static string Tile1Type = "";
	public static string Tile2Type = "";
	public static string Tile1Name = "";
	public static string Tile2Name = "";
	public static string Tile1ParticleSystemName = "";
	public static bool flag1 = false;

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

	void OnMouseDown () 
	{
		Debug.Log(ClickNum);
		if (ClickNum == 1) {
			ClickNum = 2;
			Tile1Type = transform.GetChild(0).name;
			Tile1Name = name;
			Tile1ParticleSystemName = transform.GetChild(1).name;
			StartCoroutine(FlipTile());
		}
		else if(ClickNum == 2 && Tile1Name != name) {
			ClickNum = 1;
			Tile2Type = transform.GetChild(0).name;
			Tile2Name = name;
			PreviousTile = GameObject.Find(Tile1Name);
//			Tile1ParticleSystem = GameObject.Find(Tile1ParticleSystemName);
			Debug.Log(Tile1Type + "---" + Tile2Type);
			Debug.Log(Tile1Name + "---" + Tile2Name);
//			Debug.Log(Tile1ParticleSystemName);
			StartCoroutine(FlipTile2());
		}
	}

	private IEnumerator MoveTilesIntoPosition() {
		yield return new WaitForSeconds(2f);
		thisRigidBody.useGravity = false;
		Vector3 TileStartPosition = transform.position;
		for (float f = 0f; f < 1f; f += 0.05f){
			transform.position = Vector3.Lerp(TileStartPosition, TileEndPosition, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(2f);
		thisRigidBody.useGravity = true;
		thisRigidBody.isKinematic = true;
		if (flag1 == false) {
			MainSceneController.mainSceneController.GoToNextPhase();
			flag1 = true;
		}
	}

	private IEnumerator FlipTile () {
		Quaternion StartRotation = transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(180, transform.rotation.z, transform.rotation.y);
		for (float f = 0f; f < 1f; f += 0.1f){
			transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		transform.rotation = EndRotation;
	}

	private IEnumerator FlipTile2 () {
		Quaternion StartRotation = transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(180, transform.rotation.z, transform.rotation.y);
		for (float f = 0f; f < 1f; f += 0.1f){
			transform.rotation = Quaternion.Lerp(StartRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		transform.rotation = EndRotation;

		yield return new WaitForSeconds(1f);

		if (Tile1Type == Tile2Type) {
			MatchCount += 1;
			Tile1ParticleSystem.SetActive(true);
			DissolveParticleSystem.SetActive(true);
			yield return new WaitForSeconds(1f);
			transform.gameObject.SetActive(false);
			PreviousTile.SetActive(false);
		}
		else if (Tile1Type != Tile2Type) {
			Quaternion StartRotation2 = transform.rotation;
			Quaternion EndRotation2 = Quaternion.Euler(0, transform.rotation.z, transform.rotation.y);
			Quaternion StartRotationPreviousTile = PreviousTile.transform.rotation;
			Quaternion EndRotationPreviousTile = Quaternion.Euler(0, PreviousTile.transform.rotation.z, PreviousTile.transform.rotation.y);
			for (float f = 0f; f < 1f; f += 0.1f){
				transform.rotation = Quaternion.Lerp(StartRotation2, EndRotation2, f);
				PreviousTile.transform.rotation = Quaternion.Lerp(StartRotationPreviousTile, EndRotationPreviousTile, f);
				yield return new WaitForSeconds(0.016f);
			}
			transform.rotation = EndRotation2;
			PreviousTile.transform.rotation = EndRotationPreviousTile;
		}

		if (MatchCount == 6) {
			MainSceneController.mainSceneController.GoToNextPhase();
			MainSceneController.mainSceneController.MedKit.SetActive(true);
		}
	}
}
