using UnityEngine;

public class GridDebug : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private GridCellCoord _cellViewPrefab;
    [SerializeField] private Color _color;

    private readonly GridCellCoord[,] _cells = new GridCellCoord[11, 20];

    private void Awake()
    {
        UpdateView();

        for (int column = 0; column <= _cells.GetLength(0); column++)
        {
            Debug.DrawLine(
                _grid.CellToWorld(new(column, 0)),
                _grid.CellToWorld(new(column, _cells.GetLength(1))),
                _color, 1000f);
        }

        for (int row = 0; row <= _cells.GetLength(1); row++)
        {
            Debug.DrawLine(
                _grid.CellToWorld(new(0, row)),
                _grid.CellToWorld(new(_cells.GetLength(0), row)),
                _color, 1000f);
        }
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
