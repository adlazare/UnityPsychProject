using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainSceneController : MonoBehaviour {

	public static MainSceneController mainSceneController;
	public GameObject Kira, Liam, Jeff;
	private GameObject ChosenCharacter;
	public GameObject SlidingTile;
	public GameObject GOClipboard;
	private Clipboard ClipboardScript;
	public GameObject BackWall;
	public GameObject MinigameDoor1;
	public GameObject MinigameDoor2;
	public GameObject MinigameDoor3;
	public GameObject MinigameDoors;
	public GameObject MinigameDoorParent1, MinigameDoorParent2, MinigameDoorParent3;
	public GameObject DoorMinigameBackWall;
	public GameObject DoorMinigameBackWallBoundary;
	public GameObject DoorMinigameConcealedWallLeft;
	public GameObject DoorMinigameConcealedWallRight;
	public GameObject MinigameDoorsCollider;
	public GameObject DoorParticleSystem1, DoorParticleSystem2, DoorParticleSystem3;
	public GameObject RockParticleSystem1, RockParticleSystem2, RockParticleSystem3, RockParticleSystem4;
	public GameObject FireWallParticleSystem;
	private Rigidbody ClipboardRigidBody;
	private Rigidbody MinigameDoorsRigidBody;
	public GameObject IntroUI;
	public GameObject DialogueUI;
	public GameObject Dialogue2UI;
	public enum GamePhase {StartMenuShowing, WalkingIntoRoom, TurnLeft, Tutorial1, Tutorial1Walking, Dialogue1, Dialogue2, Dialogue3, Dialogue4, ClipboardFalling, Dialogue5, HallwayChase, WaitingForPlayer, ClipboardHopToDoor, ThreeDoorShuffle, PlayerChoosesDoor, ClipboardAppears, ConcealedDoorsOpenAndClipboardRuns, ClipboardHeadingToRiver, WaitingForPlayerAtRiverBank, ClipboardSwimsAcrossRiver, RiverMinigameSetup, RiverCrossing, ClipboardRunsToFireMinigame, WaitingForPlayerAtFireArea, DialogueInterruptedByFireWall, StairsAppear};
	public static GamePhase gamePhase = GamePhase.StartMenuShowing;
	public static bool ControlsEnabled = false;
	public GameObject DialogueText; 
	public GameObject Dialogue2Text;
	public GameObject Dialogue2Button1Text, Dialogue2Button2Text;
	private Text DialogueTextComponent;
	private Text Dialogue2TextComponent;
	private Text Dialogue2Button1TextComponent, Dialogue2Button2TextComponent;
	private static int ClipboardDoorNumber = 0;


	// Use this for initialization
	void Start () {
		mainSceneController = this;
		DialogueTextComponent = DialogueText.GetComponent<Text>();
		Dialogue2TextComponent = Dialogue2Text.GetComponent<Text>();
		Dialogue2Button1TextComponent = Dialogue2Button1Text.GetComponent<Text>();
		Dialogue2Button2TextComponent = Dialogue2Button2Text.GetComponent<Text>();
		ClipboardRigidBody = GOClipboard.GetComponent<Rigidbody>();
		ClipboardScript = GOClipboard.GetComponent<Clipboard>();
		MinigameDoorsRigidBody = MinigameDoors.GetComponent<Rigidbody>();
		int rand = Random.Range(1,4);
		ClipboardDoorNumber = Random.Range(1,4);
		Debug.Log(ClipboardDoorNumber);
		if (rand == 1) {Kira.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Liam;}
		if (rand == 2) {Liam.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Kira;}
		if (rand == 3) {Liam.SetActive(false); Kira.SetActive(false); ChosenCharacter = Jeff;}

		// TESTING TESTING TESTING
//		gamePhase = GamePhase.WaitingForPlayer;
//		gamePhase = GamePhase.ClipboardAppears;
//		GoToNextPhase();
//		Kira.SetActive(false); Jeff.SetActive(false); ChosenCharacter = Liam;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartGame () {
		GoToNextPhase();
	}

	public void GoToNextPhase () {
		Debug.Log(gamePhase);
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
			IntroUI.SetActive(false);
			Dialogue2UI.SetActive(false);
			ControlsEnabled = false;
			// Unparent camera from character & fix it's position
			Camera.main.transform.parent = null;
			Camera.main.transform.position = new Vector3(0, 1, 27);
			Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
			ChosenCharacter.transform.position = new Vector3(-1f, ChosenCharacter.transform.position.y, 28f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
			GoToNextPhase();
			break;
		case GamePhase.ClipboardHopToDoor:
			gamePhase = GamePhase.ThreeDoorShuffle;
			StartCoroutine(MinigameDoorSetup());
			break;
		case GamePhase.ThreeDoorShuffle:
			gamePhase = GamePhase.PlayerChoosesDoor;
			ControlsEnabled = false;
			StartCoroutine(SpinDoors());
			DoorMinigameBackWall.SetActive(true);
			DoorMinigameBackWallBoundary.SetActive(true);
			break;
		case GamePhase.PlayerChoosesDoor:
			gamePhase = GamePhase.ClipboardAppears;
			Camera.main.transform.position = new Vector3(0, 1, 30);
			ChosenCharacter.transform.position = new Vector3(-1f, ChosenCharacter.transform.position.y, 28f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
			DoorParticleSystem1.SetActive(false);
			DoorParticleSystem2.SetActive(false);
			DoorParticleSystem3.SetActive(false);
			StartCoroutine(OpenSelectedDoor());
			break;
		case GamePhase.ClipboardAppears:
			IntroUI.SetActive(false);
			gamePhase = GamePhase.ConcealedDoorsOpenAndClipboardRuns;
			ControlsEnabled = false;
			MinigameDoorsCollider.SetActive(false);
			Camera.main.transform.position = new Vector3(0, 1, 27);
			StartCoroutine(ConcealedDoorsOpen());
			break;
		case GamePhase.ConcealedDoorsOpenAndClipboardRuns:
			gamePhase = GamePhase.ClipboardHeadingToRiver;
			ChosenCharacter.transform.position = new Vector3(0, ChosenCharacter.transform.position.y, 29f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			StartCoroutine(ClipboardScript.GoToRiver());
			StartCoroutine(DisplayTraversalDialogue2());
			break;
		case GamePhase.ClipboardHeadingToRiver:
			gamePhase = GamePhase.WaitingForPlayerAtRiverBank;
			break;
		case GamePhase.WaitingForPlayerAtRiverBank:
			gamePhase = GamePhase.ClipboardSwimsAcrossRiver;
			Dialogue2UI.SetActive(false);
			ControlsEnabled = false;
			Camera.main.transform.parent = null;
			Camera.main.transform.position = new Vector3(20, 5, 31);
			Camera.main.transform.rotation = Quaternion.Euler(38, 90, 0);
			ChosenCharacter.transform.position = new Vector3(20, 1.388f, 29.8f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 90f, 0);
			StartCoroutine(ClipboardScript.SwimAcrossRiver());
			break;
		case GamePhase.ClipboardSwimsAcrossRiver:
			gamePhase = GamePhase.RiverMinigameSetup;
			Camera.main.transform.position = new Vector3(20.23f, 11.46f, 31);
			Camera.main.transform.rotation = Quaternion.Euler(56.05f, 90, 0);
			ChosenCharacter.transform.position = new Vector3(22, 1.388f, 29.8f);
			RockParticleSystem1.SetActive(true);
			RockParticleSystem2.SetActive(true);
			RockParticleSystem3.SetActive(true);
			RockParticleSystem4.SetActive(true);
			break;
		case GamePhase.RiverMinigameSetup:
			gamePhase = GamePhase.RiverCrossing;
			Camera.main.transform.position = new Vector3(20.627f, 2.809f, 29.875f);
			Camera.main.transform.rotation = Quaternion.Euler(26.05f, 90, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			ControlsEnabled = true;
			StartCoroutine(DisplayTraversalDialogue3());
			break;
		case GamePhase.RiverCrossing:
			gamePhase = GamePhase.ClipboardRunsToFireMinigame;
			StartCoroutine(ClipboardScript.RunToFireArea());
			break;
		case GamePhase.ClipboardRunsToFireMinigame:
			gamePhase = GamePhase.WaitingForPlayerAtFireArea;
			StartCoroutine(ClipboardScript.FloatIntoPosition());
			break;
		case GamePhase.WaitingForPlayerAtFireArea:
			gamePhase = GamePhase.DialogueInterruptedByFireWall;
			StartCoroutine(DisplayDialogueInterruptedByFireWall());
			Camera.main.transform.parent = null;
			Camera.main.transform.position = new Vector3(60, 1.19f, 31.19f);
			Camera.main.transform.rotation = Quaternion.Euler(0f, 90, 0);
			ChosenCharacter.transform.position = new Vector3(62f, -0.228f, 31.986f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 90, 0);
			ControlsEnabled = false;
			break;
		case GamePhase.DialogueInterruptedByFireWall:
			gamePhase = GamePhase.StairsAppear;
			Camera.main.transform.position = new Vector3(60.933f, 0.761f, 32.146f);
			Camera.main.transform.rotation = Quaternion.Euler(0f, 135, 0);


			break;
		case GamePhase.StairsAppear:
			break;
		}
		Debug.Log(gamePhase);
	}

	public void Dialogue2UIClose() {
		Dialogue2UI.SetActive(false);
	}

	private IEnumerator WaitForPlayerToWalk () {
		yield return new WaitForSeconds(10);
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
		Vector3 OriginalPosition = GOClipboard.transform.position;
		Vector3 EndPosition = new Vector3(0.292f, 0.856f, -1.55f);
		Quaternion OriginalRotation = GOClipboard.transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(85.466f, 287.293f, 86.645f);
		ClipboardRigidBody.useGravity = false;
		for (float f = 0f; f < 1f; f += 0.05f){
			GOClipboard.transform.position = Vector3.Lerp(OriginalPosition, EndPosition, f);
			GOClipboard.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		ClipboardRigidBody.velocity = Vector3.zero;
		ClipboardRigidBody.angularVelocity = Vector3.zero;
		GOClipboard.transform.position = EndPosition;
		GOClipboard.transform.rotation = EndRotation;
		GoToNextPhase();
	}

	private IEnumerator BackWallSlideUp () {
		for (int i = 0; i < 45; i++) {
			BackWall.transform.position = BackWall.transform.position + new Vector3(0, 0.05f, 0);
			yield return new WaitForSeconds(0.016f);
		}

		yield return new WaitForSeconds(2f);

		Dialogue2UI.SetActive(true);
		Dialogue2TextComponent.text = "So I'm going to call you Lab Rat, cool?";

	}

	private IEnumerator MinigameDoorSetup () {
		// Open & close MinigameDoor2 Using Lerp

		Quaternion OriginalRotation = MinigameDoor2.transform.rotation;
		Quaternion EndRotation = Quaternion.Euler(0, 120, 0);
		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoor2.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1.5f);

		Vector3 OriginalPosition = GOClipboard.transform.position;
		Vector3 EndPosition = new Vector3(0.054f, 0.336f, 32f);
		Quaternion OriginalClipboardRotation = GOClipboard.transform.rotation;
		Quaternion EndClipboardRotation = Quaternion.Euler(100f, 267f, 96f);
		ClipboardRigidBody.useGravity = false;
		for (float f = 0f; f < 1f; f += 0.05f){
			GOClipboard.transform.position = Vector3.Lerp(OriginalPosition, EndPosition, f);
			GOClipboard.transform.rotation = Quaternion.Lerp(OriginalClipboardRotation, EndClipboardRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		ClipboardRigidBody.velocity = Vector3.zero;
		ClipboardRigidBody.angularVelocity = Vector3.zero;
		GOClipboard.transform.position = EndPosition;
		GOClipboard.transform.rotation = EndClipboardRotation;

//		yield return new WaitForSeconds(1f);

		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoor2.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
			yield return new WaitForSeconds(0.016f);
		}
		MinigameDoor2.transform.rotation = OriginalRotation;
		GOClipboard.SetActive(false);
		yield return new WaitForSeconds(0.5f);

//		StartCoroutine(SpinDoors());
		GoToNextPhase();
	}

	private IEnumerator SpinDoors () {
		// Rotate three doors
		Vector3 Door1OriginalPosition = MinigameDoorParent1.transform.localPosition;
		Vector3 Door1EndPosition = new Vector3(0.866f, 0, -0.5f);
		Quaternion Door1OriginalRotation = MinigameDoorParent1.transform.localRotation;
		Quaternion Door1EndRotation = Quaternion.Euler(0, 120, 0);
		Vector3 Door2OriginalPosition = MinigameDoorParent2.transform.localPosition;
		Vector3 Door2EndPosition = new Vector3(0, 0, 1);
		Quaternion Door2OriginalRotation = MinigameDoorParent2.transform.localRotation;
		Quaternion Door2EndRotation = Quaternion.Euler(0, 0, 0);
		Vector3 Door3OriginalPosition = MinigameDoorParent3.transform.localPosition;
		Vector3 Door3EndPosition = new Vector3(-0.866f, 0, -0.5f);
		Quaternion Door3OriginalRotation = MinigameDoorParent3.transform.localRotation;
		Quaternion Door3EndRotation = Quaternion.Euler(0, -120, 0);
		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoorParent1.transform.localPosition = Vector3.Lerp(Door1OriginalPosition, Door1EndPosition, f);
			MinigameDoorParent1.transform.localRotation = Quaternion.Lerp(Door1OriginalRotation, Door1EndRotation, f);
			MinigameDoorParent2.transform.localPosition = Vector3.Lerp(Door2OriginalPosition, Door2EndPosition, f);
			MinigameDoorParent2.transform.localRotation = Quaternion.Lerp(Door2OriginalRotation, Door2EndRotation, f);
			MinigameDoorParent3.transform.localPosition = Vector3.Lerp(Door3OriginalPosition, Door3EndPosition, f);
			MinigameDoorParent3.transform.localRotation = Quaternion.Lerp(Door3OriginalRotation, Door3EndRotation, f);
			yield return new WaitForSeconds(0.016f);
		}

		float spin = 0.1f;
		for (int i = 1; i < 60; i++) {
			MinigameDoorsRigidBody.angularVelocity = new Vector3(0, spin, 0);
			spin += 0.1f;
			yield return new WaitForSeconds(0.016f);
		}

		MinigameDoorsRigidBody.angularDrag = 2f;

		yield return new WaitForSeconds(2f);

		Door1EndPosition = MinigameDoorParent1.transform.localPosition;
		Door2EndPosition = MinigameDoorParent2.transform.localPosition;
		Door3EndPosition = MinigameDoorParent3.transform.localPosition;
		Door1EndRotation = MinigameDoorParent1.transform.localRotation;
		Door2EndRotation = MinigameDoorParent2.transform.localRotation;
		Door3EndRotation = MinigameDoorParent3.transform.localRotation;

		for (float f = 0f; f < 1f; f += 0.05f){
			MinigameDoorParent1.transform.localPosition = Vector3.Lerp(Door1EndPosition, Door1OriginalPosition, f);
			MinigameDoorParent1.transform.localRotation = Quaternion.Lerp(Door1EndRotation, Door1OriginalRotation, f);
			MinigameDoorParent2.transform.localPosition = Vector3.Lerp(Door2EndPosition, Door2OriginalPosition, f);
			MinigameDoorParent2.transform.localRotation = Quaternion.Lerp(Door2EndRotation, Door2OriginalRotation, f);
			MinigameDoorParent3.transform.localPosition = Vector3.Lerp(Door3EndPosition, Door3OriginalPosition, f);
			MinigameDoorParent3.transform.localRotation = Quaternion.Lerp(Door3EndRotation, Door3OriginalRotation, f);
			yield return new WaitForSeconds(0.016f);
		}

		Quaternion MinigameDoorsOriginalRotation = MinigameDoors.transform.rotation;
		Quaternion MinigameDoorsEndRotation = Quaternion.Euler(0, 0, 0);
		float SpinParameter = 0.999f;
		while (SpinParameter > 0.001f) {
			SpinParameter *= SpinParameter;
			MinigameDoors.transform.rotation = Quaternion.Lerp(MinigameDoorsOriginalRotation, MinigameDoorsEndRotation, 1 - SpinParameter);
			yield return new WaitForSeconds(0.016f);
		}
		// Snap doors into position
		MinigameDoorParent1.transform.localPosition = Door1OriginalPosition;
		MinigameDoorParent2.transform.localPosition = Door2OriginalPosition;
		MinigameDoorParent3.transform.localPosition = Door3OriginalPosition;
		MinigameDoorParent1.transform.localRotation = Door1OriginalRotation;
		MinigameDoorParent2.transform.localRotation = Door2OriginalRotation;
		MinigameDoorParent3.transform.localRotation = Door3OriginalRotation;

		DoorParticleSystem1.SetActive(true);
		DoorParticleSystem2.SetActive(true);
		DoorParticleSystem3.SetActive(true);

		ControlsEnabled = true;

//		GoToNextPhase();
	}

	private IEnumerator OpenSelectedDoor () {
		if (ClipboardDoorNumber == 1) {
			GOClipboard.transform.position = new Vector3(-1, 0.336f, 32);
			GOClipboard.SetActive(true);
		}
		else if (ClipboardDoorNumber == 2) {
			GOClipboard.transform.position = new Vector3(0, 0.336f, 32);
			GOClipboard.SetActive(true);
		}
		else if (ClipboardDoorNumber == 3) {
			GOClipboard.transform.position = new Vector3(1.1f, 0.336f, 32);
			GOClipboard.SetActive(true);
		}

		if (Character.SelectedDoor == 1) {
			Quaternion OriginalRotation = MinigameDoor1.transform.rotation;
			Quaternion EndRotation = Quaternion.Euler(0, 120, 0);
			for (float f = 0f; f < 1f; f += 0.05f){
				MinigameDoor1.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
				yield return new WaitForSeconds(0.016f);
			}
			yield return new WaitForSeconds(1f);
			// Close the door only if the clipboard is not behind it
			if (Character.SelectedDoor != ClipboardDoorNumber) {
				for (float f = 0f; f < 1f; f += 0.05f){
					MinigameDoor1.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
					yield return new WaitForSeconds(0.016f);
				}
				MinigameDoor1.transform.rotation = OriginalRotation;
			}
		}

		else if (Character.SelectedDoor == 2) {
			Quaternion OriginalRotation = MinigameDoor2.transform.rotation;
			Quaternion EndRotation = Quaternion.Euler(0, 120, 0);
			for (float f = 0f; f < 1f; f += 0.05f){
				MinigameDoor2.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
				yield return new WaitForSeconds(0.016f);
			}
			yield return new WaitForSeconds(1f);

			if (Character.SelectedDoor != ClipboardDoorNumber) {
				for (float f = 0f; f < 1f; f += 0.05f){
					MinigameDoor2.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
					yield return new WaitForSeconds(0.016f);
				}
				MinigameDoor2.transform.rotation = OriginalRotation;
			}
		}

		else if (Character.SelectedDoor == 3) {
			Quaternion OriginalRotation = MinigameDoor3.transform.rotation;
			Quaternion EndRotation = Quaternion.Euler(0, 120, 0);
			for (float f = 0f; f < 1f; f += 0.05f){
				MinigameDoor3.transform.rotation = Quaternion.Lerp(OriginalRotation, EndRotation, f);
				yield return new WaitForSeconds(0.016f);
			}
			yield return new WaitForSeconds(1f);

			if (Character.SelectedDoor != ClipboardDoorNumber) {
				for (float f = 0f; f < 1f; f += 0.05f){
					MinigameDoor3.transform.rotation = Quaternion.Lerp(EndRotation, OriginalRotation, f);
					yield return new WaitForSeconds(0.016f);
				}
				MinigameDoor3.transform.rotation = OriginalRotation;
			}
		}

		if (Character.SelectedDoor == ClipboardDoorNumber) {
			yield return new WaitForSeconds(2f);
			GoToNextPhase();
		}
		else {
			Character.DoorTriggered = false;
			gamePhase = GamePhase.ThreeDoorShuffle;
			GOClipboard.SetActive(false);
			Camera.main.transform.position = new Vector3(0, 1, 27);
			Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
			GoToNextPhase();
		}
	}

	private IEnumerator ConcealedDoorsOpen () {
		for (int i = 0; i < 30; i++) {
			DoorMinigameConcealedWallLeft.transform.position = DoorMinigameConcealedWallLeft.transform.position + new Vector3(0, 0, 0.05f);
			DoorMinigameConcealedWallRight.transform.position = DoorMinigameConcealedWallRight.transform.position + new Vector3(0, 0, -0.05f);
			yield return new WaitForSeconds(0.016f);
		}

		yield return new WaitForSeconds(2f);
		GoToNextPhase();
	}

	private IEnumerator DisplayTraversalDialogue2 () {
		yield return new WaitForSeconds(5f);
		Dialogue2UI.SetActive(true);
		Dialogue2TextComponent.text = "So, are you being reimbursed for this?";
		Dialogue2Button1TextComponent.text = "I think so...";
		Dialogue2Button2TextComponent.text = "I don't know...";
	}

	private IEnumerator DisplayTraversalDialogue3 () {
		yield return new WaitForSeconds(10f);
		Dialogue2UI.SetActive(true);
		Dialogue2TextComponent.text = "Hey Labrat, you think I should ask for more outta the reimbursement?";
		Dialogue2Button1TextComponent.text = "Definitely Guinea Pig!";
		Dialogue2Button2TextComponent.text = "But I'm not getting a free lunch...";
	}

	private IEnumerator DisplayDialogueInterruptedByFireWall () {
		yield return new WaitForSeconds(2f);
		Dialogue2UI.SetActive(true);
		Dialogue2TextComponent.text = "Huh, I wonder why the heck it sto-";
		Dialogue2Button1TextComponent.text = "??";
		Dialogue2Button2TextComponent.text = "!!";
		yield return new WaitForSeconds(2f);
		Dialogue2UI.SetActive(false);
		FireWallParticleSystem.SetActive(true);
		yield return new WaitForSeconds(2f);
	}
}
