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
	public GameObject StairParticleSystem;
	public GameObject FireWallParticleSystem;
	public GameObject FireAreaBackWall;
	public GameObject FireAreaTransitionWall;
	private Rigidbody ClipboardRigidBody;
	private Rigidbody MinigameDoorsRigidBody;
	public GameObject IntroUI;
	public GameObject TutorialUI;
	public GameObject DialogueUI;
	public GameObject TraversalDialogueUI;
	public GameObject TutorialText;
	public GameObject DialogueText;
	public GameObject TraversalDialogueText;
	public GameObject TutorialButtonText;
	public GameObject DialogueButton1Text, DialogueButton2Text;
	public GameObject TraversalDialogueButton1Text, TraversalDialogueButton2Text;
	public GameObject DialogueButton1, DialogueButton2;
	private Text TutorialTextComponent;
	private Text DialogueTextComponent;
	private Text TraversalDialogueTextComponent;
	private Text TutorialButtonTextComponent;
	private Text DialogueButton1TextComponent, DialogueButton2TextComponent;
	private Text TraversalDialogueButton1TextComponent, TraversalDialogueButton2TextComponent;
	public enum GamePhase {StartMenuShowing, WalkingIntoRoom, TurnLeft, Tutorial1, Tutorial1Walking, Dialogue1, Dialogue2, Dialogue3, Dialogue4, ClipboardFalling, Dialogue5, HallwayChase, WaitingForPlayer, ClipboardHopToDoor, ThreeDoorShuffle, PlayerChoosesDoor, ClipboardAppears, ConcealedDoorsOpenAndClipboardRuns, ClipboardHeadingToRiver, WaitingForPlayerAtRiverBank, ClipboardSwimsAcrossRiver, RiverMinigameSetup, RiverCrossing, ClipboardRunsToFireMinigame, WaitingForPlayerAtFireArea, DialogueInterruptedByFireWall, StairsAppear, StairDialogue1, StairDialogue2, StairDialogue3, CharacterOnStairs, WaitForPlayerToPushSpace, CharacterFallsIntoFire, CharacterExitsFire, CharacterScoldsPlayer, FireDialogue1, FireDialogue2, FireDialogue3, FireDialogue4, FireDialogue5, FireDialogue6, TransitionToNextHallway, TilesFallOutOfCeiling, MemoryMatchMinigame};
	public static GamePhase gamePhase = GamePhase.StartMenuShowing;
	public static bool ControlsEnabled = false;
	private static int ClipboardDoorNumber = 0;


	// Use this for initialization
	void Start () {
		mainSceneController = this;
		TutorialTextComponent = TutorialText.GetComponent<Text>();
		DialogueTextComponent = DialogueText.GetComponent<Text>();
		TraversalDialogueTextComponent = TraversalDialogueText.GetComponent<Text>();
		TutorialButtonTextComponent = TutorialButtonText.GetComponent<Text>();
		DialogueButton1TextComponent = DialogueButton1Text.GetComponent<Text>();
		DialogueButton2TextComponent = DialogueButton2Text.GetComponent<Text>();
		TraversalDialogueButton1TextComponent = TraversalDialogueButton1Text.GetComponent<Text>();
		TraversalDialogueButton2TextComponent = TraversalDialogueButton2Text.GetComponent<Text>();
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
//		gamePhase = GamePhase.Tutorial1;
//		gamePhase = GamePhase.WaitingForPlayer;
//		gamePhase = GamePhase.ClipboardAppears;
//		gamePhase = GamePhase.WaitingForPlayerAtFireArea;
		gamePhase = GamePhase.TransitionToNextHallway;
		GoToNextPhase();
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
			TutorialUI.SetActive(true);
			TutorialTextComponent.text = "Tutorial: Use WASD to move Virtual Avatar. Hold shift to run.";
			TutorialButtonTextComponent.text = "OK";
			break;
		case GamePhase.Tutorial1:
			gamePhase = GamePhase.Tutorial1Walking;
			IntroUI.SetActive(false);
			TutorialUI.SetActive(false);
			ControlsEnabled = true;
			StartCoroutine(WaitForPlayerToWalk());
			break;
		case GamePhase.Tutorial1Walking:
			gamePhase = GamePhase.Dialogue1;
			ControlsEnabled = false;
			TutorialUI.SetActive(true);
			// Warp the character to center
			ChosenCharacter.transform.position = new Vector3(0,ChosenCharacter.transform.position.y, 0);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Huh, didn't know I'd be working with a partner. You here for the video game experiment too?";
			DialogueButton1TextComponent.text = "Yes";
			DialogueButton2TextComponent.text = "No";
			break;
		case GamePhase.Dialogue1:
			gamePhase = GamePhase.Dialogue2;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Will that always happen when I ask you a question?";
			DialogueButton1TextComponent.text = "Yes";
			DialogueButton2TextComponent.text = "IDK, maybe?";
			break;
		case GamePhase.Dialogue2:
			gamePhase = GamePhase.Dialogue3;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Alright, good to know.";
			DialogueButton1TextComponent.text = "OK";
			DialogueButton2TextComponent.text = "Cool";
			break;
		case GamePhase.Dialogue3:
			gamePhase = GamePhase.Dialogue4;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "What do you think they want us to do?";
			DialogueButton1TextComponent.text = "Not Sure";
			DialogueButton2TextComponent.text = "You tell me!";
			ChosenCharacter.transform.position = new Vector3(0,ChosenCharacter.transform.position.y, 0);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
			break;
		case GamePhase.Dialogue4:
			gamePhase = GamePhase.ClipboardFalling;
			DialogueUI.SetActive(false);
			TutorialUI.SetActive(false);
			StartCoroutine(ClipboardFall());
			break;
		case GamePhase.ClipboardFalling:
			gamePhase = GamePhase.Dialogue5;
			TutorialUI.SetActive(true);
			TutorialTextComponent.text = "Clipboard: You can't leave 'til you catch me, guinea pig.";
			TutorialButtonTextComponent.text = "OK";
			break;
		case GamePhase.Dialogue5:
			gamePhase = GamePhase.HallwayChase;
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			ControlsEnabled = true;
			TutorialUI.SetActive(false);
			StartCoroutine(BackWallSlideUp());
			break;
		case GamePhase.HallwayChase:
			gamePhase = GamePhase.WaitingForPlayer;
			break;
		case GamePhase.WaitingForPlayer:
			gamePhase = GamePhase.ClipboardHopToDoor;
			IntroUI.SetActive(false);
			DialogueUI.SetActive(false);
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
			DialogueUI.SetActive(false);
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
			IntroUI.SetActive(false);
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
			Camera.main.transform.rotation = Quaternion.Euler(0f, 122.355f, 0);
			StairParticleSystem.SetActive(true);
			break;
		case GamePhase.StairsAppear:
			gamePhase = GamePhase.StairDialogue1;
			Camera.main.transform.position = new Vector3(60, 1.19f, 31.19f);
			Camera.main.transform.rotation = Quaternion.Euler(0f, 90, 0);
			TutorialUI.SetActive(true);
			TutorialTextComponent.text = "When on Stairs, press SPACE to jump";
			TutorialButtonTextComponent.text = "OK";
			break;
		case GamePhase.StairDialogue1:
			gamePhase = GamePhase.StairDialogue2;
			TutorialUI.SetActive(false);
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Don't even think about it, Labrat...";
			DialogueButton1TextComponent.text = "OK, but hear me out...";
			DialogueButton2TextComponent.text = "I didn't say anything!";
			break;
		case GamePhase.StairDialogue2:
			gamePhase = GamePhase.StairDialogue3;
			StartCoroutine(DialogueInterruptedByCharacter());
			break;
		case GamePhase.StairDialogue3:
			gamePhase = GamePhase.CharacterOnStairs;
			Camera.main.transform.position = new Vector3(63.75f, 2.12f, 31.258f);
			Camera.main.transform.rotation = Quaternion.Euler(30.379f, 90, 0);
			ChosenCharacter.transform.position = new Vector3(64.918f, 0.798f, 31.258f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 90, 0);
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "...ugh, fine! Your the boss. Just make sure I get across. No funny business!";
			DialogueButton1TextComponent.text = "OK";
			DialogueButton2TextComponent.text = "You betcha!";
			break;
		case GamePhase.CharacterOnStairs:
			DialogueUI.SetActive(false);
			gamePhase = GamePhase.WaitForPlayerToPushSpace;
			// Waiting for player to push space
			FireAreaBackWall.SetActive(true);
			break;
		case GamePhase.WaitForPlayerToPushSpace:
			gamePhase = GamePhase.CharacterFallsIntoFire;
			break;
		case GamePhase.CharacterFallsIntoFire:
			gamePhase = GamePhase.CharacterExitsFire;
			Camera.main.transform.position = new Vector3(70.75f, 2.12f, 31.25f);
			Camera.main.transform.rotation = Quaternion.Euler(30.379f, 270, 0);
			ChosenCharacter.transform.position = new Vector3(65.48f, -0.122f, 31.25f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 90, 0);
			break;
		case GamePhase.CharacterExitsFire:
			gamePhase = GamePhase.CharacterScoldsPlayer;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "WHAT ON EARTH! I SAID NO FUNNY BUSINESS!";
			DialogueButton1TextComponent.text = "I pressed space I swear!";
			DialogueButton2TextComponent.text = "Well..uh..technically I did get you across..";
			break;
		case GamePhase.CharacterScoldsPlayer:
			gamePhase = GamePhase.FireDialogue1;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "...";
			DialogueButton1TextComponent.text = "[Ask Guinea Pig if they are okay]";
			DialogueButton2TextComponent.text = "[They look okay]";
			// Need to figure out how to tell which button was clicked to do branching dialogue here
			break;
		case GamePhase.FireDialogue1:
			gamePhase = GamePhase.FireDialogue2;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "Thanks for clicking that, I'm not doing so good.";
			DialogueButton1TextComponent.text = "Aw";
			DialogueButton2TextComponent.text = "Great!";
			break;
		case GamePhase.FireDialogue2:
			gamePhase = GamePhase.FireDialogue3;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "The Clipboard can wait, let's find some medical supplies.";
			DialogueButton1TextComponent.text = "Sounds like a plan.";
			DialogueButton2TextComponent.text = "Roger that.";
			break;
		case GamePhase.FireDialogue3:
			gamePhase = GamePhase.FireDialogue4;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "...";
			DialogueButton1TextComponent.text = "Hey, want to hear a joke Guinea Pig?";
			DialogueButton2TextComponent.text = "-";
			break;
		case GamePhase.FireDialogue4:
			gamePhase = GamePhase.FireDialogue5;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "...";
			DialogueButton1TextComponent.text = "This was a triumph. I'm making a note here - 'Huge Success'.";
			DialogueButton2TextComponent.text = "How did Abe Lincoln win his court case? Because he's in-a-cent.";
			break;
		case GamePhase.FireDialogue5:
			gamePhase = GamePhase.FireDialogue6;
			DialogueUI.SetActive(true);
			DialogueTextComponent.text = "I'm in even more pain now.";
			DialogueButton1TextComponent.text = "...";
			DialogueButton2TextComponent.text = "...";
			break;
		case GamePhase.FireDialogue6:
			gamePhase = GamePhase.TransitionToNextHallway;
			DialogueUI.SetActive(false);
			FireWallParticleSystem.SetActive(false);
			StartCoroutine(FireAreaTransitionWallOpens());
			ChosenCharacter.transform.position = new Vector3(69.09f, -0.122f, 31.25f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 90, 0);
			Camera.main.transform.position = new Vector3(67.75f, 1.12f, 31.25f);
			Camera.main.transform.rotation = Quaternion.Euler(11.261f, 90, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			ControlsEnabled = true;
			break;
		case GamePhase.TransitionToNextHallway:
			IntroUI.SetActive(false);
			gamePhase = GamePhase.TilesFallOutOfCeiling;
			// Wait for Player to enter medical room
			ChosenCharacter.transform.position = new Vector3(91.74f, -0.23f, 31.92f);
			ChosenCharacter.transform.rotation = Quaternion.Euler(0, 103.7f, 0);
			Camera.main.transform.position = new Vector3(90.34f, 1.24f, 31.157f);
			Camera.main.transform.rotation = Quaternion.Euler(15.328f, 90, 0);
			Camera.main.transform.parent = ChosenCharacter.transform;
			ControlsEnabled = false;
			break;
		case GamePhase.TilesFallOutOfCeiling:
			gamePhase = GamePhase.MemoryMatchMinigame;
			ControlsEnabled = false;
			Camera.main.transform.parent = null;
			Camera.main.transform.position = new Vector3(95.91f, 4.57f, 31.157f);
			Camera.main.transform.rotation = Quaternion.Euler(90, 90, 0);
			break;
		case GamePhase.MemoryMatchMinigame:
			
			break;
		}
		Debug.Log(gamePhase);
	}

	public void TraversalDialogueUIClose() {
		TraversalDialogueUI.SetActive(false);
	}

	private IEnumerator WaitForPlayerToWalk () {
//		yield return new WaitForSeconds(1);
		yield return new WaitForSeconds(10);
		GoToNextPhase();
		// TESTING TESTING TESTING
//		ChosenCharacter.transform.position = new Vector3(-10, 0, 0);
//		Camera.main.transform.position = new Vector3(-10, 1, -2);
//		yield return new WaitForSeconds(1);
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

		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "So I'm going to call you Lab Rat, cool?";

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
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "So, are you being reimbursed for this?";
		TraversalDialogueButton1TextComponent.text = "I think so...";
		TraversalDialogueButton2TextComponent.text = "I don't know...";
	}

	private IEnumerator DisplayTraversalDialogue3 () {
		yield return new WaitForSeconds(10f);
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "Hey Labrat, you think I should ask for more outta the reimbursement?";
		TraversalDialogueButton1TextComponent.text = "Definitely Guinea Pig!";
		TraversalDialogueButton2TextComponent.text = "But I'm not getting a free lunch...";
	}

	private IEnumerator DisplayDialogueInterruptedByFireWall () {
		yield return new WaitForSeconds(2f);
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "Huh, I wonder why the heck it sto-";
		TraversalDialogueButton1TextComponent.text = "??";
		TraversalDialogueButton2TextComponent.text = "!!";
		yield return new WaitForSeconds(2f);
		TraversalDialogueUI.SetActive(false);
		FireWallParticleSystem.SetActive(true);
		yield return new WaitForSeconds(2f);
		GoToNextPhase();
	}

	private IEnumerator DialogueInterruptedByCharacter () {
		DialogueUI.SetActive(false);
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "...";
		TraversalDialogueButton1TextComponent.text = "How about--";
		TraversalDialogueButton2TextComponent.text = "Maybe if--";
		yield return new WaitForSeconds(3f);
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "Nope.";
		TraversalDialogueButton1TextComponent.text = "But we have to--";
		TraversalDialogueButton2TextComponent.text = "Let's talk about--";
		yield return new WaitForSeconds(3f);
		TraversalDialogueUI.SetActive(true);
		TraversalDialogueTextComponent.text = "I said NO.";
		TraversalDialogueButton1TextComponent.text = "...";
		TraversalDialogueButton2TextComponent.text = "...";
		yield return new WaitForSeconds(3f);
		GoToNextPhase();
	}

	private IEnumerator FireAreaTransitionWallOpens () {
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < 90; i++) {
			FireAreaTransitionWall.transform.position = FireAreaTransitionWall.transform.position + new Vector3(0, 0.05f, 0);
			yield return new WaitForSeconds(0.016f);
		}
	}
}
