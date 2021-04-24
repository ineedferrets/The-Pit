using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ColorSyncController : RealtimeComponent<ColorSyncModel_Carlos>
{
    #region Variables

    private MeshRenderer _meshRenderer;
    private Transform _transform;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        // Get ref
        _meshRenderer = GetComponent<MeshRenderer>();
        _transform = this.transform;
    }

    #endregion

    #region Normal Messages

    protected override void OnRealtimeModelReplaced(ColorSyncModel_Carlos previousModel, ColorSyncModel_Carlos currentModel)
    {
        /* If this RealtimeComponent was previously mapped to a different model (e.g. when switching rooms), 
         * it will provide a reference to the previous model in order to allow your component 
         * to unregister from events. */
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.colorDidChange -= ColorDidChange;
            previousModel.positionDidChange -= PositionDidChange;
            previousModel.rotationDidChange -= RotationDidChange;
            previousModel.scaleDidChange -= ScaleDidChange;

        }

        /* When a RealtimeComponent is first created, it starts with no model. Normcore populates it 
         * once we have successfully connected to the server (or instantly if we're already connected), 
         * and calls OnRealtimeModelReplaced() to provide us with a copy of it */
        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color
            if (currentModel.isFreshModel)
            {
                currentModel.color = _meshRenderer.material.color;
                currentModel.position = _transform.position;
                currentModel.rotation = _transform.rotation;
                currentModel.scale = _transform.localScale;
            }

            // Update the mesh renderer to match the new model
            UpdateMeshRendererColor();

            // Update transform
            UpdatePosition();
            UpdateRotation();
            UpdateScale();

            // Register for events so we will know if the color changes later
            currentModel.colorDidChange += ColorDidChange;
            currentModel.positionDidChange += PositionDidChange;
            currentModel.rotationDidChange += RotationDidChange;
            currentModel.scaleDidChange += ScaleDidChange;

        }

    }

    #endregion

    #region Private Methods

    private void ColorDidChange(ColorSyncModel_Carlos model, Color value)
    {
        // Update the mesh renderer
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor()
    {
        // Get the color from the model and set it on the mesh renderer.
        _meshRenderer.material.color = model.color;
    }

    private void PositionDidChange(ColorSyncModel_Carlos model, Vector3 value)
    {
        // Update transform
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Get the transform from the model and set it on our transform
        _transform.position = model.position;

    }

    private void RotationDidChange(ColorSyncModel_Carlos model, Quaternion value)
    {
        // Update transform
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        // Get the transform from the model and set it on our transform
        _transform.rotation = model.rotation;

    }

    private void ScaleDidChange(ColorSyncModel_Carlos model, Vector3 value)
    {
        // Update transform
        UpdateScale();
    }

    private void UpdateScale()
    {
        // Get the transform from the model and set it on our transform
        _transform.localScale = model.scale;

    }


    #endregion

    #region Public Methods

    public void SetColor(Color color)
    {
        // Set the color on the model
        // this will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players
        model.color = color;
    }

    public void SetPosition(Vector3 newPos)
    {
        model.position = newPos;
    }

    public void SetRotation(Quaternion newRot)
    {
        model.rotation = newRot;
    }

    public void SetScale(Vector3 newScale)
    {
        model.scale = newScale;
    }

    #endregion

}
