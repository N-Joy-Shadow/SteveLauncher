using CmlLib.Core.Version;
using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    /// <summary>
    /// 해당 서버로 게임을 실행하는 함수
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    Task StartGame(MinecraftHost host);
    /// <summary>
    /// 해당 서버에서 플래이할 수 있는 마인크래프트 버전을 가져오는 함수
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    Task<List<string>> GetAvailableVersions(string version);
    /// <summary>
    /// 모든 버전을 가져옴
    /// </summary>
    Task GetVersions();
    
    /// <summary>
    /// 런처에서 설정한 설정을 마인크래프트에 적용시키는 함수 
    /// </summary>
    /// <param name="setting"></param>
    void SetSettings(MinecraftGameSetting setting);
    /// <summary>
    /// 플레이할 마인크래프트 버전을 설정하는 함수
    /// </summary>
    /// <param name="version">버전</param>
    void SetGameVersion(string version);
    
    /// <summary>
    /// 설정을 가져오는 함수
    /// </summary>
    /// <returns></returns>
    MinecraftGameSetting GetSetting();
}