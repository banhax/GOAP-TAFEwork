using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "World State", menuName = "GOAP/World State")]
    public class G_WorldState : ScriptableObject {
        public List<G_State> states = new List<G_State>();
        public List<G_Action> actionPool = new List<G_Action>();
        public List<G_Goal> goals = new List<G_Goal>();
    }

}
