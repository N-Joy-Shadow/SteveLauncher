using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftServerService {
    /// <summary>
    /// db에 등록된 서버 도메인을 전부 서버 정보를 받아와서 리스트로 반환하는 함수
    /// </summary>
    /// <returns></returns>
    Task<ICollection<MinecraftServerInfo>> GetServerStatusListAsync();
    /// <summary>
    /// 서버를 삭제하는 함수
    /// </summary>
    /// <param name="serverInfo"></param>
    /// <returns></returns>
    bool DeleteServer(MinecraftServerInfo serverInfo);
    
    /// <summary>
    /// 서버를 등록하는 함수
    /// </summary>
    /// <param name="hostname"></param>
    /// <returns></returns>
    Task<bool> RegisterServer(MinecraftHost hostname);
    /// <summary>
    /// SRV를 활용한 서버 정보를 가져오는 함수
    /// </summary>
    /// <param name="hostname"></param>
    /// <returns></returns>
    Task<MinecraftServerInfo?> FetchServerInfo(MinecraftHost hostname);
    
    /// <summary>
    /// 서버 등록할 때 서버 정보를 가져오는 함수 SRV resolve가 포함되어 있음.
    /// </summary>
    /// <param name="hostname">hostname</param>
    /// <returns></returns>
    Task<MinecraftServerInfo?> FetchTempServerInfo(MinecraftHost hostname);
}