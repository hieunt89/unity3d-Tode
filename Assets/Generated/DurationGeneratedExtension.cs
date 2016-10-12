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
        public Duration duration { get { return (Duration)GetComponent(ComponentIds.Duration); } }

        public bool hasDuration { get { return HasComponent(ComponentIds.Duration); } }

        public Entity AddDuration(float newValue) {
            var component = CreateComponent<Duration>(ComponentIds.Duration);
            component.value = newValue;
            return AddComponent(ComponentIds.Duration, component);
        }

        public Entity ReplaceDuration(float newValue) {
            var component = CreateComponent<Duration>(ComponentIds.Duration);
            component.value = newValue;
            ReplaceComponent(ComponentIds.Duration, component);
            return this;
        }

        public Entity RemoveDuration() {
            return RemoveComponent(ComponentIds.Duration);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherDuration;

        public static IMatcher Duration {
            get {
                if (_matcherDuration == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Duration);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherDuration = matcher;
                }

                return _matcherDuration;
            }
        }
    }
}