using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueTracker : MonoBehaviour {
    public GameObject trackerEntryPrefab;
    List<TMP_Text> currentEntries = new List<TMP_Text>();

    public void Track(List<string> values) {
        for (int i = 0; i < values.Count; i++) {

            TMP_Text foundEntry = currentEntries.Find((text) => text.text.Split(" ")[0] == values[i].Split(" ")[0]);
            if (foundEntry != null) {
                foundEntry.text = values[i];
            }
            else {
                TMP_Text newEntry = Instantiate(trackerEntryPrefab, transform)
                    .transform
                    .GetChild(0)
                    .GetComponent<TMP_Text>();
                newEntry.text = values[i];
                currentEntries.Add(newEntry);
            }
        }
    }
}
