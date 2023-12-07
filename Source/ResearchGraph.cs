using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TechTree {
    public class ResearchGraph {
        public Dictionary<int, List<ResearchProjectDef>> graph = [];
        public Dictionary<ResearchProjectDef, List<ResearchProjectDef>> queue = [];

        public ResearchGraph(List<ResearchProjectDef> researchProjects) {
            researchProjects?.ForEach(researchProject => {
                List<ResearchProjectDef> prerequisites = researchProject.prerequisites?
                    .Where(prerequisite => prerequisite.tab == researchProject.tab)
                    .ToList();
                List<ResearchProjectDef> dependents = DefDatabase<ResearchProjectDef>.AllDefsListForReading
                    .Where(def => def.prerequisites != null && def.prerequisites.Contains(researchProject))
                    .Where(dependent => dependent.tab == researchProject.tab)
                    .ToList();

                if (prerequisites == null || prerequisites.Count == 0) {
                    AddNode(researchProject, 0); 
                } else {
                    int layer = prerequisites.Select(prerequisite => GetLayer(prerequisite)).Max();
                    if (layer == -1) {
                        queue.Add(researchProject, dependents);
                    } else {
                        AddNode(researchProject, layer + 1);
                    }
                }
            });
            ResolveQueue();
        }

        private void AddNode(ResearchProjectDef researchProject, int layer) {
            if (researchProject != null && layer >= 0) {
                if (graph.TryGetValue(layer, out List<ResearchProjectDef> researchProjects)) {
                    researchProjects.Add(researchProject);
                    graph[layer] = researchProjects;
                } else {
                    graph.Add(layer, [researchProject]);
                }
            }
        }

        private int GetLayer(ResearchProjectDef project) {
            if (project != null) {
                foreach (var layer in graph) {
                    List<ResearchProjectDef> projects = layer.Value;
                    if (projects.Contains(project)) {
                        return layer.Key;
                    }
                }
            }
            return -1;
        }

        private void ResolveQueue() {
            List<ResearchProjectDef> researchProjects = [];
            foreach (var researchProject in queue) {
                researchProjects.Add(researchProject.Key);
            }
            if (researchProjects.Count > 0) {
                Log.Warning($"Queue: [{Utilities.ProjectsToString(researchProjects)}]");
            }
        }

        public void Print(string tabName) {
            Log.Message($"{tabName}:");
            foreach (var layer in graph) {
                Log.Message($"\t{layer.Key}: [{Utilities.ProjectsToString(layer.Value)}]");
            }
        }
    }
}
