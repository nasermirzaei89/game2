using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OpponentController : MonoBehaviour
{
    private Transform target;
    public float movementSpeed;
    public float maxRange, minRange;
    public LayerMask solidObjectsLayer;

    private bool subscribed;
    private UnityEvent targetAct;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        targetAct = target.GetComponent<PlayerController>().act;
    }

    // Update is called once per frame
    void Update()
    {

            if (InSight(target.position))
            {
                if (!subscribed)
                {
                    subscribed = true;
                    targetAct.AddListener(FollowPlayer);
                }
            }
            else
            {
                if (subscribed)
                {
                    targetAct.RemoveListener(FollowPlayer);
                    subscribed = false;
                }
            }

    }

    private bool InSight(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) <= maxRange && Vector3.Distance(targetPosition, transform.position) > minRange;
    }

    private void FollowPlayer()
    {
        var distance = target.position - transform.position;

        var nextPosition = transform.position;

        if (Mathf.Abs(distance.x) >= Mathf.Abs(distance.y))
        {
            nextPosition.x += Mathf.Sign(distance.x);
            if (!IsWalkable(nextPosition))
            {
                nextPosition.x -= Mathf.Sign(distance.x);
                nextPosition.y += Mathf.Sign(distance.y);
            }
        } else
        {
            nextPosition.y += Mathf.Sign(distance.y);
        }

        if (IsWalkable(nextPosition))
        {
            StartCoroutine(Move(nextPosition));
        }
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.Linecast(transform.position, targetPosition, solidObjectsLayer).collider == null;
    }


    IEnumerator Move(Vector3 targetPosition)
    {


        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;


    }
}
