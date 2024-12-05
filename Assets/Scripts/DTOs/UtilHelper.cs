using System.Text.RegularExpressions;

public static class UtilHelper
{
    // JSON 응답에서 메시지를 추출하는 메서드
    public static string GetMessageFromResponse(string response)
    {
        Match match = Regex.Match(response, "\"Message\":\"([^\"]+)\"");
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return "Unknown response";
    }
}
