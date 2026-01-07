using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 snakeDirection = Vector2.right;
    private float moveRate = 0.25f;
    private Coroutine moveSnakeRoutine;

   


    private void Start()
    {
        moveSnakeRoutine = StartCoroutine(MoveSnakeRoutine());
       
    }
    private void Update()
    {
        // Assign direction based on user input
        UpdateDirectionOnInput();
        
    }

    void UpdateDirectionOnInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            snakeDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            snakeDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            snakeDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            snakeDirection = Vector2.left;
        }
    }

    IEnumerator MoveSnakeRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(moveRate);
            MoveSnake();
        }
    }

    private void MoveSnake()
    {
        // change position of snake based on user input
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + snakeDirection.x,
            Mathf.Round(transform.position.y) + snakeDirection.y,
            0.0f
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food"))
        {
            Debug.Log("Collided with Food");
            Destroy(collision.gameObject);
        }
    }
}
