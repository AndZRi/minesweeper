using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEffects : MonoBehaviour
{
    public GameObject DigParticlesPrefab;
    public GameObject RarityLight;

    private SpriteRenderer raritySprite;
    
    void Start()
    {
        raritySprite = RarityLight.GetComponent<SpriteRenderer>();
    }

    public void SummonDigVFX(RarityLevel level)
    {
        SummonDigParticles(level);
        RarityLightUp(level);
    }

    private void SummonDigParticles(RarityLevel level)
    {
        GameObject particles = Instantiate(DigParticlesPrefab, transform.position + new Vector3(0.5f, -0.5f), Quaternion.identity);
        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();

        // смешиваем цвет оригинальных частиц с цветом редкости
        if (level != null)
        {
            var particleSystemMainStartColor = particleSystem.main;
            particleSystemMainStartColor.startColor = (particleSystem.main.startColor.color + level.color) / 2;
        }

        particleSystem.Play();
        Destroy(particles, particleSystem.main.startLifetime.constant);
    }

    private void RarityLightUp(RarityLevel level)
    {
        if (level == null)
            return;

        raritySprite.color = level.color;

        Animator animatorController = RarityLight.GetComponent<Animator>();
        RarityLight.SetActive(true);
        animatorController.SetFloat("Speed", 1 / level.fadeTime);
    }
}
