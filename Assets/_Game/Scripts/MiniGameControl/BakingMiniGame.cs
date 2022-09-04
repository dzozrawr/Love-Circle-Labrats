using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BakingMiniGame : MiniGame
{
    static private BakingMiniGame instance = null;
    public EggBreakingPhase eggBreakingPhase = new EggBreakingPhase();
    public FlourPourPhase flourPourPhase = new FlourPourPhase();
    public SugarPourPhase sugarPourPhase = new SugarPourPhase();
    public EggFlourSugarPhase eggFlourSugarPhase = new EggFlourSugarPhase();
    public BakingMixingPhase mixingPhase = new BakingMixingPhase();
    public BowlSwitchPhase bowlSwitchPhase = new BowlSwitchPhase();
    public FruitPuttingPhase fruitPhase = new FruitPuttingPhase();
    public TopLayerPutPhase topLayerPutPhase = new TopLayerPutPhase();
    public PieCuttingPhase pieCuttingPhase = new PieCuttingPhase();
    public BakingPhase bakingPhase = new BakingPhase();

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

    public GameObject dough = null;
    public GameObject woodenSpoon = null;

    public Vector3 doughStartScale, doughEndScale;

    public GameObject hitCircleForFruit = null;

    public GameObject pieDishPrefab = null;

    public Transform bowlMovedPlace = null;

    public GameObject mixingBowl = null;
    public CinemachineVirtualCamera topDownBakingCamera = null;
    public PieDish pieDishBadPrefab = null;
    public GameObject[] contestantsMixingBowls;

    public CinemachineVirtualCamera contestantsPiesCamera = null;
    public bool isMiniGameStarted = false;

    private BakingMiniGameState currentState = null, prevState = null;
    private GameController gameController = null;

    private Vector3 sugarPileInitPos;

    private List<GameObject> eggYolks = new List<GameObject>();

    private PieDish pieDish = null;

    private Stack<Vector3> pileScales = new Stack<Vector3>();

    private GameObject topPile = null;
    private GameObject bottomPile = null;
    private bool isMiniGameDone = false;

    private FinalEliminationManager finalEliminationManager;

    public Vector3 SugarPileInitPos { get => sugarPileInitPos; set => sugarPileInitPos = value; }
    public List<GameObject> EggYolks { get => eggYolks; set => eggYolks = value; }
    public static BakingMiniGame Instance { get => instance; }
    public PieDish PieDish { get => pieDish; set => pieDish = value; }
    public Stack<Vector3> PileScales { get => pileScales; }
    public GameObject TopPile { get => topPile; set => topPile = value; }
    public GameObject BottomPile { get => bottomPile; set => bottomPile = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        models.SetActive(false);
        canvas.gameObject.SetActive(false);

        pileScales.Push(sugarPileEndScale);
        pileScales.Push(sugarPileStartScale);
        pileScales.Push(flourPileEndScale);
        pileScales.Push(flourPileStartScale);

    }

    [ContextMenu("InitializeMiniGame")]
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        sugarPileInitPos = sugarPile.transform.position;
        finalEliminationManager=FinalEliminationManager.Instance;

        FinalEliminationManager.Instance.SetSelectedMiniGame(this);
    }

    private void OnEnable()
    {
        //currentState = eggBreakingPhase;
        currentState = eggFlourSugarPhase;

    }
    protected override void OnEliminateButtonPressed()
    {
        ContestantScript contestant;
        Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position

        
        ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

        for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
        {
            contestant=Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
            contestant.MatchSuccessPoints=contestantQuestioningManager.WinningContestants[i].MatchSuccessPoints;
            finalEliminationManager.contestants.Add(contestant);
        }
    }

    private void Update()
    {
        if (!isMiniGameStarted || isMiniGameDone) return;



        if (prevState != currentState)
        {
            currentState.InitState(this);
        }

        prevState = currentState;
        currentState = currentState.DoState(this);

        if (currentState == null)
        {
            for (int i = 0; i < contestantsMixingBowls.Length; i++)
            {
                if (i == 0)
                {
                    Instantiate(pieDishBadPrefab, contestantsMixingBowls[i].transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(pieDish, contestantsMixingBowls[i].transform.position, Quaternion.identity);
                    finalEliminationManager.contestants[i].MatchSuccessPoints++;
                }
                contestantsMixingBowls[i].SetActive(false);
            }
            finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().SetTrigger("Cry");
            finalEliminationManager.contestants[1].GetComponentInChildren<Animator>().SetTrigger("Happy");
            CameraController.Instance.transitionToCMVirtualCamera(contestantsPiesCamera);
            isMiniGameDone = true;
        }


        /*         if(sugarSpill.isSpilling){
                    sugarPile.transform.localScale=new Vector3(sugarPile.transform.localScale.x>1?1:(sugarPile.transform.localScale.x+Time.deltaTime/4),sugarPile.transform.localScale.y>1?1:(sugarPile.transform.localScale.y+Time.deltaTime/4),sugarPile.transform.localScale.z>1?1:(sugarPile.transform.localScale.z+Time.deltaTime/4));
                    sugarProgressBar.SetFill(sugarPile.transform.localScale.x);
                } */
    }
}
