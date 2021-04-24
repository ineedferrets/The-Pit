using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerSyncScript : RealtimeComponent<PlayerSyncModel>
{
    public MeshRenderer MeshRenderer;


    protected void Awake()
    {
     
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
                currentModel.color = MeshRenderer.material.color;
            }
        }

        UpdateMeshRendererColor();

        currentModel.colorDidChange += ColorDidChange;
    }

    private void ColorDidChange(PlayerSyncModel model, Color value)
    {
        UpdateMeshRendererColor();
    }

    public void UpdateMeshRendererColor()
    {
        MeshRenderer.material.color = model.color;
    }

    public void SetColor(Color color)
    {
        model.color = color;
    }


}
