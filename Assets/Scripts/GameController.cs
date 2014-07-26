using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject PlayerObject;

	private GameObject Player1;
	private GameObject Player2;

	void Start () {
	
		Player1 = PlayerObject;
		Transform player2Start = GameObject.Find("Player2 Start").transform;
		Player2 = (GameObject)Instantiate(PlayerObject, player2Start.transform.position, player2Start.transform.rotation);
		Renderer renderer = (Renderer)Player2.GetComponentsInChildren<Renderer>()[0];
		renderer.material.color = new Color(0, 1, 1, 1);

		Player1.GetComponent<DummyBehaviour>().SetState("PullGuard");
		Player2.GetComponent<DummyBehaviour>().SetState("EnterGuard");
		Player1.GetComponent<DummyBehaviour>().SetOpponent(Player2);
        Player1.GetComponent<DummyBehaviour>().PlayerNumber = 1;
        Player2.GetComponent<DummyBehaviour>().PlayerNumber = 2;
		Player2.GetComponent<DummyBehaviour>().SetOpponent(Player1);
	
	}
	
	void Update () {
	
	}
}
