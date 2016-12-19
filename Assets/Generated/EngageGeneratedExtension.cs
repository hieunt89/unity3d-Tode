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

        public Engage engage { get { return (Engage)GetComponent(ComponentIds.Engage); } }
        public bool hasEngage { get { return HasComponent(ComponentIds.Engage); } }

        public Entity AddEngage(Entitas.Entity newTarget) {
            var component = CreateComponent<Engage>(ComponentIds.Engage);
            component.target = newTarget;
            return AddComponent(ComponentIds.Engage, component);
        }

        public Entity ReplaceEngage(Entitas.Entity newTarget) {
            var component = CreateComponent<Engage>(ComponentIds.Engage);
            component.target = newTarget;
            ReplaceComponent(ComponentIds.Engage, component);
            return this;
        }

        public Entity RemoveEngage() {
            return RemoveComponent(ComponentIds.Engage);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherEngage;

        public static IMatcher Engage {
            get {
                if(_matcherEngage == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Engage);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherEngage = matcher;
                }

                return _matcherEngage;
            }
        }
    }
}