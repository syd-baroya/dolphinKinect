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
    private bool bright_full = false;
    private bool played = false;
    private bool drawDolphin = true;
    private bool drawButterflies = false;
    //public GameObject m_water;
    //public WaterLightBehavior m_waterlight;
    //public SunLightBehavior m_sunlight;
    //public GameObject m_terrain;
    private BackgroundDataProvider m_backgroundDataProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();
    private int waving = 0;
    //private float speed = 0.0025f;
    //private float t = 0.0f;
    //private float level_out_t = 0.0f;
    //private Vector3 initialPosition, startPosition, endPosition, finalPosition;
    //private Quaternion startRotation, ascEndRotation, descEndRotation;
    //private int wavingTimer = 5;
    //private bool wasWaving = false;
    //private bool falling = false;
    private bool waveStarted = false;
    //private float islandDimHeight = 0;
    //private float islandSpeedUpHeight = 0;
    //private float waterHeight = 0;

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
    }

    void Update()
    {



        if (m_backgroundDataProvider.IsRunning)
        {
            if (m_backgroundDataProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                if (m_lastFrameData.NumOfBodies != 0)
                {

                    waving = m_tracker.updateTracker(m_lastFrameData);
                    if (waving > 0)
                        waveStarted = true;
                    else
                        waveStarted = false;
                }
            }
        }
        if (!waveStarted)
        {
            m_dolphin.swimAround();
           
        }
        else
        {
            if (waving == 1)
            {
                if (m_eyeButterflies.Playing())
                {
                    m_dolphin.SetActive(true);
                    m_butterflies.SetActive(false);
                    m_eyeButterflies.Stop();
                }
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
                        m_butterflies.DecrBloom();

                    }

                }
            }

            else if(waving==2)
            {
                if (!m_eyeButterflies.Playing())
                {
                    m_dolphin.SetActive(false);
                    m_butterflies.SetActive(false);
                    m_eyeButterflies.Play();
                }
            }

        }
        //else
        //{

        //    if (waving)
        //    {

        //        if (falling)
        //        {
        //            startRotation = m_dolphin.transform.rotation;
        //            startPosition = m_dolphin.transform.position;
        //            endPosition = finalPosition;
        //            t = 0.0f;
        //            speed = 0.0025f;

        //        }
        //        wasWaving = true;
        //        falling = false;
        //        wavingTimer--;
        //    }

        //    if (wasWaving && wavingTimer < 5)
        //    {
        //        //m_camera.StartingWave();
        //        if (!m_dolphin.getStopMoving())
        //        {
        //            m_dolphin.StartingWave();
        //            initialPosition = m_dolphin.transform.position;
        //            startPosition = initialPosition;
        //            startRotation = m_dolphin.transform.rotation;
        //        }

        //        if (m_dolphin.transform.position.y >= waterHeight)
        //        {
        //            m_camera.setRenderingPath(RenderingPath.DeferredShading);
        //        }
        //        t += Time.deltaTime * speed;
        //        Quaternion interpolatedRotation = Quaternion.Lerp(startRotation, ascEndRotation, t * 100.0f);
        //        m_dolphin.setRotation(interpolatedRotation);
        //        Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);
        //        float camDeltaY = interpolatedPosition.y - m_dolphin.transform.position.y;
        //        m_dolphin.setPosition(interpolatedPosition);
        //        m_camera.setPosition(new Vector3(m_camera.transform.position.x, m_camera.transform.position.y + camDeltaY, m_camera.transform.position.z));
        //        if (waterHeight > m_dolphin.transform.position.y)
        //            m_waterlight.UpdateUp(initialPosition.y, waterHeight, m_dolphin.transform.position.y);
        //        else if (islandDimHeight > m_dolphin.transform.position.y)
        //            m_sunlight.UpdateUp(initialPosition.y, islandDimHeight, m_dolphin.transform.position.y);
        //        if (islandSpeedUpHeight < m_dolphin.transform.position.y)
        //        {
        //            speed += 0.0001f;
        //        }
        //        Debug.Log(speed);
        //        wavingTimer--;

        //    }
        //    else if (falling)
        //    {

        //        t += Time.deltaTime * speed;
        //        Quaternion interpolatedRotation;
        //        if (m_dolphin.transform.position.y <= waterHeight)
        //        {
        //            m_camera.setRenderingPath(RenderingPath.Forward);
        //            level_out_t += Time.deltaTime * 1.1f;
        //            interpolatedRotation = Quaternion.Lerp(descEndRotation, new Quaternion(descEndRotation.x, descEndRotation.y, 0, 1), level_out_t);
        //            m_dolphin.setRotation(interpolatedRotation);
        //        }

        //        else
        //        {
        //            interpolatedRotation = Quaternion.Lerp(startRotation, descEndRotation, t*100.0f);
        //            m_dolphin.setRotation(interpolatedRotation);
        //        }
        //        Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);
        //        float camDeltaY = m_dolphin.transform.position.y - interpolatedPosition.y;
        //        m_dolphin.setPosition(interpolatedPosition);
        //        m_camera.setPosition(new Vector3(m_camera.transform.position.x, m_camera.transform.position.y - camDeltaY, m_camera.transform.position.z));
        //        if(waterHeight > m_dolphin.transform.position.y)
        //            m_waterlight.UpdateDown(initialPosition.y, waterHeight, m_dolphin.transform.position.y);
        //        else if (islandDimHeight > m_dolphin.transform.position.y)
        //            m_sunlight.UpdateDown(initialPosition.y, islandDimHeight, m_dolphin.transform.position.y);
        //        speed += 0.00008f;
        //    }
        //    if (wavingTimer < 0)
        //    {
        //        wasWaving = false;
        //        wavingTimer = 5;
        //    }
        //    if (!falling && !waving && !wasWaving && wavingTimer == 5)
        //    {
        //        speed = 0.0025f;
        //        t = 0.0f;
        //        falling = true;
        //        startRotation = m_dolphin.transform.rotation;
        //        startPosition = m_dolphin.transform.position;
        //        endPosition = initialPosition;
        //    }
        //}

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
