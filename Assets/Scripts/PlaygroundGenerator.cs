using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGame.Playground
{
    public class PlaygroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _groundBlock;

        [Header("Playground Size")]
        [SerializeField][Range(2, 6)] private int _rowNumber;
        [SerializeField][Range(2, 6)] private int _colNumber;
        [SerializeField] private float _margin;

        private void Start()
        {
            CenterPlayground();
            CreatePlayGround();
        }

        private void CenterPlayground()
        {
            Vector3 furthestPoint = (Vector3.right * (_rowNumber-1) + Vector3.forward * (_colNumber-1)) * _margin;
            transform.position = -furthestPoint /2;
        }

        private void CreatePlayGround()
        {
            for (int row = 0; row < _rowNumber; row++)
            {
                for (int col = 0; col < _colNumber; col++)
                {
                    GameObject newGroundBlock = Instantiate(_groundBlock, transform);

                    newGroundBlock.transform.position += (Vector3.right * row + Vector3.forward * col) * _margin;
                    
                }

            }
        }
    }
}
