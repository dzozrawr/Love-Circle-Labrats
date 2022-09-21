using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using DG.Tweening;

namespace Contestant
{
    public enum ContestantModelType
    {
        TP_E_MaleA01, TP_E_MaleB01, TP_E_MaleC01, TP_E_MaleD01, TPMaleA01,
        TPMaleB01, TPMaleC01, TPMaleD01, TTBoyA01, TTBoyB01, TTBoyC01, TTBoyD01
    }
    public class ContestantScript : MonoBehaviour
    {
        private static int numberOfFactorsForMatchSuccess = 2;
        public CinemachineVirtualCamera cam = null;
        public Sprite thumbsUpSprite = null;
        public Sprite thumbsDownSprite = null;

        public Image thumbsUpOrDownImage = null;

        public GameObject hitboxForSelection = null;
        public Outline selectedIndicator = null;

        public DialogueSystemTrigger dialogueSystemTrigger = null;


        public Animator animator = null;

        public CinemachineVirtualCamera cameraFollow = null;

        public ContestantModelType contestantModelType;

        private Vector3 holePreferredScale;

        private bool isSelected = false;

        private VoiceLineManagerClass voiceLineManagerClass;

        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public int MatchSuccessPoints { get => matchSuccessPoints; set => matchSuccessPoints = value; }

        private GameController gameController;

        private Rigidbody parentRb = null;
        private Collider parentCollider = null;
        private GameObject model = null;

        private int matchSuccessPoints = 0;

        private AudioSource audioSource = null;

        private void Awake()
        {
            selectedIndicator.enabled = false;
            /*         if (hole != null)
                    {
                        holePreferredScale = hole.transform.localScale;
                        hole.transform.localScale = new Vector3(0, hole.transform.localScale.y, 0);
                    } */
            parentRb = GetComponent<Rigidbody>();
            parentCollider = GetComponent<Collider>();

            voiceLineManagerClass = new VoiceLineManagerClass(GetComponent<AudioSource>(), animator);
        }

        // Start is called before the first frame update
        void Start()
        {
            gameController = GameController.Instance;
            gameController.OnConversationChanged += OnConversationChanged;

            model = animator.gameObject;

            SetRagdollRigidbodyState(false);
            SetColliderState(false);

            audioSource = GetComponent<AudioSource>();

            SoundManager.Instance.AudioSourcesOfVoices.Add(audioSource);
            // audioSource.enabled= (PlayerPrefs.GetInt("voices", -1) != 0);

            if (PlayerPrefs.GetInt("voices", -1) != 0)
            {
                audioSource.volume = 1f;
            }
            else
            {
                audioSource.volume = 0f;
            }
        }

        [ContextMenu("RestartConversation")]
        public void RestartConversation()
        {
            dialogueSystemTrigger.GetComponent<Collider>().enabled = false;
            dialogueSystemTrigger.GetComponent<Collider>().enabled = true;

            // GameCanvasController.Instance.ShowThumbsUpDown(false);
        }

        private void Update()
        {
            voiceLineManagerClass.DoUpdate();
        }

        public void SetRagdollRigidbodyState(bool state)
        {
            Rigidbody[] rigidbodies = model.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rigidbodies)
            {
                if (parentRb == rb) continue;
                rb.isKinematic = !state;
            }
        }

        public Rigidbody GetPelvisRigidBody()
        {
            Rigidbody[] rigidbodies = model.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb.gameObject.name.Contains("Pelvis")) return rb;
            }

            return null;
        }

        public void SetColliderState(bool state)
        {
            Collider[] colliders = model.GetComponentsInChildren<Collider>();

            foreach (Collider c in colliders)
            {
                c.enabled = state;
            }

            parentCollider.enabled = !state;
        }

        private void OnConversationChanged(string conversationName)
        {
            dialogueSystemTrigger.conversation = conversationName;
            // Debug.LogError(gameObject.name + " conversation changed to " + conversationName);
        }

        public void SetThumbsUpOrDown(bool isThumbsUp)
        {
            if (isThumbsUp)
            {
                thumbsUpOrDownImage.sprite = thumbsUpSprite;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("Happy");
                matchSuccessPoints++;
            }
            else
            {
                thumbsUpOrDownImage.sprite = thumbsDownSprite;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("Sad");
            }
            thumbsUpOrDownImage.gameObject.SetActive(true);
        }

        public void ToggleSelectionPhase(bool onOff)
        {
            hitboxForSelection.SetActive(onOff);

        }

        public bool Select()
        {
            isSelected = !isSelected;
            selectedIndicator.enabled = isSelected;

            return isSelected;
        }

        public void Eliminate()
        {
            selectedIndicator.enabled = false;

            gameController.studioSet.EliminateContestant(this);
            /* 
                    animator.SetTrigger("Amaze");

                    hole.transform.DOMove(hole.transform.position + hole.transform.up * hole.GetComponent<Renderer>().bounds.size.x, 0.5f).onComplete = () =>
                    {
                        Invoke(nameof(DropTweenAnimationAfterDelay), 0.5f);
                    }; */

            // play animation of getting scared
            // add/activate trail
            // animation of falling down

        }


        public void FinalEliminate()
        {
            selectedIndicator.enabled = false;
            animator.SetTrigger("Fall");
        }

        public void WinnerAction()
        {
            animator.SetTrigger("Happy");
        }

        public float GetMatchSuccessRate()
        {
            return ((float)matchSuccessPoints) / ((float)numberOfFactorsForMatchSuccess);
        }

        public void TriggerAnimator(string triggerString)
        {
            animator.SetTrigger(triggerString);
        }
    }

}