using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGame.Playground
{
    public class PlaygroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _groundBlock;
        [SerializeField] private LevelSO _levelData;

        [Header("Playground Size")]

        private List<GameObject> _groundBlocks = new List<GameObject>();

        public List<GameObject> GroundBlocks => _groundBlocks;
        public LevelSO GetLevelData => _levelData;
        private void Awake()
        {
            CenterPlayground();
            CreatePlayground();
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

        public void UpdatePlayground()
        {
            foreach (GameObject groundBlock in _groundBlocks)
            {
                Destroy(groundBlock);
            }

            _groundBlocks.Clear();

            CreatePlayground();
        }
    }
}
