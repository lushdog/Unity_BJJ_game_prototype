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

        DummyBehaviour player1script = _player1.GetComponent<DummyBehaviour>();
        DummyBehaviour player2script = _player2.GetComponent<DummyBehaviour>();
        player1script.SetOpponent(_player2);
        player1script.PlayerNumber = 1;
        player2script.PlayerNumber = 2;
        player2script.SetOpponent(_player1);

        player1script.SetState(player1script.States.Find(x => x is PullingGuard));
        player2script.SetState(player2script.States.Find(x => x is EnteringGuard));

	
	}
	
	void Update () {
	
	}
}
