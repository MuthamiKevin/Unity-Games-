using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public AudioClip beforeDominoClip; // Assign this in the inspector
    public AudioClip lastDominoClip; // Assign this in the inspector

    private AudioSource audioManagerSource; // Reference to the AudioManager's AudioSource
    private bool lastDominoHasFallen = false; // To ensure the last domino clip is played only once

    // Start is called before the first frame update
    void Start()
    {
        // Find the AudioManager GameObject in the scene
        GameObject audioManager = GameObject.Find("AudioManager");

        // Check if the AudioManager GameObject and its AudioSource component exist
        if (audioManager != null)
        {
            audioManagerSource = audioManager.GetComponent<AudioSource>();
            // Play the "before domino" clip
            if (beforeDominoClip != null && audioManagerSource != null)
            {
                // Disable physics of all game objects with the "Dom" tag
                DisablePhysicsOfDominoes();
                // Play the "before domino" clip
                audioManagerSource.clip = beforeDominoClip;
                audioManagerSource.Play();
                // Delay the game until the "before domino" clip finishes playing
                StartCoroutine(DelayBeforeDominoClip());
            }
            else
            {
                Debug.LogError("Before Domino Clip or AudioManager AudioSource not assigned!");
            }
        }
        else
        {
            Debug.LogError("AudioManager GameObject not found!");
        }
    }

    IEnumerator DelayBeforeDominoClip()
    {
        // Wait for the duration of the "before domino" clip
        yield return new WaitForSeconds(beforeDominoClip.length);

        // Enable physics of all game objects with the "Dom" tag
        EnablePhysicsOfDominoes();
    }

    void DisablePhysicsOfDominoes()
    {
        // Find all GameObjects with the "Dom" tag
        GameObject[] dominoes = GameObject.FindGameObjectsWithTag("Dom");
        foreach (GameObject domino in dominoes)
        {
            Rigidbody dominoRigidbody = domino.GetComponent<Rigidbody>();
            if (dominoRigidbody != null)
            {
                dominoRigidbody.isKinematic = true; // Disable physics simulation
            }
        }
    }

    void EnablePhysicsOfDominoes()
    {
        // Find all GameObjects with the "Dom" tag
        GameObject[] dominoes = GameObject.FindGameObjectsWithTag("Dom");
        foreach (GameObject domino in dominoes)
        {
            Rigidbody dominoRigidbody = domino.GetComponent<Rigidbody>();
            if (dominoRigidbody != null)
            {
                dominoRigidbody.isKinematic = false; // Enable physics simulation
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the last domino if it hasn't fallen yet
        if (!lastDominoHasFallen)
        {
            // Find all GameObjects with the "Dom" tag
            GameObject[] dominoes = GameObject.FindGameObjectsWithTag("Dom");
            int standingDominoes = dominoes.Length;

            foreach (GameObject domino in dominoes)
            {
                // Assuming a domino has fallen if its Z rotation is not 0
                // You might need a more complex check depending on your setup
                if (Mathf.Abs(domino.transform.eulerAngles.z) > 5)
                {
                    standingDominoes--;
                }
            }

            // If all dominoes have fallen
            if (standingDominoes == 0 && audioManagerSource != null)
            {
                // Play the last domino clip using the AudioManager's AudioSource
                audioManagerSource.clip = lastDominoClip;
                audioManagerSource.Play();
                lastDominoHasFallen = true;
            }
        }
    }
}
