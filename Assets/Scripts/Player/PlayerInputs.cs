using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActionAsset;
    private InputActionMap[] _actionMaps;
    private InputAction[] _gameplayActions;
    
    public InputAction[] GameplayActions => _gameplayActions;
    private void OnEnable() 
    {
        _playerInput = GetComponent<PlayerInput>();
        _inputActionAsset = _playerInput.actions;

        _actionMaps = _inputActionAsset.actionMaps.ToArray();

        _gameplayActions = _actionMaps[1].ToArray();
    }

    public void EnableUI()
    {
        _actionMaps[0].Enable();
        _actionMaps[1].Disable();
    }

    public void EnableGameplay()
    {
        foreach (InputAction action in _gameplayActions)
        {
            action.Enable();
        }

        _actionMaps[0].Disable();
        _actionMaps[1].Enable();
    }

    public void DisableGameplay()
    {
        foreach (InputAction action in _gameplayActions)
        {
            action.Disable();
        }

        _actionMaps[0].Disable();
        _actionMaps[1].Disable();
    }

}
