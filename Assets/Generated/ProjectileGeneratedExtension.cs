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
        public Projectile projectile { get { return (Projectile)GetComponent(ComponentIds.Projectile); } }

        public bool hasProjectile { get { return HasComponent(ComponentIds.Projectile); } }

        public Entity AddProjectile(string newProjectileId) {
            var component = CreateComponent<Projectile>(ComponentIds.Projectile);
            component.projectileId = newProjectileId;
            return AddComponent(ComponentIds.Projectile, component);
        }

        public Entity ReplaceProjectile(string newProjectileId) {
            var component = CreateComponent<Projectile>(ComponentIds.Projectile);
            component.projectileId = newProjectileId;
            ReplaceComponent(ComponentIds.Projectile, component);
            return this;
        }

        public Entity RemoveProjectile() {
            return RemoveComponent(ComponentIds.Projectile);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherProjectile;

        public static IMatcher Projectile {
            get {
                if (_matcherProjectile == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Projectile);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherProjectile = matcher;
                }

                return _matcherProjectile;
            }
        }
    }
}
