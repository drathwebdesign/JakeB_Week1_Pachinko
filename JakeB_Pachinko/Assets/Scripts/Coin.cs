using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Iitem
{
    public static event Action<int> OnCoinCollect;
    public int score = 5;
    [SerializeField] AudioClip collectSoundClip;

    //Coin Stuck
    private Vector3 lastPosition;
    private float timeStuck;
    private float stuckThreshold = 1f; // Time (in seconds)

    private void Start() {
        lastPosition = transform.position;
        timeStuck = 0f;
    }

    private void Update() {
        CheckIfStuck();
    }

    public void Collect() {
        OnCoinCollect.Invoke(score);
        SoundFXManager.Instance.PlaySoundFXClip(collectSoundClip, transform, 1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Destroy(gameObject);
        }
    }

    private void CheckIfStuck() {
        if (transform.position == lastPosition) {
            timeStuck += Time.deltaTime; // Increment the stuck time.
        } else {
            timeStuck = 0f; // Reset if the coin has moved.
        }

        lastPosition = transform.position;

        // Destroy the coin if it's stuck for more than the threshold time.
        if (timeStuck >= stuckThreshold) {
            Destroy(gameObject);
        }
    }
}