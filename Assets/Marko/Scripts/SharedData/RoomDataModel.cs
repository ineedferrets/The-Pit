using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class RoomDataModel
{
    [RealtimeProperty(1, true, true)]
    private int _numberOfPlayers;

    [RealtimeProperty(2, true, true)]
    private string _playerNames;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class RoomDataModel : RealtimeModel {
    public int numberOfPlayers {
        get {
            return _cache.LookForValueInCache(_numberOfPlayers, entry => entry.numberOfPlayersSet, entry => entry.numberOfPlayers);
        }
        set {
            if (this.numberOfPlayers == value) return;
            _cache.UpdateLocalCache(entry => { entry.numberOfPlayersSet = true; entry.numberOfPlayers = value; return entry; });
            InvalidateReliableLength();
            FireNumberOfPlayersDidChange(value);
        }
    }
    
    public string playerNames {
        get {
            return _cache.LookForValueInCache(_playerNames, entry => entry.playerNamesSet, entry => entry.playerNames);
        }
        set {
            if (this.playerNames == value) return;
            _cache.UpdateLocalCache(entry => { entry.playerNamesSet = true; entry.playerNames = value; return entry; });
            InvalidateReliableLength();
            FirePlayerNamesDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(RoomDataModel model, T value);
    public event PropertyChangedHandler<int> numberOfPlayersDidChange;
    public event PropertyChangedHandler<string> playerNamesDidChange;
    
    private struct LocalCacheEntry {
        public bool numberOfPlayersSet;
        public int numberOfPlayers;
        public bool playerNamesSet;
        public string playerNames;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        NumberOfPlayers = 1,
        PlayerNames = 2,
    }
    
    public RoomDataModel() : this(null) {
    }
    
    public RoomDataModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireNumberOfPlayersDidChange(int value) {
        try {
            numberOfPlayersDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FirePlayerNamesDidChange(string value) {
        try {
            playerNamesDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteVarint32Length((uint)PropertyID.NumberOfPlayers, (uint)_numberOfPlayers);
            length += WriteStream.WriteStringLength((uint)PropertyID.PlayerNames, _playerNames);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.numberOfPlayersSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.NumberOfPlayers, (uint)entry.numberOfPlayers);
            }
            if (entry.playerNamesSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.PlayerNames, entry.playerNames);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteVarint32((uint)PropertyID.NumberOfPlayers, (uint)_numberOfPlayers);
            stream.WriteString((uint)PropertyID.PlayerNames, _playerNames);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.numberOfPlayersSet || entry.playerNamesSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.numberOfPlayersSet) {
                stream.WriteVarint32((uint)PropertyID.NumberOfPlayers, (uint)entry.numberOfPlayers);
                didWriteProperties = true;
            }
            if (entry.playerNamesSet) {
                stream.WriteString((uint)PropertyID.PlayerNames, entry.playerNames);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.NumberOfPlayers: {
                    int previousValue = _numberOfPlayers;
                    _numberOfPlayers = (int)stream.ReadVarint32();
                    bool numberOfPlayersExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.numberOfPlayersSet);
                    if (!numberOfPlayersExistsInChangeCache && _numberOfPlayers != previousValue) {
                        FireNumberOfPlayersDidChange(_numberOfPlayers);
                    }
                    break;
                }
                case (uint)PropertyID.PlayerNames: {
                    string previousValue = _playerNames;
                    _playerNames = stream.ReadString();
                    bool playerNamesExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.playerNamesSet);
                    if (!playerNamesExistsInChangeCache && _playerNames != previousValue) {
                        FirePlayerNamesDidChange(_playerNames);
                    }
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
        }
    }
    
    #region Cache Operations
    
    private StreamEventDispatcher _streamEventDispatcher;
    
    private void FlattenCache() {
        _numberOfPlayers = numberOfPlayers;
        _playerNames = playerNames;
        _cache.Clear();
    }
    
    private void ClearCache(uint updateID) {
        _cache.RemoveUpdateFromInflight(updateID);
    }
    
    private void ClearCacheOnStreamCallback(StreamContext context) {
        if (_streamEventDispatcher != context.dispatcher) {
            UnsubscribeClearCacheCallback(); // unsub from previous dispatcher
        }
        _streamEventDispatcher = context.dispatcher;
        _streamEventDispatcher.AddStreamCallback(context.updateID, ClearCache);
    }
    
    private void UnsubscribeClearCacheCallback() {
        if (_streamEventDispatcher != null) {
            _streamEventDispatcher.RemoveStreamCallback(ClearCache);
            _streamEventDispatcher = null;
        }
    }
    
    #endregion
}
/* ----- End Normal Autogenerated Code ----- */
