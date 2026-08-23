using Photon.Pun;
using SolidMonkeysReborn.Classes;
using SolidMonkeysReborn.Patches;
using SolidMonkeysReborn.Utilities;
using UnityEngine;

namespace SolidMonkeysReborn;

public class Main : MonoBehaviour
{
    public static Main? Instance;
    public GorillaLog Log = new();
    
    private void Start()
    {
        Instance = this;
        HarmonyPatches.Patch(); 
        Config.Load();
        Application.quitting += Config.Save; 
        GorillaTagger.OnPlayerSpawned(() => MethodUtilities.Attempt(OnPlayerSpawned));
    }
    
    private void OnPlayerSpawned()
    {
        RoomSystem.PlayerJoinedEvent += OnPlayerJoined;
        NetworkSystem.Instance.OnMultiplayerStarted += OnJoin;
        Log.WriteLine($"Hello world!");
    }

    private void OnJoin()
    {
        if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
        {
            foreach (VRRig coolrigidk in VRRigCache.ActiveRigs)
            {
                if (coolrigidk != VRRig.LocalRig)
                {
                    EnableCollisions(coolrigidk);
                }
            }
        }
    }
        
    private void EnableCollisions(VRRig rig)
    {
        var collider = rig.GetComponent<SphereCollider>();
        collider.isTrigger = false;
        collider.enabled = true;
        rig.gameObject.layer = LayerMask.NameToLayer("Default");
    }
    
    private void OnPlayerJoined(NetPlayer player)
    {  // i know im checking the gamemode string each time in a function so if you can please help me improve it
        if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
        {
            if (player == NetworkSystem.Instance.LocalPlayer) return;
            var NewPlayerRig = GorillaGameManager.instance.FindPlayerVRRig(player);
            EnableCollisions(NewPlayerRig);
        }
    } 
    
    // uh if you can please make a pull request and improve the code i dunno bye
    
}