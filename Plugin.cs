using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BlueEyes.Pause;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;
using UnityEngine.Assertions;

namespace JarodMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded UPDATED!");
        Harmony.CreateAndPatchAll(typeof(Plugin));
        ClassInjector.RegisterTypeInIl2Cpp<Test>();
        var assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/jarodbundle");
    }


    [HarmonyPatch(typeof(PauseController), "Pause")]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void GettinIn(PauseController __instance)
    {
        if (__instance.gameObject.GetComponent<Test>() != null) return;
        __instance.gameObject.AddComponent<Test>();
    }


}

public class Test : MonoBehaviour
{
    public void Start()
    {
        Plugin.Log.LogInfo("Test class is loaded!");

        var npc = GameObject.Find("NPC_Jarod");
        Assert.IsNotNull(npc, "Jarod not found!");


        // var gameObjectType = Il2CppType.Of<GameObject>();
        // var prefab = assetBundle.LoadAsset("Ears", gameObjectType) as GameObject;
        // prefab!.transform.position = npc.transform.position;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "LOOK MOM!");
        GUI.Label(new Rect(10, 30, 2000, 20), "IM INTO THE GAME!");
    }
}
