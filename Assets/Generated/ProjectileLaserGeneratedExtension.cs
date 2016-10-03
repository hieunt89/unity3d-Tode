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
        public ProjectileLaser projectileLaser { get { return (ProjectileLaser)GetComponent(ComponentIds.ProjectileLaser); } }

        public bool hasProjectileLaser { get { return HasComponent(ComponentIds.ProjectileLaser); } }

        public Entity AddProjectileLaser(float newTravelSpeed, float newDuration, float newTickInterval, float newMaxDmgBuildTime) {
            var component = CreateComponent<ProjectileLaser>(ComponentIds.ProjectileLaser);
            component.travelSpeed = newTravelSpeed;
            component.duration = newDuration;
            component.tickInterval = newTickInterval;
            component.maxDmgBuildTime = newMaxDmgBuildTime;
            return AddComponent(ComponentIds.ProjectileLaser, component);
        }

        public Entity ReplaceProjectileLaser(float newTravelSpeed, float newDuration, float newTickInterval, float newMaxDmgBuildTime) {
            var component = CreateComponent<ProjectileLaser>(ComponentIds.ProjectileLaser);
            component.travelSpeed = newTravelSpeed;
            component.duration = newDuration;
            component.tickInterval = newTickInterval;
            component.maxDmgBuildTime = newMaxDmgBuildTime;
            ReplaceComponent(ComponentIds.ProjectileLaser, component);
            return this;
        }

        public Entity RemoveProjectileLaser() {
            return RemoveComponent(ComponentIds.ProjectileLaser);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherProjectileLaser;

        public static IMatcher ProjectileLaser {
            get {
                if (_matcherProjectileLaser == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.ProjectileLaser);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherProjectileLaser = matcher;
                }

                return _matcherProjectileLaser;
            }
        }
    }
}