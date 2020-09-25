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
                                currSkinIndex--;
                                if (currSkinIndex < 0)
                                    currSkinIndex = maxSkinEffects - 1;
                            }
                            else if (waving == 2)
                            {
                                currSkinIndex++;
                                if (currSkinIndex >= maxSkinEffects)
                                    currSkinIndex = 0;
                            }

                            else
                            {
                                currAnimIndex++;
                                if (currAnimIndex >= maxAnimEffects)
                                    currAnimIndex = 0;
                            }

                        }
                        else
                        {
                            waveStarted = false;
                            bright_full = false;

                        }
                        effectTimer = 0f;
                    }
                }
            }
        }
        if (!waveStarted)
        {
            if (drawButterflies)
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
                    drawDolphin = true;
                    drawButterflies = false;
                    m_dolphin.SetActive(true);
                    m_butterflies.SetActive(false);

                }
            }
            else if (drawEyes)
            {
                if (!m_eyeButterflies.getStopMoving())
                    m_eyeButterflies.StopEffect();
                if(m_eyeButterflies.getMoveToMiddle())
                    m_eyeButterflies.IncrBloom();

                if (m_eyeButterflies.GetBloom() >= 90)
                {
                    m_dolphin.SetBloom(80);
                    m_eyeButterflies.setMoveToMiddle(false);
                    drawDolphin = true;
                    drawEyes = false;
                    m_dolphin.SetActive(true);
                    m_eyeButterflies.SetActive(false);

                }
            }
            if (drawDolphin)
            {
                if (m_dolphin.GetBloom() > 0)
                    m_dolphin.DecrBloom();
                    m_dolphin.swimAround();
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

    private void playLeopard()
    {
        bool canPlay = false ;
        if(drawPsychadelic)
            canPlay = m_psychadelic.StopEffect();
        if (drawChameleon)
            canPlay = m_chameleon.StopEffect();
        if (drawDolphin)
            m_dolphin.StopEffect();
        if (drawEyes)

        if (drawButterflies)

        if (canPlay)
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
        bool canPlay;
        if (waving == 1)
            canPlay =  m_psychadelic.StopEffect();
        else
            canPlay = m_leopard.StopEffect();
        if (canPlay)
            m_chameleon.PlayEffect();
    }

    private void playPsychadelic()
    {
        bool canPlay;
        if (waving == 1)
            canPlay = m_chameleon.StopEffect();
        else
            canPlay = m_leopard.StopEffect();
        if (canPlay) 
            m_psychadelic.PlayEffect();
    }

    private void drawEyeButterflies()
    {

        if (!m_dolphin.getStopMoving())
            m_dolphin.StartingWave();

        else
        {
            if (m_dolphin.GetBloom() < 80 && !bright_full)
            {
                m_dolphin.IncrBloom();
            }
            else
            {
                if (drawDolphin)
                {
                    m_dolphin.SetActive(false);
                    drawDolphin = false;
                }
                if (!drawEyes)
                {
                    m_eyeButterflies.SetActive(true);
                    drawEyes = true;
                }
                bright_full = true;
                if (m_eyeButterflies.GetBloom() > 0f)
                    m_eyeButterflies.DecrBloom();
                m_eyeButterflies.PlayEffect();
            }

        }
    }

    private void drawParticleButterflies()
    {

        if (!m_dolphin.getStopMoving())
            m_dolphin.StartingWave();

        else
        {
            if (m_dolphin.GetBloom() < 80 && !bright_full)
            {
                m_dolphin.IncrBloom();
            }
            else
            {
                if (drawDolphin)
                {
                    m_dolphin.SetActive(false);
                    drawDolphin = false;
                }
                if (!drawButterflies)
                {
                    m_butterflies.SetActive(true);
                    drawButterflies = true;
                }
                bright_full = true;
                if (m_butterflies.GetBloom() > 0f)
                    m_butterflies.DecrBloom();

            }

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
