using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Field : MonoBehaviour
{
    public static Field Instance;

    public int FieldWidth;
    public int FieldHeight;
    public int MinesCount;
    //public const int TILE_SIZE = 32;

    [HideInInspector] public bool isGenerated = false;
    [HideInInspector] public UnityEvent OnFieldGenerated = new UnityEvent();

    public Dictionary<Vector2Int, Cell> Cells = new();
    public GameObject CellPrefab;
    public Sprite HiddenTexture;

    private Vector2Int mouseDownCellPosition;
    private byte[,] map;

    private void Awake() => Instance = this;
    
    void Update()
    {
        if (!GameManager.instance.isGameActive)
            return;

        if (GameManager.instance.isPaused)
            return;

        ProcessInput();
    }

    public void StartGame()
    {
        InitializePrefabs();
    }

    private void InitializePrefabs()
    {
        for (int y = 0; y < FieldHeight; y++)
        {
            for (int x = 0; x < FieldWidth; x++)
            {
                GameObject cell = Instantiate(CellPrefab, new Vector3(x, -y), Quaternion.identity, transform);

                Cell cellScript = cell.GetComponent<Cell>();
                cellScript.ParentField = this;
                cellScript.Position = new Vector2Int(x, y);

                // подписываем на ивенты
                cellScript.OnCellOpened.AddListener(GameCounter.Instance.CellOpened);
                cellScript.OnCellFlagged.AddListener(GameCounter.Instance.CellFlagged);

                Cells.Add(new Vector2Int(x, y), cellScript);
            }
        }
    }

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int mouseCellPosition = FromWorldToCell(mouseScreenPosition);
            mouseDownCellPosition = mouseCellPosition;
            if (IsInBounds(mouseCellPosition))
            {
                Cells[mouseCellPosition].OnLeftMouseDown();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int mouseCellPosition = FromWorldToCell(mouseScreenPosition);

            // ≈сли изначально нажата€ клетка не совпадает с текущей, то скип
            if (mouseCellPosition != mouseDownCellPosition && IsInBounds(mouseDownCellPosition))
            {
                Cell cell = Cells[mouseDownCellPosition];
                if (cell.CurrentState == Cell.States.BeingPressed)
                    cell.CurrentState = Cell.States.Hidden;
                SetSpriteToPressedAround(mouseDownCellPosition, false);

                return;
            }
            
            if (IsInBounds(mouseCellPosition))
            {
                if (!isGenerated)
                    Regenerate(mouseCellPosition);
                Cells[mouseCellPosition].Dig();
            }

            mouseDownCellPosition = Vector2Int.zero;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int mouseCellPosition = FromWorldToCell(mouseScreenPosition);
            if (IsInBounds(mouseCellPosition))
            {
                Cells[mouseCellPosition].OnRightMouseUp();
            }
        }
    }

    public void Regenerate(Vector2Int InitialPosition)
    {
        map = FieldGenerator.GenerateMap(FieldWidth, FieldHeight, MinesCount, InitialPosition);
        OnFieldGenerated.Invoke();
        isGenerated = true;
    }

    public void ResetField()
    {
        // Recreating cells
        ClearField();
        InitializePrefabs();

        isGenerated = false;
    }

    public void ClearField()
    {
        for (int y = 0; y < FieldHeight; y++)
        {
            for (int x = 0; x < FieldWidth; x++)
            {
                Cell cell = Cells[new Vector2Int(x, y)];
                Destroy(cell.gameObject);
            }
        }
        Cells.Clear();
    }

    public byte GetMinesAround(Vector2Int position)
    {
        byte mines = 0;
        foreach (var dir in Vector2IntExtensions.GetDirections())
        {
            if (!IsInBounds(position + dir))
                continue;

            mines += map[position.y + dir.y, position.x + dir.x];
        }

        return mines;
    }

    public byte GetFlagsAround(Vector2Int position)
    {
        byte flags = 0;
        foreach (var dir in Vector2IntExtensions.GetDirections())
        {
            if (!IsInBounds(position + dir))
                continue;

            flags += Convert.ToByte(Cells[position + dir].CurrentState == Cell.States.Flagged);
        }

        return flags;
    }

    public void DigAround(Vector2Int position)
    {
        foreach (var dir in Vector2IntExtensions.GetDirections())
        {
            if (!IsInBounds(position + dir))
                continue;

            Cells[position + dir].Dig(true);
        }
    }

    public void SetSpriteToPressedAround(Vector2Int position, bool isPressed)
    {
        foreach (var dir in Vector2IntExtensions.GetDirections())
        {
            if (!IsInBounds(position + dir))
                continue;

            Cell cell = Cells[position + dir];
            if (cell.CurrentState == Cell.States.Hidden && isPressed)
                cell.CurrentState = Cell.States.BeingPressed;
            if (cell.CurrentState == Cell.States.BeingPressed && !isPressed)
                cell.CurrentState = Cell.States.Hidden;
        }
    }

    public Vector2Int FromWorldToCell(Vector2 position)
    {
        return Vector2Int.FloorToInt(position * new Vector2(1, -1) - (Vector2)transform.position * new Vector2(1, -1));
    }

    public bool IsMine(Vector2Int position)
    {
        return map[position.y, position.x] != 0;
    }

    public bool IsInBounds(Vector2Int position)
    {
        return position.x < FieldWidth && position.x >= 0 && position.y < FieldHeight && position.y >= 0;
    }
}
