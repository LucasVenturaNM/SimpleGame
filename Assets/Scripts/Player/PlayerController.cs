using System;
using System.Collections;
using System.Collections.Generic;
using SimpleGame.Playground;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlaygroundGenerator _playgroundGenerator;
    public static event Action PlayerLoss;
    private LevelSO _levelData;
    private int _currentBlock;

    private PlayerInputs _playerInputs;

    #region Unity
    private void Start()
    {
       
        _playerInputs = GetComponent<PlayerInputs>();
        _playerInputs.EnableGameplay();
        _playerInputs.GameplayActions[0].performed += Move;

        _levelData = _playgroundGenerator.GetLevelData;

        if(_levelData.InitialNumberOfRows % 2 == 0 || _levelData.InitialNumberOfColumns % 2 == 0)
        {
            transform.position = new Vector3(_playgroundGenerator.GroundBlocks[0].transform.position.x,transform.position.y,_playgroundGenerator.GroundBlocks[0].transform.position.z);
        }
       
    }

    private void Update() {
        Debug.DrawRay(transform.position,Vector3.down * 5);
         RaycastHit hit;
        Physics.Raycast(transform.position,Vector3.down * 5, out hit, 3f, LayerMask.GetMask("Ground"));

        if(hit.collider.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            PlayerLoss?.Invoke();
            RespawnPlayer();
        }

    }

    private void OnDisable() 
    {
        _playerInputs.DisableGameplay();
    }
    #endregion

    private void RespawnPlayer()
    {
        
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 inputVector = ctx.ReadValue<Vector2>();

        int nextBlock = CheckForMovement(inputVector);
        if(nextBlock >= 0 && nextBlock < _playgroundGenerator.GroundBlocks.Count)
        {
            GameObject nextGroundBlock = _playgroundGenerator.GroundBlocks[nextBlock];
            transform.position = new Vector3(nextGroundBlock.transform.position.x, 0, nextGroundBlock.transform.position.z);
        }
        
    }

    private int CheckForMovement(Vector2 direction)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,Vector3.down * 5, out hit, 3f, LayerMask.GetMask("Ground"));


        for (int i = 0; i < _playgroundGenerator.GroundBlocks.Count; i++)
        {
            if (hit.collider.gameObject == _playgroundGenerator.GroundBlocks[i])
            {
                _currentBlock = i;
            }
        }

        if(direction.y > 0)
        {
            int nextIndex = _currentBlock + 1;
            int remainder = nextIndex % _levelData.InitialNumberOfColumns;

           

            if( nextIndex < _playgroundGenerator.GroundBlocks.Count && remainder != 0)
            {
                return nextIndex;
            }
        }
        else if(direction.y < 0)
        {
            int nextIndex = _currentBlock - 1;
            int remainder = _currentBlock % _levelData.InitialNumberOfColumns;

            if(nextIndex >= 0 && remainder != 0)
            {
                return nextIndex;
            }
        }
        
        if(direction.x > 0)
        {
            int nextIndex = _currentBlock + _levelData.InitialNumberOfColumns;

            if(nextIndex < _playgroundGenerator.GroundBlocks.Count)
            {
                return nextIndex;
            }
        }
        else if(direction.x < 0)
        {
            int nextIndex = _currentBlock - _levelData.InitialNumberOfColumns;

            if(nextIndex >= 0)
            {
                return nextIndex;
            }
        }

        return 100000;
    }
}
