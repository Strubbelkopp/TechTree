using RimWorld;
using System.Collections.Generic;
using Verse;

namespace TechTree {
    [StaticConstructorOnStartup]
    public static class TechTree {
        static TechTree() {
            List<ResearchProjectDef> researchProjects = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
            researchProjects.ForEach(researchProjectDef => {
                TechLevel techLevel = researchProjectDef.techLevel;
                ResearchTabDef newTab = techLevel switch {
                    TechLevel.Animal => ResearchTabDefOf.Animal,
                    TechLevel.Neolithic => ResearchTabDefOf.Neolithic,
                    TechLevel.Medieval => ResearchTabDefOf.Medieval,
                    TechLevel.Industrial => ResearchTabDefOf.Industrial,
                    TechLevel.Spacer => ResearchTabDefOf.Spacer,
                    TechLevel.Ultra => ResearchTabDefOf.Ultra,
                    TechLevel.Archotech => ResearchTabDefOf.Archotech,
                    _ => ResearchTabDefOf.Undefined
                };
                researchProjectDef.tab = newTab;
            });

            //List<ResearchTabDef> populatedTabs = [];
            //DefDatabase<ResearchProjectDef>.AllDefsListForReading.ForEach(researchProjectDef => {
            //    ResearchTabDef tab = researchProjectDef.tab;
            //    if (!populatedTabs.Contains(tab)) {
            //        populatedTabs.Add(tab);
            //    }
            //});
        }
    }
}
