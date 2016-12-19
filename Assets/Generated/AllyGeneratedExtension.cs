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

        public Ally ally { get { return (Ally)GetComponent(ComponentIds.Ally); } }
        public bool hasAlly { get { return HasComponent(ComponentIds.Ally); } }

        public Entity AddAlly(string newCharId) {
            var component = CreateComponent<Ally>(ComponentIds.Ally);
            component.charId = newCharId;
            return AddComponent(ComponentIds.Ally, component);
        }

        public Entity ReplaceAlly(string newCharId) {
            var component = CreateComponent<Ally>(ComponentIds.Ally);
            component.charId = newCharId;
            ReplaceComponent(ComponentIds.Ally, component);
            return this;
        }

        public Entity RemoveAlly() {
            return RemoveComponent(ComponentIds.Ally);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherAlly;

        public static IMatcher Ally {
            get {
                if(_matcherAlly == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Ally);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherAlly = matcher;
                }

                return _matcherAlly;
            }
        }
    }
}