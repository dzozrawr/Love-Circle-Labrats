// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections;
using UnityEngine;

namespace PixelCrushers.DialogueSystem.Wrappers
{

    /// <summary>
    /// This wrapper class keeps references intact if you switch between the 
    /// compiled assembly and source code versions of the original class.
    /// </summary>
    [HelpURL("http://www.pixelcrushers.com/dialogue_system/manual2x/html/standard_u_i_subtitle_panel.html")]
    [AddComponentMenu("Pixel Crushers/Dialogue System/UI/Standard UI/Dialogue/Standard UI Subtitle Panel")]
    public class StandardUISubtitlePanel : PixelCrushers.DialogueSystem.StandardUISubtitlePanel
    {
        private bool shouldCheckIfTypeWriterIsStillPlaying = false;
        private AbstractTypewriterEffect typeWriter;
        protected override void SetSubtitleTextContent(Subtitle subtitle)
        {
            base.SetSubtitleTextContent(subtitle);
            HideContinueButton();
            if (HasTypewriter()) //if it has the typewriter, it started typing
            {
                shouldCheckIfTypeWriterIsStillPlaying = true;
               // Debug.LogError("shouldCheckIfTypeWriterIsStillPlaying = true;");
            }
        }

        protected override IEnumerator ShowSubtitleAfterClosing(Subtitle subtitle)
        {
         //   shouldShowContinueButton = false;
            float safeguardTime = Time.realtimeSinceStartup + WaitForCloseTimeoutDuration;
            while (dialogueUI.AreAnyPanelsClosing() && Time.realtimeSinceStartup < safeguardTime)
            {
                yield return null;
            }
            ShowSubtitleNow(subtitle);
         //   if (shouldShowContinueButton) ShowContinueButton();
            m_showAfterClosingCoroutine = null;
        }

        public override void ShowContinueButton()
        {
            if (!typeWriter) return;
            if (!typeWriter.isPlaying)
            {
                base.ShowContinueButton();
            }
        }

        private new void Start()
        {
            base.Start();
            typeWriter = GetTypewriter();
        }

        private new void Update()
        {
            base.Update();
            if (shouldCheckIfTypeWriterIsStillPlaying)
            {
                var typewriter = GetTypewriter();
                if (typewriter != null && !typewriter.isPlaying)
                {
                    shouldCheckIfTypeWriterIsStillPlaying = false;
                    ShowContinueButton();
                    //Debug.LogError(" ShowContinueButtonNow();");
                }
            }
        }
    }

}
