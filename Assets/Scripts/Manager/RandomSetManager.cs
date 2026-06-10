using System.Collections.Generic;
using UnityEngine;

public class RandomSetManager : MonoBehaviour
{
    private void Awake()
    {
        LevelSet[] allSets = FindObjectsByType<LevelSet>(FindObjectsSortMode.None);

        Dictionary<SetCategory, List<LevelSet>> groups = new();

        // Agrupar sets
        foreach (var set in allSets)
        {
            if (!groups.ContainsKey(set.category))
                groups[set.category] = new List<LevelSet>();

            groups[set.category].Add(set);
        }

        // IMPORTANTE: desactivar todo primero
        foreach (var set in allSets)
        {
            set.gameObject.SetActive(false);
        }

        // Activar 1 por categoría
        foreach (var group in groups)
        {
            List<LevelSet> sets = group.Value;

            int randomIndex = Random.Range(0, sets.Count);
            sets[randomIndex].gameObject.SetActive(true);
        }
    }
}