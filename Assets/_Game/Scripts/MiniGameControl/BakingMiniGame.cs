using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGame : MiniGame
{
    public GameObject[] placeForContestants = null;
    public GameObject placeForPlayer;

    private GameController gameController = null;
    private void Awake()
    {
        models.SetActive(false);
    }

    private void Start()
    {
        GameController.Instance.ContestantsEliminated.AddListener(OnEliminateButtonPressed);
    }
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        gameController = GameController.Instance;
    }

    protected override void OnEliminateButtonPressed()
    {
        Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position
        ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

        for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
        {
            Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
        }
    }
}
