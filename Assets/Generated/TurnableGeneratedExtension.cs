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
        public Turnable turnable { get { return (Turnable)GetComponent(ComponentIds.Turnable); } }

        public bool hasTurnable { get { return HasComponent(ComponentIds.Turnable); } }

        public Entity AddTurnable(float newTurnSpeed) {
            var component = CreateComponent<Turnable>(ComponentIds.Turnable);
            component.turnSpeed = newTurnSpeed;
            return AddComponent(ComponentIds.Turnable, component);
        }

        public Entity ReplaceTurnable(float newTurnSpeed) {
            var component = CreateComponent<Turnable>(ComponentIds.Turnable);
            component.turnSpeed = newTurnSpeed;
            ReplaceComponent(ComponentIds.Turnable, component);
            return this;
        }

        public Entity RemoveTurnable() {
            return RemoveComponent(ComponentIds.Turnable);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherTurnable;

        public static IMatcher Turnable {
            get {
                if (_matcherTurnable == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Turnable);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherTurnable = matcher;
                }

                return _matcherTurnable;
            }
        }
    }
}
