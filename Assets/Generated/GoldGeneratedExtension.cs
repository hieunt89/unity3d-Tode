//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Entitas {
    public partial class Entity {
        public Gold gold { get { return (Gold)GetComponent(ComponentIds.Gold); } }

        public bool hasGold { get { return HasComponent(ComponentIds.Gold); } }

        public Entity AddGold(int newValue) {
            var component = CreateComponent<Gold>(ComponentIds.Gold);
            component.value = newValue;
            return AddComponent(ComponentIds.Gold, component);
        }

        public Entity ReplaceGold(int newValue) {
            var component = CreateComponent<Gold>(ComponentIds.Gold);
            component.value = newValue;
            ReplaceComponent(ComponentIds.Gold, component);
            return this;
        }

        public Entity RemoveGold() {
            return RemoveComponent(ComponentIds.Gold);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherGold;

        public static IMatcher Gold {
            get {
                if (_matcherGold == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Gold);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherGold = matcher;
                }

                return _matcherGold;
            }
        }
    }
}