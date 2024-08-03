using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    int scoreAmount;
    public Slider scoreSlider;
    public Slider staminaSlider;
    private PlayerMovement playerMovement;

    //coins
    public GameObject coinPrefab; // The coin prefab to spawn
    public int maxCoins = 5; // Maximum number of coins
    public float spawnDelay = 2f; // Delay between spawns
    public Vector2 spawnAreaMin; // Minimum x and y values for spawn area
    public Vector2 spawnAreaMax; // Maximum x and y values for spawn area

    private List<GameObject> activeCoins = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        scoreAmount = 0;
        scoreSlider.value = 0;
        Coin.OnCoinCollect += IncreaseScoreAmount;

        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null) {
            staminaSlider.maxValue = playerMovement.MaxStamina; // Assuming MaxStamina is public
            staminaSlider.value = playerMovement.CurrentStamina; // Assuming CurrentStamina is public
        }

        // Start the coin spawning process
        StartCoroutine(SpawnCoins());
    }

    // Update is called once per frame
    void Update() {
        if (playerMovement != null) {
            staminaSlider.value = playerMovement.CurrentStamina;
        }
    }

    void IncreaseScoreAmount(int amount) {
        scoreAmount += amount;
        scoreSlider.value = scoreAmount;
        if (scoreAmount >= 100) {
            //levelComplete
            Debug.Log("Level Completed");
        }
    }

    IEnumerator SpawnCoins() {
        while (true) {
            // Clean up the activeCoins list by removing null entries (destroyed coins)
            activeCoins.RemoveAll(coin => coin == null);
            // Check if we need to spawn more coins
            if (activeCoins.Count < maxCoins) {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                // Instantiate the coin and add to the list
                GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                activeCoins.Add(newCoin);
            }

            // Wait for the specified delay before checking again
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void HandleCoinCollected(int score) {
        // Remove null entries from the list
        activeCoins.RemoveAll(coin => coin == null);
    }
}