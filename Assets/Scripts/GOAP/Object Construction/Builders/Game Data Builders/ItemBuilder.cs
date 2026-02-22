using UnityEngine;

namespace GOAP {
    public class ItemBuilder {
        #region Basic Values
        // any values to be transferred into the build object
        string name = "";
        bool stackable = true;

        public ItemBuilder(string name) {
            this.name = name;
        }
        #endregion

        #region With Functions
        public ItemBuilder isStackable(bool stackable) {
            this.stackable = stackable;
            return this;
        }

        //public ItemBuilder WithName(string name) {
        //    this.name = name;
        //    return this;
        //}
        #endregion

        #region Object Creation

        public Item Build() {
            Item item = ScriptableObject.CreateInstance<Item>();
            item.stackable = stackable;
            return item;
        }

        public static implicit operator Item(ItemBuilder builder) {
            return builder.Build();
        }

        #endregion
    }
}
