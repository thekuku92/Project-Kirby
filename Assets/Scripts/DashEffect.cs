using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public ParticleSystem dashParticles; // Reference to the Particle System

    // Call this method to play the dash particle effect
    public void PlayDashParticles()
    {
        dashParticles.Play();
    }
}