using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    private bool isMoving;
    private Vector2 input;

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                var targetPosition = transform.position;
                if (input.x != 0) targetPosition.x += Mathf.Sign(input.x);
                if (input.y != 0) targetPosition.y += Mathf.Sign(input.y);

                if (IsWalkable(targetPosition))
                    StartCoroutine(Move(targetPosition));
            }
        }
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.Linecast(transform.position, targetPosition, solidObjectsLayer).collider == null;
    }


    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
    }
}
