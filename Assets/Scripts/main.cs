using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;

public class main : MonoBehaviour
{
    // Handler for SkeletalTracking thread.
    public TrackerHandler m_tracker;
    public DolphinMover m_dolphin;
    public CameraBehavior m_camera;
    public ButterflyBehavior m_butterflies;
    public ButterflyEyeBehavior m_eyeButterflies;
    public LeopardBehavior m_leopard;
    public ChameleonBehavior m_chameleon;
    public PsychadelicBehavior m_psychadelic;

    private int currSkinIndex;
    private int maxSkinEffects;
    private int currAnimIndex;
    private int maxAnimEffects;
    private bool bright_full = false;
    private bool drawDolphin = true;
    private bool drawButterflies = false;
    private bool drawEyes = false;
    private bool drawLeopard = false;
    private bool drawChameleon = false;
    private bool drawPsychadelic = false;
    private bool alreadyTrackedPos = false;
    private BackgroundDataProvider m_backgroundDataProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();
    private int waving = 0;
    private bool waveStarted = false;
    private float effectTimer = 0f;

    //RenderingPath userRenderPath;
    void Start()
    {
        SkeletalTrackingProvider m_skeletalTrackingProvider = new SkeletalTrackingProvider();

        //tracker ids needed for when there are two trackers
        const int TRACKER_ID = 0;
        m_skeletalTrackingProvider.StartClientThread(TRACKER_ID);
        m_backgroundDataProvider = m_skeletalTrackingProvider;
        m_dolphin.SetActive(true);
        m_butterflies.SetActive(false);
        m_eyeButterflies.SetActive(false);
        m_leopard.SetActive(false);
        m_chameleon.SetActive(false);
        m_psychadelic.SetActive(false);

        //skinEffectStates = new int[3];
        //for (int i = 0; i < skinEffectStates.Length; i++)
        //    skinEffectStates[i] = i;
        //animationEffectStates = new int[2];
        //for (int i = 0; i < animationEffectStates.Length; i++)
        //    animationEffectStates[i] = i;
        currAnimIndex = -1;
        currSkinIndex = -1;
        maxAnimEffects = 2;
        maxSkinEffects = 3;
    }

    void Update()
    {

        effectTimer += Time.deltaTime;

        if (m_backgroundDataProvider.IsRunning)
        {
            if (m_backgroundDataProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                if (m_lastFrameData.NumOfBodies != 0)
                {

                    waving = m_tracker.updateTracker(m_lastFrameData);
                    if (effectTimer >= 5f)
                    {
                        if (waving > 0)
                        {
                            waveStarted = true;

                            if (waving == 1)
                            {
                                Debug.Log("waving left");
                                currSkinIndex--;
                                if (currSkinIndex < 0)
                                    currSkinIndex = maxSkinEffects - 1;
                            }
                            else if (waving == 2)
                            {
                                Debug.Log("waving right");
                                currSkinIndex++;
                                if (currSkinIndex >= maxSkinEffects)
                                    currSkinIndex = 0;
                            }

                            else
                            {
                                Debug.Log("arms up");

                                currAnimIndex++;
                                if (currAnimIndex >= maxAnimEffects)
                                    currAnimIndex = 0;
                            }

                        }
                        else
                        {
                            waveStarted = false;

                        }
                        effectTimer = 0f;
                    }
                }
            }
        }
        if(!waveStarted)
        {
            bool canPlay = false;
            if (drawPsychadelic)
                canPlay = m_psychadelic.StopEffect();
            if (drawChameleon)
                canPlay = m_chameleon.StopEffect();
            if (drawLeopard)
                canPlay = m_leopard.StopEffect();
            if (drawEyes)
                canPlay = StopEyesToDolphin();
            if (drawButterflies)
                canPlay = StopButterfliesToDolphin();
            if (canPlay || drawDolphin)
            {
                canPlay = false;
                if (drawPsychadelic || drawChameleon || drawLeopard)
                    canPlay = m_dolphin.StartFadeIn();
                if (canPlay || drawButterflies || drawEyes || drawDolphin)
                {
                    if (m_dolphin.GetBloom() > 0)
                        m_dolphin.DecrBloom();
                    m_dolphin.swimAround();
                    drawLeopard = true;
                    drawPsychadelic = false;
                    drawChameleon = false;
                    drawDolphin = true;
                    drawEyes = false;
                    drawButterflies = false;
                }
            }

        }

        else
        {

            if (waving < 3)
            {
                switch (currSkinIndex)
                {
                    case 0:
                        playLeopard();
                        break;
                    case 1:
                        playChameleon();
                        break;
                    case 2:
                        playPsychadelic();
                        break;
                }


            }


            else
            {
                switch (currAnimIndex)
                {
                    case 0:
                        drawEyeButterflies();
                        break;
                    case 1:
                        drawParticleButterflies();
                        break;
                }
            }


        }



    }

