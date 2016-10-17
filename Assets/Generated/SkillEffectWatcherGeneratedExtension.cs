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
        public SkillEffectWatcher skillEffectWatcher { get { return (SkillEffectWatcher)GetComponent(ComponentIds.SkillEffectWatcher); } }

        public bool hasSkillEffectWatcher { get { return HasComponent(ComponentIds.SkillEffectWatcher); } }

        public Entity AddSkillEffectWatcher(Entitas.Entity newTarget, SkillEffect newEffect) {
            var component = CreateComponent<SkillEffectWatcher>(ComponentIds.SkillEffectWatcher);
            component.target = newTarget;
            component.effect = newEffect;
            return AddComponent(ComponentIds.SkillEffectWatcher, component);
        }

        public Entity ReplaceSkillEffectWatcher(Entitas.Entity newTarget, SkillEffect newEffect) {
            var component = CreateComponent<SkillEffectWatcher>(ComponentIds.SkillEffectWatcher);
            component.target = newTarget;
            component.effect = newEffect;
            ReplaceComponent(ComponentIds.SkillEffectWatcher, component);
            return this;
        }

        public Entity RemoveSkillEffectWatcher() {
            return RemoveComponent(ComponentIds.SkillEffectWatcher);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherSkillEffectWatcher;

        public static IMatcher SkillEffectWatcher {
            get {
                if (_matcherSkillEffectWatcher == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.SkillEffectWatcher);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherSkillEffectWatcher = matcher;
                }

                return _matcherSkillEffectWatcher;
            }
        }
    }
}