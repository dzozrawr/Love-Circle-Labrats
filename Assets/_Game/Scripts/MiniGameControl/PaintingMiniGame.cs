using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using DG.Tweening;

public class PaintingMiniGame : MiniGame
{
    private static PaintingMiniGame instance = null;
    public P3dPaintSphere paintSphere = null;
    public P3dPaintableTexture canvasPaintableTexComponent = null;


    public float canvasBackgroundFilledRatio = 0.56256f;

    public Transform startingPlaceForStencil = null;
    public Transform endPlaceForStencil = null;
    public P3dColorCounter colorCounterComponent=null;


    private PaintingMiniGameCanvas paintingMiniGameCanvas = null;

    private P3dChangeCounter canvasChangeCounterComponent = null;

    private GameObject curStencil = null;
    private Camera mainCamera = null;

    private Coroutine waitForCanvasPaintCoroutine = null;

    private bool isFirstPhaseActive = false;
    private bool isStencilPhaseActive = false;

    private float ratioForCompletenessOfStencil=-1f;

    private P3dColor paint3DColor=null;

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
        paint3DColor=GetComponent<P3dColor>();
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

    /*    protected override void OnEliminateButtonPressed()
        {
            base.OnEliminateButtonPressed();
        }*/
    protected override void Start()
    {
        base.Start();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (isFirstPhaseActive)
        {
            if(canvasChangeCounterComponent.Ratio==canvasBackgroundFilledRatio)
            {
                isFirstPhaseActive = false;
                paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.thirdPlanGroup,false);
                paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup, true);
/*                paintingMiniGameCanvas.thirdPlanGroup.SetActive(false);
                paintingMiniGameCanvas.secondPlanGroup.SetActive(true);*/
            }
        }

        if (isStencilPhaseActive)
        {
            if (Mathf.Approximately(canvasChangeCounterComponent.Ratio, ratioForCompletenessOfStencil))
            {
                isStencilPhaseActive = false;
               

                if (waitForCanvasPaintCoroutine != null)
                {
                    StopCoroutine(waitForCanvasPaintCoroutine);
                    waitForCanvasPaintCoroutine = null;
                }

                curStencil.transform.DOMove(startingPlaceForStencil.position, 0.5f).OnComplete(() =>
                {
                    curStencil.SetActive(false);
                    curStencil = null;
                    if (paintingMiniGameCanvas.CurActiveGroup == paintingMiniGameCanvas.secondPlanGroup)
                    {
                        paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.secondPlanGroup,false);
                        paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, true);
                    }
                    else
                    {
                        paintingMiniGameCanvas.SetEnabledGroup(paintingMiniGameCanvas.firstPlanGroup, false);
                        //and something else here, like go to final elimination
                    }
                   
                });

                //activate third phase
            }
            //Debug.Log(canvasChangeCounterComponent.Ratio);
            Debug.Log(colorCounterComponent.Count(paint3DColor));
        }
        //Debug.Log(canvasChangeCounterComponent.Ratio);
    }
    public void TriggerMiniGame()
    {
        canvas.gameObject.SetActive(true);
        paintingMiniGameCanvas.thirdPlanGroup.SetActive(true);
       // isFirstPhaseActive = true;
        //and some other things
    }

    public void SetBackgroundTexture(Texture tex)
    {
        paintSphere.BlendMode = P3dBlendMode.ReplaceCustom(Color.white, tex, new Vector4(1, 1, 1, 1));
        paintSphere.gameObject.SetActive(true);

      //  canvasChangeCounterComponent.Texture = tex;

        isFirstPhaseActive = true;
    }

    public void SetStencil(Texture invertedTexture, GameObject stencilGameObject, Color brushColor, float ratioForCompleteness)
    {


        if (waitForCanvasPaintCoroutine != null)
        {
            StopCoroutine(waitForCanvasPaintCoroutine);
            waitForCanvasPaintCoroutine = null;
        }

        paintSphere.gameObject.SetActive(false);
        paintingMiniGameCanvas.SetEnabledButtonsInGroup(paintingMiniGameCanvas.CurActiveGroup,false);
        if (curStencil != null)
        {
            //paintSphere.gameObject.SetActive(false);
            curStencil.transform.DOMove(startingPlaceForStencil.position, 0.5f).OnComplete(()=>
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

        ratioForCompletenessOfStencil = ratioForCompleteness;

        canvasChangeCounterComponent.Color = brushColor;
        canvasChangeCounterComponent.MaskTexture = invertedTexture;

        colorCounterComponent.MaskTexture=invertedTexture;
        paint3DColor.Color=brushColor;
        colorCounterComponent.Count(paint3DColor);
        //colorCounterComponent.Contributions.Add(p3dColor.Contribute())
        //p3DColor.Contribute(colorCounterComponent,1);
        

        if (paintSphere.BlendMode.Index != P3dBlendMode.ALPHA_BLEND)
        {
            paintSphere.BlendMode = P3dBlendMode.AlphaBlend(new Vector4(1, 1, 1, 1));
        }

        paintSphere.Color = brushColor;
        paintSphere.gameObject.SetActive(true);
    }

    IEnumerator WaitForCanvasHit()
    {
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
                    if (hitObject == curStencil || hitObject == canvas)
                    {
                        paintingMiniGameCanvas.DisableStencilGroupSelection();
                        wasHit = true;
                        isStencilPhaseActive = true;
                    }
                }
            //}
            if (!wasHit) yield return null;
        }
        yield return null;
    }
}
