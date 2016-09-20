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
        public ProjectileThrowingTime projectileThrowingTime { get { return (ProjectileThrowingTime)GetComponent(ComponentIds.ProjectileThrowingTime); } }

        public bool hasProjectileThrowingTime { get { return HasComponent(ComponentIds.ProjectileThrowingTime); } }

        public Entity AddProjectileThrowingTime(float newTime) {
            var component = CreateComponent<ProjectileThrowingTime>(ComponentIds.ProjectileThrowingTime);
            component.time = newTime;
            return AddComponent(ComponentIds.ProjectileThrowingTime, component);
        }

        public Entity ReplaceProjectileThrowingTime(float newTime) {
            var component = CreateComponent<ProjectileThrowingTime>(ComponentIds.ProjectileThrowingTime);
            component.time = newTime;
            ReplaceComponent(ComponentIds.ProjectileThrowingTime, component);
            return this;
        }

        public Entity RemoveProjectileThrowingTime() {
            return RemoveComponent(ComponentIds.ProjectileThrowingTime);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherProjectileThrowingTime;

        public static IMatcher ProjectileThrowingTime {
            get {
                if (_matcherProjectileThrowingTime == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.ProjectileThrowingTime);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherProjectileThrowingTime = matcher;
                }

                return _matcherProjectileThrowingTime;
            }
        }
    }
}
