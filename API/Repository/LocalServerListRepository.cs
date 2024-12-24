using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Repository;

public interface ILocalServerListRepository {
    /// <summary>
    /// 전체 서버 목록을 가져옵니다.
    /// </summary>
    public List<MinecraftMultiHost> GetServerList();
    /// <summary>
    /// 데이터 베이스에 서버를 추가합니다.
    /// </summary>
    /// <param name="host">마인크래프트 서버 주소</param>
    /// <param name="srvHost">마인크래프트 SRV 서버 주소</param>
    /// <returns>성공시 True 반환</returns>
    public Task<bool> AddServer(MinecraftHost host,MinecraftHost srvHost);
    
    /// <summary>
    /// 서버주소를 찾아 해당 서버를 삭제합니다.
    /// </summary>
    /// <param name="server">서버 주소</param>
    /// <returns></returns>
    public bool RemoveServer(MinecraftHost server);
    
    /// <summary>
    /// 서버를 찾아 반환합니다.
    /// </summary>
    /// <param name="server">서버 주소</param>
    /// <returns>서버가 있으면 서버주소와 SRV 서버 주소를 반환하고 없으면 Null 반환</returns>
    public MinecraftMultiHost? FindServer(MinecraftHost server);
}