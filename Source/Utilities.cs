using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TechTree {
    internal static class Utilities {
        public static string ProjectsToString(List<ResearchProjectDef> researchProjects) {
            if (researchProjects == null || researchProjects.Count == 0) {
                return string.Empty;
            }
            return string.Join(", ", researchProjects.Select(project => project.label));
        }
    }
}
