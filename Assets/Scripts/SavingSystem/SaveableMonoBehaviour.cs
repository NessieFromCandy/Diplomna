using System;
using UnityEditor;
using UnityEngine;

public abstract class SaveableMonoBehaviour : MonoBehaviour, ISaveable, ISerializationCallbackReceiver
{
    public string saveIdentifier => this._saveIdentifier;

    [SerializeField, HideInInspector] private string _saveIdentifier;

    public abstract object CaptureState();

    public abstract void RestoreState(object state);

    public virtual void OnBeforeSerialize()
    {
        if (!string.IsNullOrEmpty(this._saveIdentifier)) return;

        // don't set identifier for non prefab instances
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;

        this._saveIdentifier = Guid.NewGuid().ToString();

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public virtual void OnAfterDeserialize()
    {
    }
}