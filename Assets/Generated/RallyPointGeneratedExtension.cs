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

        public RallyPoint rallyPoint { get { return (RallyPoint)GetComponent(ComponentIds.RallyPoint); } }
        public bool hasRallyPoint { get { return HasComponent(ComponentIds.RallyPoint); } }

        public Entity AddRallyPoint(UnityEngine.Vector3 newPosition) {
            var component = CreateComponent<RallyPoint>(ComponentIds.RallyPoint);
            component.position = newPosition;
            return AddComponent(ComponentIds.RallyPoint, component);
        }

        public Entity ReplaceRallyPoint(UnityEngine.Vector3 newPosition) {
            var component = CreateComponent<RallyPoint>(ComponentIds.RallyPoint);
            component.position = newPosition;
            ReplaceComponent(ComponentIds.RallyPoint, component);
            return this;
        }

        public Entity RemoveRallyPoint() {
            return RemoveComponent(ComponentIds.RallyPoint);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherRallyPoint;

        public static IMatcher RallyPoint {
            get {
                if(_matcherRallyPoint == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.RallyPoint);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherRallyPoint = matcher;
                }

                return _matcherRallyPoint;
            }
        }
    }
}
