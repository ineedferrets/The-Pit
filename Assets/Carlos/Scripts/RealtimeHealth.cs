using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RealtimeHealth : RealtimeComponent<BlockHealthSyncModel>
{
    #region Variables

    private Block _block;

    // Used to check ownership
    private RealtimeView _realtimeView;

    // Used to check changes in health
    private int _previousHealth = default;


    #endregion
    #region Unity Messages

    private void Awake()
    {
        // Get ref
        _block = GetComponent<Block>();
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

        // If the value has changed, call SetHealth 
        if (_block.health != _previousHealth)
        {
            SetHealth(_block.health);
            _previousHealth = _block.health;
        }


    }

    #endregion

    #region Normal Messages

    protected override void OnRealtimeModelReplaced(BlockHealthSyncModel previousModel, BlockHealthSyncModel currentModel)
    {
        /* If this RealtimeComponent was previously mapped to a different model (e.g. when switching rooms), 
         * it will provide a reference to the previous model in order to allow your component 
         * to unregister from events. */
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.healthDidChange -= HealthDidChange;

        }

        /* When a RealtimeComponent is first created, it starts with no model. Normcore populates it 
         * once we have successfully connected to the server (or instantly if we're already connected), 
         * and calls OnRealtimeModelReplaced() to provide us with a copy of it */
        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current value
            if (currentModel.isFreshModel)
            {
                currentModel.health = _block.health;
            }

            // Update the mesh renderer to match the new model
            UpdateBlockHealth();


            // Register for events so we will know if the value changes later
            currentModel.healthDidChange += HealthDidChange;

        }

    }

    #endregion

    #region Private Methods

    private void HealthDidChange(BlockHealthSyncModel model, int value)
    {
        // Update the mesh renderer
        UpdateBlockHealth();
    }

    private void UpdateBlockHealth()
    {
        _block.health = model.health;
    }


    #endregion

    #region Public Methods

    public void SetHealth(int health)
    {
        // Set the value on the model
        // this will fire the valueChanged event on the model, which will update the value for both the local player and all remote players
        model.health = health;

    }


    #endregion


}
