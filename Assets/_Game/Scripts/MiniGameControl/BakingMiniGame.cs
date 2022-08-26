using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGame : MiniGame
{
    public EggBreakingPhase eggBreakingPhase = new EggBreakingPhase();
    public FlourPourPhase flourPourPhase = new FlourPourPhase();
    public SugarPourPhase sugarPourPhase=new SugarPourPhase();
    public GameObject[] placeForContestants = null;
    public GameObject placeForPlayer;

    public Spill sugarSpill = null;
    public GameObject sugarPile = null;
    public GameObject sugarBox=null;

    public BakingMiniGameCanvas bakingMiniGameCanvas = null;

    public bool isMiniGameStarted=false;

    private BakingMiniGameState currentState = null;
    private GameController gameController = null;
    private void Awake()
    {
        models.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    [ContextMenu("InitializeMiniGame")]
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);
    }

    private void OnEnable()
    {
        currentState = eggBreakingPhase;
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

    private void Update()
    {
        if(!isMiniGameStarted)return;
        currentState = currentState.DoState(this);
        /*         if(sugarSpill.isSpilling){
                    sugarPile.transform.localScale=new Vector3(sugarPile.transform.localScale.x>1?1:(sugarPile.transform.localScale.x+Time.deltaTime/4),sugarPile.transform.localScale.y>1?1:(sugarPile.transform.localScale.y+Time.deltaTime/4),sugarPile.transform.localScale.z>1?1:(sugarPile.transform.localScale.z+Time.deltaTime/4));
                    sugarProgressBar.SetFill(sugarPile.transform.localScale.x);
                } */
    }
}
