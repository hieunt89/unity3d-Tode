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
        public SkillWatcherList skillWatcherList { get { return (SkillWatcherList)GetComponent(ComponentIds.SkillWatcherList); } }

        public bool hasSkillWatcherList { get { return HasComponent(ComponentIds.SkillWatcherList); } }

        public Entity AddSkillWatcherList(System.Collections.Generic.List<Entitas.Entity> newWatchers) {
            var component = CreateComponent<SkillWatcherList>(ComponentIds.SkillWatcherList);
            component.watchers = newWatchers;
            return AddComponent(ComponentIds.SkillWatcherList, component);
        }

        public Entity ReplaceSkillWatcherList(System.Collections.Generic.List<Entitas.Entity> newWatchers) {
            var component = CreateComponent<SkillWatcherList>(ComponentIds.SkillWatcherList);
            component.watchers = newWatchers;
            ReplaceComponent(ComponentIds.SkillWatcherList, component);
            return this;
        }

        public Entity RemoveSkillWatcherList() {
            return RemoveComponent(ComponentIds.SkillWatcherList);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherSkillWatcherList;

        public static IMatcher SkillWatcherList {
            get {
                if (_matcherSkillWatcherList == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.SkillWatcherList);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherSkillWatcherList = matcher;
                }

                return _matcherSkillWatcherList;
            }
        }
    }
}
