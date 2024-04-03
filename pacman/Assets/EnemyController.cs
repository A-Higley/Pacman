using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public MovementController movementController;

    public GameObject startingNode;

    public bool readyToLeaveHome = false;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStatesEnum.startNode;
            startingNode = ghostNodeStart;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;

        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;

        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;

        }
        movementController.currentNode = startingNode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStatesEnum.movingInNodes)
        {
            //determine next game node to go to
            if (ghostType == GhostType.red)
            {
                DetermineRedGhostDirection();
            }
        }
        else if (ghostNodeState == GhostNodeStatesEnum.respawning)
        {
            //Determine quickest direction to home
        }
        else
        {
            //if we are ready to leave our home
            if (readyToLeaveHome)
            {
                //if we are in the left home node, move to the center
                if (ghostNodeState == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                //if we are in the right home node, move to the fcenter
                else if (ghostNodeState == GhostNodeStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                //if we are in the center node, move to the start node
                else if (ghostNodeState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                //if we are in the start node, start moving around in the game
                else if (ghostNodeState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {

    }

    void DetermineBlueGhostDirection()
    {

    }

    void DetermineOrangeGhostDirection()
    {

    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        //if we can move up and we aren't reversing 
        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            //get the node above us
            GameObject nodeUp = nodeController.nodeUp;
            //get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            //if this is the shortest distance so far, set our direction
            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";

            }
        }
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            //get the node above us
            GameObject nodeDown = nodeController.nodeDown;
            //get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            //if this is the shortest distance so far, set our direction
            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";

            }
        }
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            //get the node above us
            GameObject nodeLeft = nodeController.nodeLeft;
            //get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            //if this is the shortest distance so far, set our direction
            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";

            }
        }
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            //get the node above us
            GameObject nodeRight = nodeController.nodeRight;
            //get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            //if this is the shortest distance so far, set our direction
            if(distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";

            }
        }

        return newDirection;
    }

}
