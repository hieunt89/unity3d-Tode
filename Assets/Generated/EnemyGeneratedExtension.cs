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
        public Enemy enemy { get { return (Enemy)GetComponent(ComponentIds.Enemy); } }

        public bool hasEnemy { get { return HasComponent(ComponentIds.Enemy); } }

        public Entity AddEnemy(EnemyType newType) {
            var component = CreateComponent<Enemy>(ComponentIds.Enemy);
            component.type = newType;
            return AddComponent(ComponentIds.Enemy, component);
        }

        public Entity ReplaceEnemy(EnemyType newType) {
            var component = CreateComponent<Enemy>(ComponentIds.Enemy);
            component.type = newType;
            ReplaceComponent(ComponentIds.Enemy, component);
            return this;
        }

        public Entity RemoveEnemy() {
            return RemoveComponent(ComponentIds.Enemy);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherEnemy;

        public static IMatcher Enemy {
            get {
                if (_matcherEnemy == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Enemy);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherEnemy = matcher;
                }

                return _matcherEnemy;
            }
        }
    }
}
