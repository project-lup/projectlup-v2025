using System.Collections.Generic;
using System.Text;

/// <summary>
/// CSV 파싱 유틸리티 - 따옴표로 감싸진 필드 처리
/// </summary>
public static class CSVParser
{
    /// <summary>
    /// CSV 라인을 파싱하여 필드 배열로 변환
    /// "Hello, World",123,"Another ""quoted"" value" → ["Hello, World", "123", "Another \"quoted\" value"]
    /// </summary>
    public static string[] ParseLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // 다음 문자도 따옴표면 escape된 따옴표
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField.Append('"');
                    i++; // 다음 따옴표 건너뛰기
                }
                else
                {
                    // 따옴표 모드 토글
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // 따옴표 밖의 쉼표 = 필드 구분자
                fields.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        // 마지막 필드 추가
        fields.Add(currentField.ToString());

        return fields.ToArray();
    }

    /// <summary>
    /// CSV 텍스트를 라인 배열로 분리 (따옴표 안의 개행 처리)
    /// </summary>
    public static string[] SplitLines(string csvText)
    {
        List<string> lines = new List<string>();
        StringBuilder currentLine = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < csvText.Length; i++)
        {
            char c = csvText[i];

            if (c == '"')
            {
                // escape된 따옴표 체크
                if (i + 1 < csvText.Length && csvText[i + 1] == '"')
                {
                    currentLine.Append("\"\"");
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                    currentLine.Append(c);
                }
            }
            else if (c == '\n' && !inQuotes)
            {
                // 따옴표 밖의 개행 = 라인 구분
                string line = currentLine.ToString().TrimEnd('\r');
                // 빈 행도 유지 (행 번호 일치를 위해)
                lines.Add(line);
                currentLine.Clear();
            }
            else if (c == '\r')
            {
                // \r은 무시 (Windows 개행 처리)
                continue;
            }
            else
            {
                currentLine.Append(c);
            }
        }

        // 마지막 라인 추가
        if (currentLine.Length > 0)
        {
            string line = currentLine.ToString().TrimEnd('\r');
            // 빈 행도 유지 (행 번호 일치를 위해)
            lines.Add(line);
        }

        return lines.ToArray();
    }
}
