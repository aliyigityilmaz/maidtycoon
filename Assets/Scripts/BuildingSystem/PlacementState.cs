using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystemCs previewSystem;
    ObjectsDatabase dataBase;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    private Vector3 rotation = Vector3.zero; // Rotasyon verisi

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystemCs previewSystem,
                          ObjectsDatabase dataBase,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.dataBase = dataBase;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = this.dataBase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(this.dataBase.objectsData[selectedObjectIndex].Prefab, this.dataBase.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(dataBase.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));
        objectPlacer.SetRotation(rotation); // Rotasyonu ayarla

        GridData selectedData = dataBase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition,
            dataBase.objectsData[selectedObjectIndex].Size,
            dataBase.objectsData[selectedObjectIndex].ID,
            index,rotation);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = dataBase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, dataBase.objectsData[selectedObjectIndex].Size);
    }
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    public void SetRotation(Vector3 newRotation)
    {
        rotation = newRotation;
    }
}
