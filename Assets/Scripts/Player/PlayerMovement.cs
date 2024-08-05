using System.Collections;
using System.Collections.Generic;
using SimpleGame.Playground;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int CheckForMovement(Vector2 direction, PlaygroundGenerator playground, LevelSO levelData)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,Vector3.down * 5, out hit, 3f, LayerMask.GetMask("Ground"));
        if(hit.collider == null) return 0;

        int currentCell = 0;

        
        for (int i = 0; i < playground.GroundBlocks.Count; i++)
        {
            if (hit.collider.gameObject == playground.GroundBlocks[i])
            {
                currentCell = i;
            }
        }

        int nextIndex = currentCell + 1;
        int nextRemainder = nextIndex % levelData.InitialNumberOfColumns;

        int previousIndex = currentCell - 1;
        int previousRemainder = currentCell % levelData.InitialNumberOfColumns;

        int nextTopIndex = currentCell + levelData.InitialNumberOfColumns;

        int previousTopIndex = currentCell - levelData.InitialNumberOfColumns;

        //Vertical Direction
        if(direction.y > 0)
        {
            if( nextIndex < playground.GroundBlocks.Count && nextRemainder != 0)
            {
                return nextIndex;
            }
        }
        else if(direction.y < 0)
        {
            if(previousIndex >= 0 && previousRemainder != 0)
            {
                return previousIndex;
            }
        }
        
        //Horizontal Direction
        if(direction.x > 0)
        {
            if(nextTopIndex < playground.GroundBlocks.Count)
            {
                return nextTopIndex;
            }
        }
        else if(direction.x < 0)
        {
            

            if(previousTopIndex >= 0)
            {
                return previousTopIndex;
            }
        }

        return 100000;
    }
}
