using UnityEngine;

namespace TetrisHack
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Figure _figurePrefab;

        private void SpawnFigure()
        {
            var newFigure = CreateFigure(1);

        }

        private Figure CreateFigure(int blocksCount = 1)
        {
            var newFigure = Instantiate(_figurePrefab);
            return newFigure;
        }
    }
}