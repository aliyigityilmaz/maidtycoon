using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystemCs : MonoBehaviour
{
    [SerializeField] private float previewOffSet = 0.056f;

    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;
    // Start is called before the first frame update
    void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }
    private void Update()
    {
        if (previewObject != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("rotated");
                RotateThePreview();
            }
        }
    }
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if(previewObject != null ) 
            Destroy(previewObject);
        Destroy(previewObject);
    }
    public void UpdatePosition(Vector3 position, bool validity)
    {   
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
            
        }
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewOffSet, position.z);
    }
    //private void RotatePreview()
    //{
    //    foreach (Transform Child in previewObject.GetComponentInChildren<Transform>())
    //    {
    //        Child.transform.localRotation = Quaternion.Euler(Child.transform.localEulerAngles.x, Child.transform.localEulerAngles.y + 90, Child.transform.localEulerAngles.z);
    //    }
        
    //}
    private void RotateThePreview()
    {
        previewObject.transform.Rotate(new Vector3(0, 90, 0));
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }

}
