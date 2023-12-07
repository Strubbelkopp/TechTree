using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace TechTree {
    [StaticConstructorOnStartup]
    public static class TechTree {
        private static Dictionary<ResearchTabDef, List<ResearchProjectDef>> projectsByTabs;

        static TechTree() {
            projectsByTabs = ModifyResearchTabs();

            foreach (var tab in projectsByTabs) {
                ResearchGraph graph = new(tab.Value);
                graph.Print(tab.Key.defName);
            }

            var harmony = new Harmony("dev.strubbelkopp.tech_tree");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);
        }

        public static Dictionary<ResearchTabDef, List<ResearchProjectDef>> ModifyResearchTabs() {
            Dictionary<ResearchTabDef, List<ResearchProjectDef>> projectsByTabs = [];

            DefDatabase<ResearchProjectDef>.AllDefsListForReading.ForEach(researchProject => {
                TechLevel techLevel = researchProject.techLevel;
                ResearchTabDef newTab = techLevel switch {
                    TechLevel.Animal => ModResearchTabDefOf.Animal,
                    TechLevel.Neolithic => ModResearchTabDefOf.Neolithic,
                    TechLevel.Medieval => ModResearchTabDefOf.Medieval,
                    TechLevel.Industrial => ModResearchTabDefOf.Industrial,
                    TechLevel.Spacer => ModResearchTabDefOf.Spacer,
                    TechLevel.Ultra => ModResearchTabDefOf.Ultra,
                    TechLevel.Archotech => ModResearchTabDefOf.Archotech,
                    _ => ModResearchTabDefOf.Undefined
                };
                researchProject.tab = newTab;

                if (projectsByTabs.TryGetValue(newTab, out List<ResearchProjectDef> researchProjects)) {
                    researchProjects.Add(researchProject);
                    projectsByTabs[newTab] = researchProjects;
                } else {
                    projectsByTabs.Add(newTab, [researchProject]);
                }
            });

            return projectsByTabs;
        }

        public static List<ResearchTabDef> GetPopulatedResearchTabs() {
            return new List<ResearchTabDef>(projectsByTabs.Keys);
        }
    }
}
