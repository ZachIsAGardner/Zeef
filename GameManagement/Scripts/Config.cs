namespace Zeef.GameManagement {
    public enum FacingsEnum {
        Up,
        Down,
        Left,
        Right
    }

    public enum PromptsEnum {
		Interact,
		Speach,
		Exit,
	}

    public enum EntitiesEnum {
        Default,
        Jack,
        Pebbles,
        Zoe,
        BurglarKid,
        Enemy
    }

	public enum IdentifiersEnum {
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

    public enum ContainersEnum {
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