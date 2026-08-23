using BepInEx;
using UnityEngine;

namespace SolidMonkeysReborn;
[BepInPlugin(Constants.Name, Constants.Guid, Constants.Version)]
public class PluginBepInEx : BaseUnityPlugin
{
    private void Start()
    {
        GameObject obj = new GameObject(Constants.Guid);
        obj.AddComponent<Main>();
        DontDestroyOnLoad(obj);
    }
}