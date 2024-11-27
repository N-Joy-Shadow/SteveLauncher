namespace SteveLauncher.Domain.Entity;

public class MinecraftGame {
    
}

public class MinecraftGameSetting {
    public const string GameDirectoryName = "instances";
    public int Width { get; set; }
    public int Height { get; set; }
    public string MinecraftPath { get; set; }    
    public int AllocatedMemory { get; set; }

    public static MinecraftGameSetting InitialSetting() {
        return new() {
            AllocatedMemory = 2048,
            Width = 1280,
            Height = 720,
            MinecraftPath = Path.Combine(FileSystem.AppDataDirectory, GameDirectoryName)
        };
    }
}