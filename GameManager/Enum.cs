namespace Zeef.GameManager {
    public enum FacingID {
        Up,
        Down,
        Left,
        Right
    }

    public enum PromptID {
		Interact,
		Speach,
		Exit,
	}

    public enum EntityID {
        Someone,
        Jack,
        Pebbles,
        Zoe,
        BurglarKid,
        Enemy
    }

	public enum IdentifierID {
        Default,
        Player,
        Enemy,
        Hurt,
        Hit,
        Spawn,
        EnemySpawn,
        SceneSpawn,
        Bounds,
        GameCanvas,
        Fight,
        FightCanvas
    }

    public enum ContainerID {
		Default,
		Game,
		Map,
		Overworld,
		Performance,
		Background,
		Ground,
		Foreground,
		Decor,
		Entities,
		Triggers,
		Colliders,
		Spawns,
		PerformanceElements,
		PlayerFighters,
		EnemyFighters
	}
}