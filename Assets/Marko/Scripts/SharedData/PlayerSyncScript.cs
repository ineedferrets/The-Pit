using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerSyncScript : RealtimeComponent<PlayerSyncModel>
{

    private MeshRenderer _meshRenderer;
    private Transform transform;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void OnRealtimeModelReplaced(PlayerSyncModel previousModel, PlayerSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.colorDidChange -= ColorDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.color = _meshRenderer.material.color;
            }

            UpdateMeshRendererColor();

            currentModel.colorDidChange += ColorDidChange;
        }
    }

    private void ColorDidChange(PlayerSyncModel model, Color value)
    {
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor()
    {
        _meshRenderer.material.color = model.color;
    }

}
