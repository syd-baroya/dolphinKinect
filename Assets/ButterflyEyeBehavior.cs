using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ButterflyEyeBehavior : MonoBehaviour
{
    ParticleSystem ps;
    Renderer renderer;
    Color[] colors;
    ParticleSystem.TextureSheetAnimationModule animToEye;
    ParticleSystem.NoiseModule noise;
    ParticleSystem.Particle[] particles;
    int numParticlesChanged = 0;
    int frameCount = 0;
    float timer = 0f;
    bool allParticlesChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        //enabled = false;
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
        renderer = GetComponent<Renderer>();
        colors = new[] {
            Color.blue,
            Color.magenta,
            Color.red,
            Color.green,
            Color.yellow
        };
        animToEye = ps.textureSheetAnimation;

        noise = ps.noise;
        var main = ps.main;
        var particleSystemGradient = main.startColor.gradient;
        var randomColors = new ParticleSystem.MinMaxGradient(particleSystemGradient);
        randomColors.mode = ParticleSystemGradientMode.RandomColor;
        main.startColor = randomColors;
    }

    // Update is called once per frame
    void Update()
    {
        if (ps == null)
        {
            enabled = false;
            return;
        }

        int numParticlesAlive = ps.GetParticles(particles);
        numParticlesAlive = numParticlesAlive <= 20 ? numParticlesAlive : 20;

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {

            if (particles[i].remainingLifetime <= 1f)
            {
                numParticlesChanged += 1;
                particles[i].remainingLifetime = 100000f;

                particles[i].velocity = Vector3.zero;
            }
        }

        // Apply the particle changes to the Particle System
        ps.SetParticles(particles, numParticlesAlive);

        if (numParticlesChanged == numParticlesAlive && numParticlesAlive != 0)
        {
            allParticlesChanged = true;
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].remainingLifetime = 100000f;

            }
        }
        if (allParticlesChanged)
        {
            timer += Time.deltaTime;
            noise.enabled = false;
            renderer.material.SetFloat("_Speed", 0);
            if (timer >= 1f && timer < 3.4f)
            {
                animToEye.fps = 4f;
                animToEye.mode = ParticleSystemAnimationMode.Grid;

            }
            else if (timer >= 3.4f)
            {
                animToEye.fps = 0.0001f;
                animToEye.startFrame = 1;
                animToEye.mode = ParticleSystemAnimationMode.Sprites;
            }

        }

    }
}
