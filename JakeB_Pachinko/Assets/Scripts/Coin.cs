using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Iitem
{
    public static event Action<int> OnCoinCollect;
    public int score = 5;
    public void Collect() {
        OnCoinCollect.Invoke(score);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Destroy(gameObject);
        }
    }
}