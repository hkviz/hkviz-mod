using UnityEngine;

namespace HKViz.Silk.Recording;

public class HeroLocationWriter(RunFiles runFiles) {
    private const int FPS = 10;
    private HeroController? hero;

    private float deltaPerFrame = 1f / FPS;
    private float lastWriteTime = 0f;
    
    public void Update() {
        if (!hero) {
            hero = HeroController.SilentInstance;
        }
        if (!hero) return;
        
        var deltaSinceLastWrite = Time.unscaledTime - lastWriteTime;
        if (deltaSinceLastWrite >= deltaPerFrame) {
            var pos = hero.transform.position;
            runFiles.WriteHeroLocation(pos);
            lastWriteTime = Time.unscaledTime;
        }
    }
}