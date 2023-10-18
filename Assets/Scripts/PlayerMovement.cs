using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerGO;
    private GameObject transformerGO;

    private Vector3[] wayPositions;
    private int nextPositionIndex;
    private Vector3 nextPosition;
    private bool isStarted;

    [SerializeField] float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");

        transformerGO = GameObject.Find("Transformer");
        transformerGO.SetActive(false);

        var lr = GameObject.Find("PlayerWay").GetComponent<LineRenderer>();
        wayPositions = new Vector3[lr.positionCount];
        lr.GetPositions(wayPositions);

        nextPositionIndex = 0;
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            // if currently hiding
            if (IsHiding())
            {
                Show();
            }

            // reached end of road
            if (nextPositionIndex == wayPositions.Length)
            {
                return;
            }

            // next corner
            if (nextPosition == Vector3.zero)
            {
                nextPosition = wayPositions[nextPositionIndex];
                nextPosition.y = playerGO.transform.position.y;
            }

            playerGO.transform.position = Vector3.MoveTowards(playerGO.transform.position, nextPosition, moveSpeed * Time.deltaTime);
            var distance = Vector3.Distance(playerGO.transform.position, nextPosition);
            if (distance < 0.1f)
            {
                nextPositionIndex++;
                nextPosition = Vector3.zero;
            }

            transformerGO.transform.position = playerGO.transform.position;

            if (!isStarted)
            {
                isStarted = true;
            }
        }
        else if (Input.touchCount == 0)
        {
            if (isStarted && !IsHiding())
            {
                Hide();
            }
        }
    }

    void Hide()
    {
        transformerGO.SetActive(true);
        playerGO.SetActive(false);
    }

    void Show()
    {
        transformerGO.SetActive(false);
        playerGO.SetActive(true);
    }

    bool IsHiding()
    {
        return transformerGO.activeSelf;
    }
}
