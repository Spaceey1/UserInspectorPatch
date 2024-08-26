using FrooxEngine;
using FrooxEngine.ProtoFlux;
using FrooxEngine.UIX;
using HarmonyLib;
using MonkeyLoader.Patching;
using MonkeyLoader.Resonite;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyLoader.UserInspectorPatch
{
    [HarmonyPatchCategory(nameof(TestPatch))]
    [HarmonyPatch(typeof(UserInspector), nameof(UserInspector.OnAttach))]
    internal class TestPatch : ResoniteMonkey<TestPatch>
    {
        protected override IEnumerable<IFeaturePatch> GetFeaturePatches() => Enumerable.Empty<IFeaturePatch>();
        private static void Postfix(UserInspector __instance, SyncRef<Slot> ____userListContentRoot)
        {
            if(!__instance.World.IsAuthority)
            foreach (User user in __instance.World.AllUsers){
                __instance.RunSynchronously(delegate
                {
                    Slot slot = ____userListContentRoot.Target.AddSlot("User");
                    slot.PersistentSelf = false;
                    slot.AttachComponent<VerticalLayout>().PaddingTop.Value = 4f;
                    slot.AttachComponent<UserInspectorItem>().Setup(user);
                });
            }
        }
    }
}