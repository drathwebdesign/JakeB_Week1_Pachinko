using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawControlRight : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    private Vector2 startPosition;
    private int direction = 1;
    [SerializeField] private float distance = 36f;

    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(0, 0, speed);

        float offset = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPosition + Vector2.right * offset * direction;

        if (offset >= distance) {
            direction *= -1;
        }
    }
}
