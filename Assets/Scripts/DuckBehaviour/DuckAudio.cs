using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class DuckAudio : MonoBehaviour
{

    [Header("Quack Type:")]
    [SerializeField] bool OnClick;
    [SerializeField] bool random;

    [Header("Quack properties")]
    [SerializeField] int minSecs;
    [SerializeField] int maxSecs;
    AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] List<AudioClip> quacks;
    [SerializeField] AudioClip collideSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (random) StartCoroutine(randomQuack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.clip = collideSound;
        audioSource.Play();
    }

    private void OnMouseDown()
    {
        if (OnClick) playQuack();
    }

    void playQuack()
    {
        audioSource.clip = quacks[Random.Range(0, quacks.Count)];
        audioSource.Play();
    }

    IEnumerator randomQuack()
    {
        yield return new WaitForSeconds(Random.Range(minSecs, maxSecs));
        playQuack();
        StartCoroutine(randomQuack());
    }
}
