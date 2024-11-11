namespace SteveLauncher.API.Repository;

public interface ILocalServerListRepository {
    public List<string> GetServerList();
    public void AddServer(string server);
    public void RemoveServer(string server);
    public void FindServer(string server);
}