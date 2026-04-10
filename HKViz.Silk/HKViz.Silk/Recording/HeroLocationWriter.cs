using UnityEngine;

namespace HKViz.Silk.Recording;

public class HeroLocationWriter(RunFiles runFiles) {
    private const int FPS = 10;

    private float deltaPerFrame = 1f / FPS;
    private float lastWriteTime = 0f;
    
    public void Update() {
        var hero = HeroController._instance;
        if (!hero) return;
        
        var deltaSinceLastWrite = Time.unscaledTime - lastWriteTime;
        if (deltaSinceLastWrite >= deltaPerFrame) {
            var pos = hero.transform.position;
            runFiles.WriteHeroLocation(pos);
            lastWriteTime = Time.unscaledTime;
        }
    }
}