    private bool StopEyesToDolphin()
    {
        if (!m_eyeButterflies.getStopMoving())
            m_eyeButterflies.StopEffect();
        if (m_eyeButterflies.getMoveToMiddle())
            m_eyeButterflies.IncrBloom();

        if (m_eyeButterflies.GetBloom() >= 90)
        {
            m_dolphin.SetBloom(80);
            m_eyeButterflies.setMoveToMiddle(false);
            m_dolphin.SetActive(true);
            m_eyeButterflies.SetActive(false);
            return true;
        }
        return false;
    }

    private bool StopEyesEE()
    {
        if (!m_eyeButterflies.getStopMoving())
            m_eyeButterflies.StopEffect();
        if (m_eyeButterflies.getMoveToMiddle())
            m_eyeButterflies.IncrBloom();

        if (m_eyeButterflies.GetBloom() >= 90)
        {
            m_eyeButterflies.setMoveToMiddle(false);
            m_eyeButterflies.SetActive(false);
            return true;
        }
        return false;
    }

    private bool StopButterfliesToDolphin()
    {
        if (!alreadyTrackedPos)
        {
            m_butterflies.SetCurrPosition();
            alreadyTrackedPos = true;
        }
        m_butterflies.IncrBloom();

        if (!m_butterflies.getStopMoving())
            m_butterflies.StopEffect();


        else if (m_butterflies.GetBloom() >= 90)
        {
            alreadyTrackedPos = false;
            m_dolphin.SetBloom(80);
            m_dolphin.SetActive(true);
            m_butterflies.SetActive(false);
            return true;
        }
        return false;
    }

    private bool StopButterfliesEE()
    {
        if (!alreadyTrackedPos)
        {
            m_butterflies.SetCurrPosition();
            alreadyTrackedPos = true;
        }
        m_butterflies.IncrBloom();

        if (!m_butterflies.getStopMoving())
            m_butterflies.StopEffect();


        else if (m_butterflies.GetBloom() >= 90)
        {
            alreadyTrackedPos = false;
            m_butterflies.SetActive(false);
            return true;
        }
        return false;
    }

    private void playLeopard()
    {
        bool canPlay = false;
        if (drawPsychadelic)
            canPlay = m_psychadelic.StopEffect();
        else if (drawChameleon)
            canPlay = m_chameleon.StopEffect();
        else if (drawDolphin)
            canPlay = m_dolphin.StartFadeOut();
        else if (drawEyes)
            canPlay = StopEyesEE();
        else if (drawButterflies)
            canPlay = StopButterfliesEE();
        if (canPlay || drawLeopard)
        {
            m_leopard.PlayEffect();
            drawLeopard = true;
            drawPsychadelic = false;
            drawChameleon = false;
            drawDolphin = false;
            drawEyes = false;
            drawButterflies = false;
        }

    }

