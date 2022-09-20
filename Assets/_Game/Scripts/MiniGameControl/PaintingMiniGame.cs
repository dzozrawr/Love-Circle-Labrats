using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using DG.Tweening;
using Contestant;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class PaintingMiniGame : MiniGame
{
    private static PaintingMiniGame instance = null;
    public P3dPaintSphere paintSphere = null;
    public P3dPaintableTexture canvasPaintableTexComponent = null;


    public float canvasBackgroundFilledRatio = 0.56256f;

    public Transform startingPlaceForStencil = null;
    public Transform endPlaceForStencil = null;
    public P3dColorCounter colorCounterComponent = null;

    public GameObject canvasGameObject = null;

    public CinemachineVirtualCamera contestantsResultsCam=null;

    public PantingCanvasScript leftContestantFailCanvas=null;

    public PantingCanvasScript rightContestantWinCanvas=null;

    public Material canvasFailMat=null;


    private PaintingMiniGameCanvas paintingMiniGameCanvas = null;

    private P3dChangeCounter canvasChangeCounterComponent = null;

    private GameObject curStencil = null;
    private Camera mainCamera = null;

    private Coroutine waitForCanvasPaintCoroutine = null;

    private bool isFirstPhaseActive = false;
    private bool isStencilPhaseActive = false;

    private float ratioForCompletenessOfStencil = -1f;

    private P3dColor paint3DColor = null;

    private DialogueSystemTrigger dialogueSystemTrigger=null;

    private DialogueSystemEvents dialogueSystemEvents=null;

    public static PaintingMiniGame Instance { get => instance; }

    // private GameController gameController=null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        paintingMiniGameCanvas = canvas.GetComponent<PaintingMiniGameCanvas>();

        canvasChangeCounterComponent = canvasPaintableTexComponent.GetComponent<P3dChangeCounter>();
        paint3DColor = GetComponent<P3dColor>();
        dialogueSystemTrigger=GetComponent<DialogueSystemTrigger>();
    }
    /*    public override void InitializeMiniGame()
        {
            models.SetActive(true);
            canvas.gameObject.SetActive(false);
            miniGameCam.gameObject.SetActive(true); //this will be the same

            gameController = GameController.Instance;
            gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

            FinalEliminationManager.Instance.SetSelectedMiniGame(this);
        }*/

    protected override void OnEliminateButtonPressed()
    {
        base.OnEliminateButtonPressed();
    }
    protected override void Start()
    {
        base.Start();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        dialogueSystemEvents = GetComponent<DialogueSystemEvents>();


        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener((x) => finalEliminationManager.StartPhase());
    }

    private void Update()
    {
        if (isFirstPhaseActive)
        {

            if (!paintingMiniGameCanvas.CurActiveGroup.activeSelf)
            {
                isFirstPhaseActive = false;
                //paintingMiniGameCanvas.continueButton.onClick.RemoveAllListeners();
                paintingMiniGameCanvas.continueButton.onClick.AddListener(ContinueToSecondPhase);
                Invoke(nameof(ShowContinueButtonAfterDelay), 3f);
            }
            /*             if(canvasChangeCounterComponent.Ratio==canvasBackgroundFilledRatio)
                        {
                            isFirstPhaseActive = false;
                            paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.thirdPlanGroup,false);
                            paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup, true);
                        } */
        }

        if (isStencilPhaseActive)
        {
            //if (Mathf.Approximately(canvasChangeCounterComponent.Ratio, ratioForCompletenessOfStencil))
            //{

            if (!paintingMiniGameCanvas.CurActiveGroup.activeSelf)
            {
                isStencilPhaseActive = false;


                if (waitForCanvasPaintCoroutine != null)
                {
                    StopCoroutine(waitForCanvasPaintCoroutine);
                    waitForCanvasPaintCoroutine = null;
                }

                paintingMiniGameCanvas.continueButton.onClick.AddListener(ContinueFromStencilPhase);
                Invoke(nameof(ShowContinueButtonAfterDelay), 3f);

                /*    curStencil.transform.DOMove(startingPlaceForStencil.position, 0.5f).OnComplete(() =>
                   {
                       curStencil.SetActive(false);
                       curStencil = null;
                       if (paintingMiniGameCanvas.CurActiveGroup == paintingMiniGameCanvas.secondPlanGroup)
                       {
                           paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup, false);
                           paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, true);
                       }
                       else
                       {
                           paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, false);
                           //and something else here, like go to final elimination
                       }

                   }); */

                //activate third phase
                //}
                //Debug.Log(canvasChangeCounterComponent.Ratio);
                // Debug.Log(colorCounterComponent.Count(paint3DColor));
            }
        }
        //Debug.Log(canvasChangeCounterComponent.Ratio);
    }
    public void TriggerMiniGame()
    {
        canvas.gameObject.SetActive(true);
        paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.thirdPlanGroup, true);
        //waitForCanvasPaintCoroutine=StartCoroutine(WaitForCanvasHit());
        // paintingMiniGameCanvas.thirdPlanGroup.SetActive(true);
        // isFirstPhaseActive = true;
        //and some other things
    }

    private void ShowContinueButtonAfterDelay()
    {
        paintingMiniGameCanvas.continueButton.gameObject.SetActive(true);
    }

    public void SetBackgroundTexture(Texture tex)
    {
        paintSphere.BlendMode = P3dBlendMode.ReplaceCustom(Color.white, tex, new Vector4(1, 1, 1, 1));
        paintSphere.gameObject.SetActive(true);

        //  canvasChangeCounterComponent.Texture = tex;

        isFirstPhaseActive = true;
        if (waitForCanvasPaintCoroutine == null)
            waitForCanvasPaintCoroutine = StartCoroutine(WaitForCanvasHit());
    }

    private void ContinueToSecondPhase()
    {
        //isFirstPhaseActive = false;
        //paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.thirdPlanGroup, false);


        paintingMiniGameCanvas.continueButton.onClick.RemoveListener(ContinueToSecondPhase);
        paintingMiniGameCanvas.continueButton.gameObject.SetActive(false);  //or hide it with animation
        paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup, true);
    }

    private void ContinueFromStencilPhase()
    {
        //isFirstPhaseActive = false;
        //paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.thirdPlanGroup, false);


        curStencil.transform.DOMove(startingPlaceForStencil.position, 0.5f).OnComplete(() =>
             {
                 curStencil.SetActive(false);
                 curStencil = null;
                 if (paintingMiniGameCanvas.CurActiveGroup == paintingMiniGameCanvas.secondPlanGroup)
                 {
                     //paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup, false);
                     paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, true);
                 }
                 else
                 {
                     paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, false);
                     Invoke(nameof(TransitionToContestants), 1f);
                     //and something else here, like go to final elimination
                 }

             });

        paintingMiniGameCanvas.continueButton.onClick.RemoveListener(ContinueFromStencilPhase);
        paintingMiniGameCanvas.continueButton.gameObject.SetActive(false);  //or hide it with animation
        // paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, true);
    }

    public void SetStencil(Texture invertedTexture, GameObject stencilGameObject, Color brushColor, float ratioForCompleteness)
    {
        isStencilPhaseActive = true;

        if (waitForCanvasPaintCoroutine != null)
        {
            StopCoroutine(waitForCanvasPaintCoroutine);
            waitForCanvasPaintCoroutine = null;
        }

        paintSphere.gameObject.SetActive(false);
        paintingMiniGameCanvas.SetEnabledButtonsInGroup(paintingMiniGameCanvas.CurActiveGroup, false);
        if (curStencil != null)
        {
            //paintSphere.gameObject.SetActive(false);
            curStencil.transform.DOMove(startingPlaceForStencil.position, 0.5f).OnComplete(() =>
            {
                curStencil.SetActive(false);

                curStencil = stencilGameObject;

                // Vector3 stencilFinalPos = curStencil.transform.position;

                //paintSphere.gameObject.SetActive(false);
                curStencil.transform.position = startingPlaceForStencil.position;
                curStencil.transform.DOMove(endPlaceForStencil.position, 0.5f).OnComplete(() =>
                {
                    AllowStencilPainting(invertedTexture, brushColor, ratioForCompleteness);
                });

                curStencil.SetActive(true);
            });

        }
        else
        {
            curStencil = stencilGameObject;

            // Vector3 stencilFinalPos = curStencil.transform.position;



            curStencil.transform.position = startingPlaceForStencil.position;
            curStencil.transform.DOMove(endPlaceForStencil.position, 0.5f).OnComplete(() =>
            {
                AllowStencilPainting(invertedTexture, brushColor, ratioForCompleteness);
            });

            curStencil.SetActive(true);     //tweening goes here
        }

    }

    private void AllowStencilPainting(Texture invertedTexture, Color brushColor, float ratioForCompleteness)
    {
        paintingMiniGameCanvas.SetEnabledButtonsInGroup(paintingMiniGameCanvas.CurActiveGroup, true);

        waitForCanvasPaintCoroutine = StartCoroutine(WaitForCanvasHit());

        canvasPaintableTexComponent.LocalMaskTexture = invertedTexture;

        //ratioForCompletenessOfStencil = ratioForCompleteness;

        //canvasChangeCounterComponent.Color = brushColor;
        //canvasChangeCounterComponent.MaskTexture = invertedTexture;

        //colorCounterComponent.MaskTexture = invertedTexture;
        //paint3DColor.Color = brushColor;
        //colorCounterComponent.Count(paint3DColor);

        //colorCounterComponent.Contributions.Add(p3dColor.Contribute())
        //p3DColor.Contribute(colorCounterComponent,1);


        if (paintSphere.BlendMode.Index != P3dBlendMode.ALPHA_BLEND)
        {
            paintSphere.BlendMode = P3dBlendMode.AlphaBlend(new Vector4(1, 1, 1, 1));
        }

        paintSphere.Color = brushColor;
        paintSphere.gameObject.SetActive(true);
    }

    public void TransitionToContestants()
    {
        //set lipstick materials to contestants
        paintSphere.gameObject.SetActive(false);

        ContestantScript winnerContestant = finalEliminationManager.contestants[1], loserContenstant = finalEliminationManager.contestants[0];

        leftContestantFailCanvas.SetCanvasMaterial(canvasFailMat);
        leftContestantFailCanvas.gameObject.SetActive(true);
        rightContestantWinCanvas.SetCanvasMaterial(canvasGameObject.GetComponent<MeshRenderer>().material);
        rightContestantWinCanvas.gameObject.SetActive(true);

        // winnerContestant.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(contestantGoodLipstickMatsDict[winnerContestant.contestantModelType]);
        // loserContenstant.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(contestantBadLipstickMatsDict[loserContenstant.contestantModelType]);

        CameraController.Instance.transitionToCMVirtualCamera(contestantsResultsCam);
        CheckForCameraBlending.onCameraBlendFinished += ActionWhenCameraOnContestants;
    }

    public void ActionWhenCameraOnContestants()
    {
        //dogAnimator0.SetTrigger("Spin");
        //dogAnimator1.SetTrigger("Bark");
        //  finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().SetTrigger("Happy");
        //  finalEliminationManager.contestants[1].GetComponentInChildren<Animator>().SetTrigger("Sad");

        finalEliminationManager.contestants[1].MatchSuccessPoints++;

       // ToonModelScript playerScriptModel = PlayerInMiniGameGO.GetComponentInChildren<ToonModelScript>();
        //Debug.Log(playerScriptModel);
       // PlayerInMiniGameGO.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(girlGoodLipstickMat);

        Invoke(nameof(StartFinalEliminationConversation), 0.75f);
        //   StartCoroutine(WaitForIdle());   ovde sam stao pre nego sto je god emperor branima stigao
        CheckForCameraBlending.onCameraBlendFinished -= ActionWhenCameraOnContestants;
    }

    private void StartFinalEliminationConversation()
    {
        dialogueSystemTrigger.enabled = true;
    }

    IEnumerator WaitForCanvasHit()
    {
        //        Debug.Log("WaitForCanvasHit() start");
        Ray ray;
        RaycastHit hit;
        GameObject hitObject;

        bool wasHit = false;

        while (!wasHit)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.collider.gameObject;
                if (hitObject == curStencil || hitObject == canvasGameObject)
                {
                    //paintingMiniGameCanvas.DisableStencilGroupSelection();
                    paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.CurActiveGroup, false);
                    //  Debug.Log("paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.CurActiveGroup, false);");
                    wasHit = true;
                    // isStencilPhaseActive = true;
                }
            }
            //}
            if (!wasHit) yield return null;
        }

        //StopCoroutine(waitForCanvasPaintCoroutine);
        //waitForCanvasPaintCoroutine
    }
}
