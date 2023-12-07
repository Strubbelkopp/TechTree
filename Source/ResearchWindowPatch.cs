using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace TechTree {
    internal static class ResearchWindowPatch {

        [HarmonyPatch(typeof(MainTabWindow_Research), nameof(MainTabWindow_Research.PostOpen))]
        class HideEmptyTabs() {
            private static List<ResearchTabDef> populatedResearchTabs = TechTree.GetPopulatedResearchTabs();

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                populatedResearchTabs.ForEach(tab => {
                    Log.Warning(tab.defName);
                });

                var codes = instructions.ToList();
                var targetMethod = AccessTools.PropertyGetter(typeof(DefDatabase<ResearchTabDef>), nameof(DefDatabase<ResearchTabDef>.AllDefs));

                int i = codes.FindIndex(instruction => instruction.Calls(targetMethod));
                codes[i].opcode = OpCodes.Ldsfld;
                codes[i].operand = AccessTools.Field(typeof(HideEmptyTabs), nameof(populatedResearchTabs));

                return codes;
            }
        }
    }
}