    private void playChameleon()
    {
        bool canPlay = false;
        if (drawPsychadelic)
            canPlay = m_psychadelic.StopEffect();
        else if (drawLeopard)
            canPlay = m_leopard.StopEffect();
        else if (drawDolphin)
            canPlay = m_dolphin.StartFadeOut();
        else if (drawEyes)
            canPlay = StopEyesEE();
        else if (drawButterflies)
            canPlay = StopButterfliesEE();
        if (canPlay || drawChameleon)
        {
            m_chameleon.PlayEffect();
            drawLeopard = false;
            drawPsychadelic = false;
            drawChameleon = true;
            drawDolphin = false;
            drawEyes = false;
            drawButterflies = false;
        }
    }

    private void playPsychadelic()
    {
        bool canPlay = false;
        if (drawChameleon)
            canPlay = m_chameleon.StopEffect();
        else if (drawLeopard)
            canPlay = m_leopard.StopEffect();
        else if (drawDolphin)
            canPlay = m_dolphin.StartFadeOut();
        else if (drawEyes)
            canPlay = StopEyesEE();
        else if (drawButterflies)
            canPlay = StopButterfliesEE();
        if (canPlay || drawPsychadelic)
        {
            m_psychadelic.PlayEffect();
            drawLeopard = false;
            drawPsychadelic = true;
            drawChameleon = false;
            drawDolphin = false;
            drawEyes = false;
            drawButterflies = false;
        }
    }

    private void drawEyeButterflies()
    {

        bool canPlay = false;
        if (drawChameleon)
            canPlay = m_chameleon.StopEffect();
        else if (drawLeopard)
            canPlay = m_leopard.StopEffect();
        else if (drawPsychadelic)
            canPlay = m_psychadelic.StopEffect();
        else if (drawDolphin)
        {
            if (!m_dolphin.getStopMoving())
                m_dolphin.StartingWave();

            else
            {
                if (m_dolphin.GetBloom() < 80)
                {
                    m_dolphin.IncrBloom();
                }
                else
                {
                    m_dolphin.SetActive(false);
                    canPlay = true;
                }

            }
        }
        else if (drawButterflies)
        {
            if (m_butterflies.GetBloom() < 90)
            {
                m_butterflies.IncrBloom();
            }
            else
            {
                m_butterflies.SetActive(false);
                canPlay = true;
            }
        }
        if (canPlay || drawEyes)
        {
            m_eyeButterflies.SetActive(true);
            if (m_eyeButterflies.GetBloom() > 0f)
                m_eyeButterflies.DecrBloom();
            m_eyeButterflies.PlayEffect();
            drawLeopard = false;
            drawPsychadelic = false;
            drawChameleon = false;
            drawDolphin = false;
            drawEyes = true;
            drawButterflies = false;
        }



    }

    private void drawParticleButterflies()
    {

        bool canPlay = false;
        if (drawChameleon)
            canPlay = m_chameleon.StopEffect();
        else if (drawLeopard)
            canPlay = m_leopard.StopEffect();
        else if (drawPsychadelic)
            canPlay = m_psychadelic.StopEffect();
        else if (drawDolphin)
        {
            if (!m_dolphin.getStopMoving())
                m_dolphin.StartingWave();

            else
            {
                if (m_dolphin.GetBloom() < 80)
                {
                    m_dolphin.IncrBloom();
                }
                else
                {
                    m_dolphin.SetActive(false);
                    canPlay = true;
                }

            }
        }
        else if (drawEyes)
        {
            if (m_eyeButterflies.GetBloom() < 90)
            {
                m_eyeButterflies.IncrBloom();
            }
            else
            {
                m_eyeButterflies.SetActive(false);
                canPlay = true;
            }
        }    
        if (canPlay || drawButterflies)
        {
            m_butterflies.SetActive(true);
            if (m_butterflies.GetBloom() > 0f)
                m_butterflies.DecrBloom();
            drawLeopard = false;
            drawPsychadelic = false;
            drawChameleon = false;
            drawDolphin = false;
            drawEyes = false;
            drawButterflies = true;
        }
       
    }

    void OnDestroy()
    {
        // Stop background threads.
        if (m_backgroundDataProvider != null)
        {
            m_backgroundDataProvider.StopClientThread();
        }
    }

}
