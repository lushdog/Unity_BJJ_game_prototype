using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public BjjPlayer Player1;
	public BjjPlayer Player2;

	void Start () 
    {

        BjjPlayer player1script = (BjjPlayer)Player1.GetComponent<BjjPlayer>();
        BjjPlayer player2script = (BjjPlayer)Player2.GetComponent<BjjPlayer>();

        player1script.Opponent = Player2;
        player1script.PlayerNumber = 1;
        player2script.PlayerNumber = 2;
        player2script.Opponent = Player1;

        player1script.SetState(BjjState.PullingGuard);
        player2script.SetState(BjjState.EnteringGuard);
	}
	
	void Update () 
    {
	
	}
}
