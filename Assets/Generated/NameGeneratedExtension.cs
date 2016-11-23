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

        public Name name { get { return (Name)GetComponent(ComponentIds.Name); } }
        public bool hasName { get { return HasComponent(ComponentIds.Name); } }

        public Entity AddName(string newValue) {
            var component = CreateComponent<Name>(ComponentIds.Name);
            component.value = newValue;
            return AddComponent(ComponentIds.Name, component);
        }

        public Entity ReplaceName(string newValue) {
            var component = CreateComponent<Name>(ComponentIds.Name);
            component.value = newValue;
            ReplaceComponent(ComponentIds.Name, component);
            return this;
        }

        public Entity RemoveName() {
            return RemoveComponent(ComponentIds.Name);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherName;

        public static IMatcher Name {
            get {
                if(_matcherName == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Name);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherName = matcher;
                }

                return _matcherName;
            }
        }
    }
}
