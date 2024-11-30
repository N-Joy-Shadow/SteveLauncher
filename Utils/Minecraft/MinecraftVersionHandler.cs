using System.Text.RegularExpressions;
using CmlLib.Core.VersionMetadata;

namespace SteveLauncher.Utils.Minecraft;

public class MinecraftVersionHandler {
    private static readonly Regex VersionRegex = new Regex(@"\d+\.\d+(\.\d+)?", RegexOptions.Compiled);

    public static List<string> ParsingVersions(string version, VersionMetadataCollection versionMetadataCollection) {
        var parsedVersions = VersionRegex.Matches(version)
            .Select(match => match.Value)
            .Distinct()
            .ToList();

        switch (parsedVersions.Count()) {
            case 2:
                return GetVersionsInRange(parsedVersions[0], parsedVersions[1], versionMetadataCollection);
            case 3:
                return parsedVersions;
                break;

        }

        return parsedVersions;

    }
    
    private static List<string> GetVersionsInRange(string lowerVersion, string upperVersion, VersionMetadataCollection versionMetadataCollection)
    {

        // 등록된 버전들을 정규식으로 파싱합니다 (예: fabric-1.20.2, forge-1.18.4 등).
        var parsedAllVersions = versionMetadataCollection.Select(v => VersionRegex.Match(v.Name).Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .Distinct()
            .ToList();

        // 파싱된 버전들을 비교하여 범위 내의 버전들을 가져옵니다.
        return parsedAllVersions.Where(v => IsVersionInRange(v, lowerVersion, upperVersion)).ToList();
    }
    
    private static bool IsVersionInRange(string version, string lowerVersion, string upperVersion)
    {
        // 버전 문자열을 비교 가능한 형태로 변환합니다.
        Version v = Version.Parse(version);
        Version lower = Version.Parse(lowerVersion);
        Version upper = Version.Parse(upperVersion);

        // 하위 버전과 상위 버전 사이에 있는지 확인합니다.
        return v >= lower && v <= upper;
    }
}