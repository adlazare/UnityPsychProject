using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainSceneController : MonoBehaviour {

	public static MainSceneController mainSceneController;
	public GameObject Kira, Liam, Jeff;
	private GameObject ChosenCharacter;
	public GameObject SlidingTile;
	public GameObject Clipboard;
	public GameObject BackWall;
	public GameObject MinigameDoor2;
	private Rigidbody MinigameDoor2RigidBody;
	private Rigidbody ClipboardRigidBody;
	public GameObject IntroUI;
	public GameObject DialogueUI;
	public enum GamePhase {StartMenuShowing, WalkingIntoRoom, TurnLeft, Tutorial1, Tutorial1Walking, Dialogue1, Dialogue2, Dialogue3, Dialogue4, ClipboardFalling, Dialogue5, HallwayChase, WaitingForPlayer, ClipboardHopToDoor, ThreeDoorShuffle};
	public static GamePhase gamePhase = GamePhase.StartMenuShowing;
	public static bool ControlsEnabled = false;
	public GameObject DialogueText;
	private Text DialogueTextComponent;


	// Use this for initialization
	void Start () {
		mainSceneController = this;
		DialogueTextComponent = DialogueText.GetComponent<Text>();
		ClipboardRigidBody = Clipboard.GetComponent<Rigidbody>();
		MinigameDoor2RigidBody = MinigameDoor2.GetComponentInChildren<Rigidbody>();
		int rand = Random.Range(1,4);
		if (rand == 1) {Kira.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Liam;}
		if (rand == 2) {Liam.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Kira;}
		if (rand == 3) {Liam.SetActive(false); Kira.SetActive(false); ChosenCharacter = Jeff;}
//		Kira.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Liam;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartGame () {
		GoToNextPhase();
	}

	public void GoToNextPhase () {
		switch (gamePhase)
		{
		case GamePhase.StartMenuShowing:
			IntroUI.SetActive(false);
			gamePhase = GamePhase.WalkingIntoRoom;
			break;
		case GamePhase.WalkingIntoRoom:
			gamePhase = GamePhase.TurnLeft;
			break;
		case GamePhase.TurnLeft:
			gamePhase = GamePhase.Tutorial1;
			DialogueUI.SetActive(true);
			break;
		case GamePhase.Tutorial1:
			DialogueUI.SetActive(false);
			ControlsEnabled = true;
			gamePhase = GamePhase.Tutorial1Walking;
			StartCoroutine(WaitForPlayerToWalk());
			break;
		case GamePhase.Tutorial1Walking:
			gamePhase = GamePhase.Dialogue1;
			ControlsEnabled = false;
			DialogueUI.SetActive(true);
			// Warp the character to center
			ChosenCharacter.transform.position = new Vector3(0,ChosenCharacter.transform.position.y, 0);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
			DialogueTextComponent.text = "Huh, didn't know I'd be working with a partner. You here for the video game experiment too?";
			break;
		case GamePhase.Dialogue1:
			gamePhase = GamePhase.Dialogue2;
			DialogueTextComponent.text = "Will that always happen when I ask you a question?";
			break;
		case GamePhase.Dialogue2:
			gamePhase = GamePhase.Dialogue3;
			DialogueTextComponent.text = "Alright, good to know.";
			break;
		case GamePhase.Dialogue3:
			gamePhase = GamePhase.Dialogue4;
			DialogueTextComponent.text = "What do you think they want us to do?";
			ChosenCharacter.transform.position = new Vector3(0,ChosenCharacter.transform.position.y, 0);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
			break;
		case GamePhase.Dialogue4:
			gamePhase = GamePhase.ClipboardFalling;
			DialogueUI.SetActive(false);
			StartCoroutine(ClipboardFall());
			break;
		case GamePhase.ClipboardFalling:
			gamePhase = GamePhase.Dialogue5;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Clipboard: You can't leave 'til you catch me, guinea pig.";
			break;
		case GamePhase.Dialogue5:
			gamePhase = GamePhase.HallwayChase;
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			ControlsEnabled = true;
			DialogueUI.SetActive(false);
			StartCoroutine(BackWallSlideUp());
			break;
		case GamePhase.HallwayChase:
			gamePhase = GamePhase.WaitingForPlayer;
			break;
		case GamePhase.WaitingForPlayer:
			gamePhase = GamePhase.ClipboardHopToDoor;
			ControlsEnabled = false;
			// Unparent camera from character & fix it's position
			Camera.main.transform.parent = null;
			Camera.main.transform.position = new Vector3(0, 1, 27);
			Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
			ChosenCharacter.transform.position = new Vector3(-1f, ChosenCharacter.transform.position.y, 28f);
			GoToNextPhase();
			break;
		case GamePhase.ClipboardHopToDoor:
			gamePhase = GamePhase.ThreeDoorShuffle;
			StartCoroutine(MinigameDoor2Open());

			break;
		case GamePhase.ThreeDoorShuffle:
			break;
		}
	}

	private IEnumerator WaitForPlayerToWalk () {
		yield return new WaitForSeconds(1);
		GoToNextPhase();
	}

	private IEnumerator ClipboardFall () {
		for (int i = 0; i < 30; i++) {
			SlidingTile.transform.position = SlidingTile.transform.position + new Vector3(0, 0, -0.05f);
			yield return new WaitForSeconds(0.016f);
		}
		ClipboardRigidBody.useGravity = true;
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 30; i++) {
			SlidingTile.transform.position = SlidingTile.transform.position + new Vector3(0, 0, 0.05f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1);

		// Move the clipboard to the camera
		Vector3 OriginalPosition = Clipboard.transform.position;
		Vector3 EndPosition = new Vector3(0.292f, 0.856f, -1.55f);
		Quaternion OriginalRotation = Clipboard.transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(85.466f, 287.293f, 86.645f);
		ClipboardRigidBody.useGravity = false;
		for (float f = 0f; f < 1f; f += 0.05f){
			Clipboard.transform.position = Vector3.Lerp(OriginalPosition, EndPosition, f);
			Clipboard.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		ClipboardRigidBody.velocity = Vector3.zero;
		ClipboardRigidBody.angularVelocity = Vector3.zero;
		Clipboard.transform.position = EndPosition;
		Clipboard.transform.rotation = EndRotation;
		GoToNextPhase();
	}

	private IEnumerator BackWallSlideUp () {
		for (int i = 0; i < 45; i++) {
			BackWall.transform.position = BackWall.transform.position + new Vector3(0, 0.05f, 0);
			yield return new WaitForSeconds(0.016f);
		}

	}

	private IEnumerator MinigameDoor2Open () {
		// Open & close MinigameDoor2 Using Lerp

		Quaternion OriginalRotation = MinigameDoor2RigidBody.transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(0, 120, 0);
		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoor2RigidBody.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1.5f);
		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoor2RigidBody.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
	}

}
