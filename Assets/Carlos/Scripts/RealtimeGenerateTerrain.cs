using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RealtimeGenerateTerrain : RealtimeComponent<GenerateTerrainModel>
{
    #region Variables

    /// <summary>
    /// The terrain generator
    /// </summary>
    private GenerateTerrain _terrainGenerator;

    // Used to check ownership
    private RealtimeView _realtimeView;



    #endregion
    #region Unity Messages

    private void Awake()
    {
        // Get ref
        _terrainGenerator = GetComponent<GenerateTerrain>();
        _realtimeView = GetComponent<RealtimeView>();

    }

    private void Update()
    {
        if (_realtimeView != null)
        {
            // If this Player prefab is not owned by this client, bail.
            if (_realtimeView.realtime != null && !_realtimeView.isOwnedLocallySelf)
                return;
        }



    }

    #endregion

    #region Normal Messages

    protected override void OnRealtimeModelReplaced(GenerateTerrainModel previousModel, GenerateTerrainModel currentModel)
    {
        /* If this RealtimeComponent was previously mapped to a different model (e.g. when switching rooms), 
         * it will provide a reference to the previous model in order to allow your component 
         * to unregister from events. */
        if (previousModel != null)
        {
            // Unregister from events
            currentModel.generationStartedDidChange -= GenerationStartedDidChange;
            currentModel.generationCompletedDidChange -= GenerationCompletedDidChange;

        }

        /* When a RealtimeComponent is first created, it starts with no model. Normcore populates it 
         * once we have successfully connected to the server (or instantly if we're already connected), 
         * and calls OnRealtimeModelReplaced() to provide us with a copy of it */
        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current value
            if (currentModel.isFreshModel)
            {
                currentModel.generationStarted = _terrainGenerator.generationStarted;
                currentModel.generationCompleted = _terrainGenerator.generationCompleted;
            }

            // Update the terraing generator to match the new model
            UpdateGenerationStarted();
            UpdateGenerationCompleted();


            // Register for events so we will know if the value changes later
            currentModel.generationStartedDidChange += GenerationStartedDidChange;
            currentModel.generationCompletedDidChange += GenerationCompletedDidChange;

        }

    }

    #endregion

    #region Private Methods

    private void GenerationStartedDidChange(GenerateTerrainModel model, bool value)
    {
        // Update the mesh renderer
        UpdateGenerationStarted();
    }

    private void UpdateGenerationStarted()
    {
        _terrainGenerator.generationStarted = model.generationStarted;
    }

    private void GenerationCompletedDidChange(GenerateTerrainModel model, bool value)
    {
        // Update the mesh renderer
        UpdateGenerationCompleted();
    }

    private void UpdateGenerationCompleted()
    {
        _terrainGenerator.generationCompleted = model.generationCompleted;
    }


    #endregion

    #region Public Methods

    public bool GetGenerationStarted()
    {
        return model.generationStarted;
    }
    
    public bool GetGenerationCompleted()
    {
        return model.generationCompleted;
    }

    public void SetGenerationStarted(bool value)
    {
        // Set the value on the model
        // this will fire the valueChanged event on the model, which will update the value for both the local player and all remote players
        model.generationStarted = value;
    }

    public void SetGenerationCompleted(bool value)
    {
        // Set the value on the model
        // this will fire the valueChanged event on the model, which will update the value for both the local player and all remote players
        model.generationCompleted = value;
    }


    #endregion

}
