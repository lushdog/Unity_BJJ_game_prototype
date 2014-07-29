using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject PlayerObject;

	private GameObject _player1;
	private GameObject _player2;

	void Start () {
	
		_player1 = PlayerObject;
		Transform player2Start = GameObject.Find("Player2 Start").transform;
		_player2 = (GameObject)Instantiate(PlayerObject, player2Start.transform.position, player2Start.transform.rotation);
		Renderer renderer = (Renderer)_player2.GetComponentsInChildren<Renderer>()[0];
		renderer.material.color = new Color(0, 1, 1, 1);

		_player1.GetComponent<DummyBehaviour>().SetState("PullGuard");
		_player2.GetComponent<DummyBehaviour>().SetState("EnterGuard");
		_player1.GetComponent<DummyBehaviour>().SetOpponent(_player2);
        _player1.GetComponent<DummyBehaviour>().PlayerNumber = 1;
        _player2.GetComponent<DummyBehaviour>().PlayerNumber = 2;
		_player2.GetComponent<DummyBehaviour>().SetOpponent(_player1);
	
	}
	
	void Update () {
	
	}
}
