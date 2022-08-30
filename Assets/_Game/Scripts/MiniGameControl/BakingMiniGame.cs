using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGame : MiniGame
{
    static private BakingMiniGame instance=null;
    public EggBreakingPhase eggBreakingPhase = new EggBreakingPhase();
    public FlourPourPhase flourPourPhase = new FlourPourPhase();
    public SugarPourPhase sugarPourPhase = new SugarPourPhase();
    public BakingMixingPhase mixingPhase=new BakingMixingPhase();
    public BowlSwitchPhase bowlSwitchPhase=new BowlSwitchPhase();
    public FruitPuttingPhase fruitPhase=new FruitPuttingPhase();

    public GameObject[] placeForContestants = null;
    public GameObject placeForPlayer;




    public BakingMiniGameCanvas bakingMiniGameCanvas = null;

    public BrokenEgg[] brokenEggs;

    public GameObject flourPile = null;
    public Vector3 flourPileStartScale, flourPileEndScale;
    public GameObject flourBag = null;
    public Spill flourSpill = null;

    public GameObject sugarPile = null;

    public Vector3 sugarPileStartScale, sugarPileEndScale;
    public GameObject sugarBox = null;
    public Spill sugarSpill = null;

    public GameObject dough=null;

    public Vector3 doughStartScale, doughEndScale;

    public GameObject hitCircleForFruit=null;

    public GameObject pieDishPrefab=null;

    public Transform bowlMovedPlace=null;

    public GameObject mixingBowl=null;
    public bool isMiniGameStarted = false;

    private BakingMiniGameState currentState = null, prevState = null;
    private GameController gameController = null;

    private Vector3 sugarPileInitPos;

    private List<GameObject> eggYolks=new List<GameObject>();

    public Vector3 SugarPileInitPos { get => sugarPileInitPos; set => sugarPileInitPos = value; }
    public List<GameObject> EggYolks { get => eggYolks; set => eggYolks = value; }
    public static BakingMiniGame Instance { get => instance; }

    private void Awake()
    {
        if(instance!=null){
            Destroy(gameObject);
            return;
        }
        instance=this;

        models.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    [ContextMenu("InitializeMiniGame")]
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        sugarPileInitPos= sugarPile.transform.position;
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
        if (!isMiniGameStarted) return;

        if (prevState != currentState)
        {
            currentState.InitState(this);
        }

        prevState = currentState;
        currentState = currentState.DoState(this);


        /*         if(sugarSpill.isSpilling){
                    sugarPile.transform.localScale=new Vector3(sugarPile.transform.localScale.x>1?1:(sugarPile.transform.localScale.x+Time.deltaTime/4),sugarPile.transform.localScale.y>1?1:(sugarPile.transform.localScale.y+Time.deltaTime/4),sugarPile.transform.localScale.z>1?1:(sugarPile.transform.localScale.z+Time.deltaTime/4));
                    sugarProgressBar.SetFill(sugarPile.transform.localScale.x);
                } */
    }
}
