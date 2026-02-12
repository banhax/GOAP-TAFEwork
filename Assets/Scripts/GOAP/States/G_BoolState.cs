using UnityEngine;

namespace GOAP {
    [CreateAssetMenu(fileName = "New Bool State", menuName = "GOAP/States/Bool State")]
    public class G_BoolState : G_State {
        bool value;


        #region Basic Controls

        public override void Construct(string name, object value) {
            this.name = name;
            SetValue(value);
        }

        public override object GetValue() {
            return value;
        }

        public override void SetValue(object value) {
            if (value is bool) {
                this.value = (bool)value;   
            }
        }

        public override G_State Clone() {
            G_BoolState clone = CreateInstance<G_BoolState>();
            clone.Construct(this.name, this.value);
            return clone;
        }

        #endregion

        #region Testing Controls

        #endregion
    }
}
