using UnityEngine;

public class GridDebug : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private GridCellCoord _cellViewPrefab;
    [SerializeField] private Color _color;

    private readonly GridCellCoord[,] _cells = new GridCellCoord[11, 20];

    private void Awake()
    {
        //UpdateView();
    }

    private void UpdateView()
    {
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                Vector3 localPosition = _grid.CellToLocal(new(i, j)) +
                    new Vector3(_grid.cellSize.x, -0.001f, _grid.cellSize.y) / 2f;

                GridCellCoord cell = _cells[i, j] == null ? Instantiate(_cellViewPrefab, transform) : _cells[i, j];
                cell.Init(i, j, _color, localPosition);
            }
        }
    }
}
