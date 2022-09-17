using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class PaintingMiniGame : MiniGame
{
    private static PaintingMiniGame instance = null;
    public P3dPaintSphere paintSphere = null;
    public P3dPaintableTexture canvasPaintableTexComponent = null;


    public float canvasBackgroundFilledRatio = 0.56256f;


    private PaintingMiniGameCanvas paintingMiniGameCanvas = null;

    private P3dChangeCounter canvasChangeCounterComponent = null;

    private GameObject curStencil = null;
    private Camera mainCamera = null;

    private Coroutine waitForCanvasPaintCoroutine = null;

    private bool isFirstPhaseActive = false;
    private bool isSecondPhaseActive = false;

    private float ratioForCompletenessOfStencil=-1f;

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
                paintingMiniGameCanvas.thirdPlanGroup.SetActive(false);
                paintingMiniGameCanvas.secondPlanGroup.SetActive(true);
            }
        }

        if (isSecondPhaseActive)
        {
            if (Mathf.Approximately(canvasChangeCounterComponent.Ratio, ratioForCompletenessOfStencil))
            {
                isSecondPhaseActive = false;
                paintingMiniGameCanvas.secondPlanGroup.SetActive(false);
                curStencil.SetActive(false);//tweening goes here
                //activate third phase
            }
            //Debug.Log(canvasChangeCounterComponent.Ratio);
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
        if (curStencil != null)
        {
            curStencil.SetActive(false);//tweening goes here
        }
        if (waitForCanvasPaintCoroutine != null)
        {
            StopCoroutine(waitForCanvasPaintCoroutine);
            waitForCanvasPaintCoroutine = null;
        }

        curStencil = stencilGameObject;
        curStencil.SetActive(true);     //tweening goes here

        waitForCanvasPaintCoroutine = StartCoroutine(WaitForCanvasHit());

        canvasPaintableTexComponent.LocalMaskTexture = invertedTexture;

        ratioForCompletenessOfStencil = ratioForCompleteness;

       /* Destroy(canvasChangeCounterComponent);
        canvasChangeCounterComponent = null;

        canvasChangeCounterComponent=canvasPaintableTexComponent.gameObject.AddComponent<P3dChangeCounter>();
        canvasChangeCounterComponent.PaintableTexture = canvasPaintableTexComponent;*/

        canvasChangeCounterComponent.Color = brushColor;
        canvasChangeCounterComponent.MaskTexture = invertedTexture;

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
            if (Input.GetMouseButtonDown(0))
            {
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    hitObject = hit.collider.gameObject;
                    if (hitObject == curStencil || hitObject == canvas)
                    {
                        paintingMiniGameCanvas.DisableSecondPlanGroupSelection();
                        wasHit = true;
                        isSecondPhaseActive = true;
                    }
                }
            }
            if (!wasHit) yield return null;
        }
        yield return null;
    }
}
