using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.AppUI.UI;

public class Snake : MonoBehaviour
{
    private Vector2 snakeDirection = Vector2.right;
    private List<Transform> snakeSegments = new List<Transform>();

    private float initialMoveRate = 0.25f;
    private float moveRate = 0.25f;
    private float decrementRate = 0.009f;
    private float minMoveRate = 0.04f;


    private bool ignoreSelfCollisionThisStep;

    public GameObject snakeBodyPrefab;
    public GameObject warpBoundary;
    public TextMeshProUGUI scoreText, livesText;
    public Button mainMenuButton;
    private AudioSource audioSource;
    public AudioClip eatSound, moveSound, penaltySound;
    private Coroutine snakeMovementCoroutine;
    
   

    private int initialSnakeLength = 4;
    public int lives = 3;
    public int score = 0;


    private void Start()
    {
        snakeSegments.Add(transform); //Store the snake's head transform.
        for(int i = 1; i < initialSnakeLength; i++)
        {
            Grow();
        }
        snakeMovementCoroutine = StartCoroutine(MoveSnakeRoutine());
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;

        
        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        if(GameManager.instance.currentState == GameState.Playing)
        {
            // Assign direction based on user input
            UpdateDirectionOnInput();
        }
        
        
    }

    void UpdateDirectionOnInput()
    {
        // We restrict certain inputs based on direction of the snake to avoid instant collision. for ex: restrict going left when snake is moving in 'right' direction
        if (Input.GetKeyDown(KeyCode.W) && (snakeDirection != Vector2.down))
        {
            audioSource.PlayOneShot(moveSound, 1.0f);
            snakeDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && (snakeDirection != Vector2.up))
        {
            audioSource.PlayOneShot(moveSound, 1.0f);
            snakeDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && (snakeDirection != Vector2.left))
        {
            audioSource.PlayOneShot(moveSound, 1.0f);
            snakeDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && (snakeDirection != Vector2.right))
        {
            audioSource.PlayOneShot(moveSound, 1.0f);
            snakeDirection = Vector2.left;
        }
    }

    IEnumerator MoveSnakeRoutine()
    {
        while(GameManager.instance.currentState == GameState.Playing)
        {
            yield return new WaitForSeconds(moveRate);
            MoveSnake();

        }
    }

    public IEnumerator BlinkLives()
    {
        for(int i =0; i < 2; i++)
        {
            livesText.enabled = false;
            yield return new WaitForSeconds(0.25f);
            livesText.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void MoveSnake()
    {
        //Move the other segments of the snake from tail to head.
        for (int segmentIndex = snakeSegments.Count - 1; segmentIndex > 0; segmentIndex--)
        {
            snakeSegments[segmentIndex].position = snakeSegments[segmentIndex - 1].position;
        }

        Vector3 newHeadPosition = new Vector3(
            Mathf.Round(transform.position.x) + snakeDirection.x,
            Mathf.Round(transform.position.y) + snakeDirection.y,
            0.0f
            );

        newHeadPosition = WarpSnakeAroundBoundary(newHeadPosition);
        transform.position = newHeadPosition;
        ignoreSelfCollisionThisStep = false; // start enabling collision after one step of movement;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            audioSource.PlayOneShot(eatSound, 1.0f);
            Grow();  // Grow the snake after colliding with food
            GameManager.instance.UpdateScore();
            collision.gameObject.GetComponent<Food>().GenerateRandomSpawnPosition(); //Generate next random position for food
        }
        else if (collision.CompareTag("Body") && (ignoreSelfCollisionThisStep == false))  //since we are spawning the first new segment inside head, it triggers a false collision. TO avoid that we check if we are allowed to acknowledge the collision
        {
            audioSource.PlayOneShot(penaltySound, 1.0f);
            if (lives >= 0)
            {
                ResetSnake();
            }
        }
           
    }

    void ResetSnake()
    {
        
        GameManager.instance.UpdateLives();
        moveRate = initialMoveRate;

        for (int i = snakeSegments.Count - 1; i > 0 ; i--)  //Remove all segments of the snake except the head.
        {
            Destroy(snakeSegments[i].gameObject);
            snakeSegments.RemoveAt(i);
        }

        transform.position = Vector3.zero;
        for (int i = 1; i < initialSnakeLength; i++)
        {
            Grow();
        }

    }

    Vector3 WarpSnakeAroundBoundary(Vector3 coordinate)
    {
        Bounds boundingBox = warpBoundary.GetComponent<BoxCollider2D>().bounds;
     
        //Warp around x-axis
        if (coordinate.x > (boundingBox.max.x - 0.5))  //0.5 offset so that snake does not warp over the wall, but before it.
            coordinate.x = Mathf.Round(boundingBox.min.x + 1); //+1 to avoid being placed on the wall
        else if (coordinate.x < (boundingBox.min.x + 0.5))
        {
            coordinate.x = Mathf.Round(boundingBox.max.x - 1);
        }

        //warp around y-axis
        if (coordinate.y > (boundingBox.max.y - 0.5))
        {
            coordinate.y = Mathf.Round(boundingBox.min.y + 1);
        }
        else if (coordinate.y < (boundingBox.min.y + 0.5))
        {
            coordinate.y = Mathf.Round(boundingBox.max.y - 1);
        }

        return coordinate;
       
    }
    void Grow()
    {   
        //Grab the tail position and instantiate a new segment at the tail position.
        Transform tail = snakeSegments[snakeSegments.Count - 1];
        GameObject newSegment = Instantiate(snakeBodyPrefab, tail.position, Quaternion.identity);
        snakeSegments.Add(newSegment.transform);

        ignoreSelfCollisionThisStep = true; // Set the flag to avoid false overlap triggers
    }

    public void UpdateMoveRate()
    {
        moveRate -= decrementRate;
        moveRate = Mathf.Max(moveRate, minMoveRate);
    }
}
