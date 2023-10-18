using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class NpcMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3[] wayPositions;
    private int nextPositionIndex;
    private Vector3 nextPosition;
    private bool direction;

    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] LineRenderer moveWay;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        wayPositions = new Vector3[moveWay.positionCount];
        moveWay.GetPositions(wayPositions);

        direction = true;
        nextPositionIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // next corner
        if (nextPosition == Vector3.zero)
        {
            nextPosition = wayPositions[nextPositionIndex];
            nextPosition.y = rb.position.y;
        }

        rb.position = Vector3.MoveTowards(rb.position, nextPosition, moveSpeed * Time.deltaTime);
        var distance = Vector3.Distance(rb.position, nextPosition);
        if (distance < 0.1f)
        {
            // reached end of road -> turn back
            if (direction)
            {
                nextPositionIndex++;
                if (nextPositionIndex == wayPositions.Length)
                {
                    direction = false;
                    nextPositionIndex = wayPositions.Length - 1;
                }
            }
            else
            {
                nextPositionIndex--;
                if (nextPositionIndex == -1)
                {
                    direction = true;
                    nextPositionIndex = 0;
                }
            }


            nextPosition = Vector3.zero;
        }
    }
}
