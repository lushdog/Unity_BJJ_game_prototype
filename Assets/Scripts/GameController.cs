using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public BjjPlayer PlayerObject;

	private BjjPlayer _player1;
	private BjjPlayer _player2;

	void Start () {
	
		_player1 = PlayerObject;
		Transform player2Start = GameObject.Find("Player2 Start").transform;
		_player2 = (BjjPlayer)Instantiate(PlayerObject, player2Start.transform.position, player2Start.transform.rotation);
		Renderer renderer = (Renderer)_player2.GetComponentsInChildren<Renderer>()[0];
		renderer.material.color = new Color(0, 1, 1, 1);

        BjjPlayer player1script = (BjjPlayer)_player1.GetComponent<BjjPlayer>();
        BjjPlayer player2script = (BjjPlayer)_player2.GetComponent<BjjPlayer>();
        player1script.Opponent = _player2;
        player1script.PlayerNumber = 1;
        player2script.PlayerNumber = 2;
        player2script.Opponent = _player1;

        player1script.SetState(BjjState.PullingGuard);
        player2script.SetState(BjjState.EnteringGuard);
	}
	
	void Update () 
    {
	
	}
}
