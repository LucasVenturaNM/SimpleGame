using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGame.Playground
{
    public class PlaygroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _groundBlock;
        [SerializeField] private GameObject _player;
        private GameObject _playerInstance;
        

        private Vector3 _playerPosition;
        private LevelSO _levelData;

        [Header("Playground Size")]

        private List<GameObject> _groundBlocks = new List<GameObject>();

        public List<GameObject> GroundBlocks => _groundBlocks;
        private void OnEnable() 
        {
            LevelManager.onLevelStateChanged += UpdatePlayground;
        }

        private void OnDisable()
        {
            LevelManager.onLevelStateChanged -= UpdatePlayground;
        }

        

        public void UpdatePlayground(LevelState state)
        {
            switch (state)
            {
                case LevelState.Start:

                    _levelData = LevelManager.Instance.CurrentLevelData;

                    CenterPlayground();

                    CreatePlayground();

                    SpawnPlayer();

                    break;


                case LevelState.NextLevel:

                    foreach (GameObject groundBlock in _groundBlocks)
                    {
                        Destroy(groundBlock);
                    }

                    if(_playerInstance != null) Destroy(_playerInstance);

                    _groundBlocks.Clear();

                    break;

            }
        }

        private void CenterPlayground()
        {
            Vector3 furthestPoint = (Vector3.right * (_levelData.InitialNumberOfRows-1) + Vector3.forward * (_levelData.InitialNumberOfColumns-1)) * _levelData.MarginBetweenCells;
            transform.position = -furthestPoint /2;
        }

        private void CreatePlayground()
        {
            for (int row = 0; row < _levelData.InitialNumberOfRows; row++)
            {
                for (int col = 0; col < _levelData.InitialNumberOfColumns; col++)
                {
                    GameObject newGroundBlock = Instantiate(_groundBlock, transform);

                    newGroundBlock.transform.position += (Vector3.right * row + Vector3.forward * col) * _levelData.MarginBetweenCells;

                    newGroundBlock.name = "Ground cell (" + row + ", " + col + ")";

                    _groundBlocks.Add(newGroundBlock);
                }

            }
        }

        private void SpawnPlayer()
        {
            if(_playerInstance == null) _playerInstance = Instantiate(_player, transform);

            if (_levelData.InitialNumberOfRows % 2 == 0 || _levelData.InitialNumberOfColumns % 2 == 0)
            {
                _playerPosition = new Vector3(_groundBlocks[0].transform.position.x,
                                                transform.position.y,
                                                _groundBlocks[0].transform.position.z);
            }
            
            _playerInstance.transform.position = _playerPosition;

            
        }

    }
}
