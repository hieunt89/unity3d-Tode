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
        static readonly NextWave nextWaveComponent = new NextWave();

        public bool isNextWave {
            get { return HasComponent(ComponentIds.NextWave); }
            set {
                if (value != isNextWave) {
                    if (value) {
                        AddComponent(ComponentIds.NextWave, nextWaveComponent);
                    } else {
                        RemoveComponent(ComponentIds.NextWave);
                    }
                }
            }
        }

        public Entity IsNextWave(bool value) {
            isNextWave = value;
            return this;
        }
    }

    public partial class Matcher {
        static IMatcher _matcherNextWave;

        public static IMatcher NextWave {
            get {
                if (_matcherNextWave == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.NextWave);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherNextWave = matcher;
                }

                return _matcherNextWave;
            }
        }
    }
}