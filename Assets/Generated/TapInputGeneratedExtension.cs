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
        public TapInput tapInput { get { return (TapInput)GetComponent(ComponentIds.TapInput); } }

        public bool hasTapInput { get { return HasComponent(ComponentIds.TapInput); } }

        public Entity AddTapInput(string newId) {
            var component = CreateComponent<TapInput>(ComponentIds.TapInput);
            component.id = newId;
            return AddComponent(ComponentIds.TapInput, component);
        }

        public Entity ReplaceTapInput(string newId) {
            var component = CreateComponent<TapInput>(ComponentIds.TapInput);
            component.id = newId;
            ReplaceComponent(ComponentIds.TapInput, component);
            return this;
        }

        public Entity RemoveTapInput() {
            return RemoveComponent(ComponentIds.TapInput);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherTapInput;

        public static IMatcher TapInput {
            get {
                if (_matcherTapInput == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.TapInput);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherTapInput = matcher;
                }

                return _matcherTapInput;
            }
        }
    }
}