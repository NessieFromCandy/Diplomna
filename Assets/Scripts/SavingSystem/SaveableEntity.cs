using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    public Dictionary<string, object> CaptureState(params string[] saveableIds)
    {
        var state = new Dictionary<string, object>();
        var saveables = this.GetComponents<ISaveable>();

        foreach (var saveable in saveables)
        {
            if (saveableIds?.Length > 0 && !saveableIds.Contains(saveable.saveIdentifier)) continue;

            state[saveable.saveIdentifier] = saveable.CaptureState();
        }

        return state;
    }

    public void RestoreState(Dictionary<string, object> stateDict)
    {
        var saveables = this.GetComponents<ISaveable>();

        foreach (var saveable in saveables)
        {
            if (stateDict.ContainsKey(saveable.saveIdentifier))
            {
                saveable.RestoreState(stateDict[saveable.saveIdentifier]);
            }
        }
    }
}