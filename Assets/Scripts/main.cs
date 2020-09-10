using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;

public class main : MonoBehaviour
{
    // Handler for SkeletalTracking thread.
    public GameObject m_tracker;
    public DolphinMover m_dolphin;
    public GameObject m_camera;
    public GameObject m_water;
    private BackgroundDataProvider m_backgroundDataProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();
    private bool waving = false;
    private float speed = 0.005f;
    private float t = 0.0f;
    private float level_out_t = 0.0f;
    private Vector3 initialPosition, startPosition, endPosition, finalPosition;
    private Quaternion startRotation, ascEndRotation, descEndRotation;
    private int wavingTimer = 5;
    private bool wasWaving = false;
    private bool falling = false;
    private bool waveStarted = false;
    void Start()
    {
        SkeletalTrackingProvider m_skeletalTrackingProvider = new SkeletalTrackingProvider();

        //tracker ids needed for when there are two trackers
        const int TRACKER_ID = 0;
        m_skeletalTrackingProvider.StartClientThread(TRACKER_ID);
        m_backgroundDataProvider = m_skeletalTrackingProvider;
        initialPosition = m_dolphin.transform.position;
        startPosition = initialPosition;
        finalPosition = new Vector3(0, 3000, 0);
        endPosition = finalPosition;
        startRotation = m_dolphin.transform.rotation;
        ascEndRotation = new Quaternion(0, 0, Mathf.PI / 4, 1);
        descEndRotation = new Quaternion(0, 0, -Mathf.PI / 4, 1);
    }

    void Update()
    {



        if (m_backgroundDataProvider.IsRunning)
        {
            if (m_backgroundDataProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                if (m_lastFrameData.NumOfBodies != 0)
                {

                    waving = m_tracker.GetComponent<TrackerHandler>().updateTracker(m_lastFrameData);
                    if(waving)
                        waveStarted = true;
                }
            }
        }
        if (!waveStarted)
        {
            m_dolphin.swimAround();
            initialPosition = m_dolphin.transform.position;
            startPosition = initialPosition;
            startRotation = m_dolphin.transform.rotation;
        }
        else
        {

            if (waving)
            {

                if (falling)
                {
                    startRotation = m_dolphin.transform.rotation;
                    startPosition = m_dolphin.transform.position;
                    endPosition = finalPosition;
                    t = 0.0f;
                    speed = 0.005f;

                }
                wasWaving = true;
                falling = false;
                wavingTimer--;

            }

            if (wasWaving && wavingTimer < 5)
            {
                t += Time.deltaTime * speed;
                Quaternion interpolatedRotation = Quaternion.Lerp(startRotation, ascEndRotation, t*100.0f);
                m_dolphin.setRotation(interpolatedRotation);
                Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);
                m_dolphin.setPosition(interpolatedPosition);
                m_camera.transform.position = new Vector3(m_camera.transform.position.x, m_dolphin.transform.position.y, m_camera.transform.position.z);
                wavingTimer--;
                
            }
            else if (falling)
            {
               
                t += Time.deltaTime * speed;
                Quaternion interpolatedRotation;
                if (m_dolphin.transform.position.y <= (m_water.transform.position.y + 20))
                {
                    level_out_t += Time.deltaTime * 1.1f;

                    interpolatedRotation = Quaternion.Lerp(descEndRotation, new Quaternion(descEndRotation.x, descEndRotation.y, 0, 1), level_out_t);
                    m_dolphin.setRotation(interpolatedRotation);
                }

                else
                {
                    interpolatedRotation = Quaternion.Lerp(startRotation, descEndRotation, t);
                    m_dolphin.setRotation(interpolatedRotation);
                }
                Vector3 interpolatedPosition = Vector3.Lerp(startPosition, endPosition, t);
                m_dolphin.setPosition(interpolatedPosition);
                m_camera.transform.position = new Vector3(m_camera.transform.position.x, m_dolphin.transform.position.y, m_camera.transform.position.z);
                speed += 0.001f;
            }
            if (wavingTimer < 0)
            {
                wasWaving = false;
                wavingTimer = 5;
            }
            if (!falling && !waving && !wasWaving && wavingTimer == 5)
            {
                speed = 0.005f;
                t = 0.0f;
                falling = true;
                startRotation = m_dolphin.transform.rotation;
                startPosition = m_dolphin.transform.position;
                endPosition = initialPosition;
            }
        }

    }

   

    public static float getAngle(Vector3 fr, Vector3 to, Vector3 up)
    {
        // http://answers.unity3d.com/questions/24983/how-to-calculate-the-angle-between-two-vectors.html       

        // the vector perpendicular to referenceForward (90 degrees clockwise)
        // (used to determine if angle is positive or negative)
        Vector3 referenceRight = Vector3.Cross(up, fr);

        // Get the angle in degrees between 0 and 180
        float angle = Vector3.Angle(to, fr);

        // Determine if the degree value should be negative. Here, a positive value
        // from the dot product means that our vector is the right of the reference vector
        // whereas a negative value means we're on the left.
        float sign = (Vector3.Dot(to, referenceRight) > 0.0f) ? 1.0f : -1.0f;

        float finalAngle = sign * angle;

        return finalAngle;

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
