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
    public GhostNodeStatesEnum respawnState;

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

    public bool testRespawn = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodeStatesEnum.centerNode;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodeStatesEnum.leftNode;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodeStatesEnum.rightNode;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (testRespawn == true)
        {
            readyToLeaveHome = false;
            ghostNodeState = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }
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
            string direction = "";

            //we have reached our start node, move to the center node
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }
            //we have reached our ceneter node, either finish respawn, or move to the left/right node
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                else if (respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            //if our respawn sttae is either the left or right node, and we got to htat node, leave home again.
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodeState = respawnState;
            }
            //we are in the gameboard still, locate our start node
            else
            {
                //Determine quickest direction to home
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }

            
            movementController.SetDirection(direction);
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
