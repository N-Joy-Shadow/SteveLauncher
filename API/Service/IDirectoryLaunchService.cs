namespace SteveLauncher.API.Service;

public interface IDirectoryLaunchService {
    /// <summary>
    /// 외부 FileManager로 폴더를 여는 함수
    /// </summary>
    /// <param name="directoryPath">finder또는 explore로 열 폴더 경로</param>
    void Open(string directoryPath);
}