using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

namespace Experiment
{
    public class SceneObjectPlacer : MonoBehaviour
    {
        // Positions prédéfinies
        private static readonly Dictionary<string, Vector3> positionMap 
            = new() {
                { "R",  new Vector3(1, 0, 0)  },
                { "SR", new Vector3(2, 0, 0)  },
                { "L",  new Vector3(-1, 0, 0) },
                { "SL", new Vector3(-2, 0, 0) }
            };

        // Chemins de base des prefabs
        private static string basePath      = "Assets/Labsim/Experiment/LEXIKHUM-OAT/Prefabs/";
        private static string cuePath       = basePath + "Cue/";
        private static string strongCuePath = basePath + "StrongCue/";

        [MenuItem("Tools/Place Objects in Scenes")]
        public static void PlaceObjectsInScenes()
        {

            string[] scenePaths   = GetScenePaths();
            string[] patternCodes = GetPatternCodes();

            // Spécifiez les valeurs de position Z ici
            float departureZ      = 20f;
            float arrivalZ        = 150f;
            float firstObstacleZ  = 50f;
            float secondObstacleZ = 90f;
            float thirdObstacleZ  = 130f;

            for (int i = 0; i < scenePaths.Length; i++)
            {

                EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Single);
                RemoveAllObstacles();
                string patternCode = patternCodes[i];
                PlaceObjectsInScene(patternCode, firstObstacleZ, secondObstacleZ, thirdObstacleZ);
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

            } /* for() */

        } /* PlaceObjectsInScenes() */

        private static void RemoveAllObstacles()
        {
            // Supprimer tous les objets "Obstacle" dans la scène sauf "Departure", "Arrival" et "Pattern"
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name != "Departure" && obj.name != "Arrival" && obj.name != "Pattern" &&
                    (obj.name.Contains("Obstacle") || obj.transform.parent?.name == "Pattern"))
                {
                    GameObject.DestroyImmediate(obj);
                }
            }
        }

        private static string[] GetScenePaths()
        {
            // Remplacer par les chemins réels de vos scènes
            string basePath = "Assets/Labsim/Experiment/LEXIKHUM-OAT/Scenes/Scene_ID";
            string[] scenePaths = new string[48];
            for (int i = 1; i <= 48; i++)
            {
                scenePaths[i - 1] = basePath + i + ".unity";
            }
            return scenePaths;
        }

        private static string[] GetPatternCodes()
        {
            return new string[]
            {
                "SRRSL_AC_BV", "RSLL_AC_BV",   "LRL_AC_BV",   "SLLR_AC_BV", "RSLR_AC_BV",  "SRSLSR_AC_BV", "LLSR_AC_MV", "RSLSR_AC_MV",
                "RLSR_AC_MV",  "SRSRSL_AC_MV", "SLSLR_AC_MV", "RLSL_AC_MV", "SLRL_SC_BV",  "RSLSL_SC_BV",  "LSRR_SC_BV", "SLLSR_SC_BV",
                "RSRL_SC_BV",  "LSRSR_SC_BV",  "SRLSR_SC_MV", "LSLR_SC_MV", "SRSLR_SC_MV", "SRSLL_SC_MV",  "RLR_SC_MV",  "SRSLSL_SC_MV",
                "SLLSR_AC_BV", "LSRR_AC_BV",   "RLR_AC_BV",   "SRRL_AC_BV", "LSRL_AC_BV",  "SLSRSL_AC_BV", "RRSL_AC_MV", "LSRSL_AC_MV",
                "LRSL_AC_MV",  "SLSLSR_AC_MV", "SRSRL_AC_MV", "LRSR_AC_MV", "SRLR_SC_BV",  "LSRSR_SC_BV",  "RSLL_SC_BV", "SRRSL_SC_BV",
                "LSLR_SC_BV",  "RSLSL_SC_BV",  "SLRSL_SC_MV", "RSRL_SC_MV", "SLSRL_SC_MV", "SLSRR_SC_MV",  "LRL_SC_MV",  "SLSRSR_SC_MV"
            };
        }

        private static void PlaceObjectsInScene(string patternCode, float firstObstacleZ, float secondObstacleZ, float thirdObstacleZ)
        {
            // Trouver ou créer le GameObject "Pattern"
            GameObject patternParent = GameObject.Find("Pattern");
            if (patternParent == null)
            {
                patternParent = new GameObject("Pattern");
            }
            else
            {
                // Supprimer tous les enfants existants de "Pattern"
                foreach (Transform child in patternParent.transform)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }

            // Analyser le code de pattern
            string[] parts = patternCode.Split('_');
            string pattern = parts[0]; // ex: SRRSL

            // Définir les positions Z spécifiques
            float[] zPositions = { firstObstacleZ, secondObstacleZ, thirdObstacleZ };

            int zIndex = 0; // Index pour accéder aux positions Z

            // Créer et positionner les objets selon le pattern
            for (int i = 0; i < pattern.Length && zIndex < 3; i++) // S'assurer de ne placer que 3 obstacles
            {
                string posKey = pattern.Substring(i, 1); // Lire 1 caractère pour gérer R et L
                if (i + 1 < pattern.Length && (pattern.Substring(i, 2) == "SR" || pattern.Substring(i, 2) == "SL"))
                {
                    posKey = pattern.Substring(i, 2); // Lire 2 caractères pour gérer SR et SL
                    i++; // Incrémenter i correctement pour sauter le prochain caractère
                }

                if (positionMap.ContainsKey(posKey))
                {
                    Vector3 position = positionMap[posKey];
                    position.z = zPositions[zIndex]; // Assigner la position Z correcte
                    zIndex++;

                    // Déterminer quel prefab instancier
                    string prefabPath;
                    if (posKey == "R")
                    {
                        prefabPath = cuePath + "Obstacle_RS5.prefab";
                    }
                    else if (posKey == "L")
                    {
                        prefabPath = cuePath + "Obstacle_LS5.prefab";
                    }
                    else if (posKey == "SR")
                    {
                        prefabPath = strongCuePath + "Obstacle_SRS5.prefab";
                    }
                    else // posKey == "SL"
                    {
                        prefabPath = strongCuePath + "Obstacle_SLS5.prefab";
                    }

                    PlacePrefab(patternParent, prefabPath, position);
                }
                else
                {
                    Debug.LogError("Invalid position key: " + posKey);
                }
            }
        }

        private static void PlacePrefab(GameObject parent, string prefabPath, Vector3 position)
        {
            // Charger le prefab à partir du chemin spécifié
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                if (obj != null)
                {
                    obj.transform.localPosition = position;
                    Debug.Log("Prefab instantiated at position: " + position);
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab: " + prefabPath);
                }
            }
            else
            {
                Debug.LogError("Prefab not found at path: " + prefabPath);
            }
        }
    }
}
