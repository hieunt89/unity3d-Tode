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

        public EffectMovement effectMovement { get { return (EffectMovement)GetComponent(ComponentIds.EffectMovement); } }
        public bool hasEffectMovement { get { return HasComponent(ComponentIds.EffectMovement); } }

        public Entity AddEffectMovement(SkillEffect newEf) {
            var component = CreateComponent<EffectMovement>(ComponentIds.EffectMovement);
            component.ef = newEf;
            return AddComponent(ComponentIds.EffectMovement, component);
        }

        public Entity ReplaceEffectMovement(SkillEffect newEf) {
            var component = CreateComponent<EffectMovement>(ComponentIds.EffectMovement);
            component.ef = newEf;
            ReplaceComponent(ComponentIds.EffectMovement, component);
            return this;
        }

        public Entity RemoveEffectMovement() {
            return RemoveComponent(ComponentIds.EffectMovement);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherEffectMovement;

        public static IMatcher EffectMovement {
            get {
                if(_matcherEffectMovement == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.EffectMovement);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherEffectMovement = matcher;
                }

                return _matcherEffectMovement;
            }
        }
    }
}