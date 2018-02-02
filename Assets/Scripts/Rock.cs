using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

	public GameObject Plank;
	public Vector3 PlankEndPosition;
	private Renderer thisRenderer;
	public static bool Plank1Placed, Plank2Placed, Plank3Placed, Plank4Placed, AllPlanksPlaced;

	// Use this for initialization
	void Start () {
		thisRenderer = GetComponent<Renderer>();
		Plank1Placed = false;
		Plank2Placed = false;
		Plank3Placed = false;
		Plank4Placed = false;
		AllPlanksPlaced = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown()
	{
		Debug.Log(Plank.name);
		if (Plank.name == "Plank1" && Plank1Placed == false) {
			StartCoroutine(MovePlank());
			Plank1Placed = true;
		}
		else if (Plank.name == "Plank2" && Plank2Placed == false) {
			StartCoroutine(MovePlank());
			Plank2Placed = true;
		}
		else if (Plank.name == "Plank3" && Plank3Placed == false) {
			StartCoroutine(MovePlank());
			Plank3Placed = true;
		}
		else if (Plank.name == "Plank4" && Plank4Placed == false) {
			StartCoroutine(MovePlank());
			Plank4Placed = true;
		}

		if (Plank1Placed == true && Plank2Placed == true && Plank3Placed == true && Plank4Placed == true) {
			AllPlanksPlaced = true;
			MainSceneController.mainSceneController.GoToNextPhase();
		}

	}

	private IEnumerator MovePlank() {
		Vector3 PlankStartPosition = Plank.transform.position;
		for (float f = 0f; f < 1f; f += 0.05f){
			Plank.transform.position = Vector3.Lerp(PlankStartPosition, PlankEndPosition, f);
			yield return new WaitForSeconds(0.016f);
		}
		yield return new WaitForSeconds(1f);
	}
}
