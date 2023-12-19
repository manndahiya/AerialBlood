using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject deathVFX;
    [SerializeField] int scorePerHit = 25;
    [SerializeField] int hitPoints = 10;
    
    Scoreboard scoreBoard;
    GameObject parentGameObject;

    public TextMeshProUGUI gameOverText;
    private int numberOfEnemies;

    PlayerController playerController;

    private bool isGamePaused = false;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        scoreBoard = FindObjectOfType<Scoreboard>();
        AddRigidBody();
        InitializeEnemies();

    }

    private void Update()
    {
        // Initialize the number of enemies
        numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;


        Debug.Log(numberOfEnemies);
        // Check if all enemies are killed
        if (numberOfEnemies < 1)
        {
            // Trigger Game Over
            TriggerGameOver();
            GetComponent<PlayerController>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isGamePaused)
        {
            // Resume the game
            Time.timeScale = 1f;
            isGamePaused = false;
        }
        else
        {
            // Pause the game
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }
    void TriggerGameOver()
    {
        // Show the Game Over text
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            
        }

    }



    private void InitializeEnemies()
    {
        

        // Ensure the Game Over text is initially hidden
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }


    private void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
        rb.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hitPoints < 1)
        {
            KillEnemy();
            numberOfEnemies--;
            Debug.Log("Number of Enemies: " + numberOfEnemies);

            if(numberOfEnemies < 1)
            {
                TriggerGameOver();
                GetComponent<PlayerController>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                Time.timeScale = 0f;
                isGamePaused = true;
            }
        }
    }

    private void KillEnemy()
    {
        
        scoreBoard.IncreaseScore(scorePerHit);
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(gameObject);

       


    }

    private void ProcessHit()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        hitPoints--;
        scoreBoard.IncreaseScore(scorePerHit);
    }
}
