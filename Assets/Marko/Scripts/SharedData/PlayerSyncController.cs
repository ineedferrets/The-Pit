using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerSyncController : RealtimeComponent<PlayerSyncModel>
{

    //set by PlayerScript_Marko.cs
    public MeshRenderer _meshRenderer = new MeshRenderer();


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

    public void SetPlayerColor(Color c)
    {
        model.color = c;
    }

}
