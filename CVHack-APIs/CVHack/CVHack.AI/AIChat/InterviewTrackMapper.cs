using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace CVHack.AI.AIChat;

// بيحدد هنجيب الأسئلة من أنهي تراك (فولدر) وأنهي مستوى (ملف) بناءً على بيانات الوظيفة
public static class InterviewTrackMapper
{
    public const string BackendDotNet = "BackendDotNet";
    public const string FrontendReact = "FrontendReact";
    public const string FrontendAngular = "FrontendAngular";
    public const string DevOps = "DevOps";
    public const string Cybersecurity = "Cybersecurity";

    public static string ResolveCategory(string jobTitle)
    {
        var t = (jobTitle ?? string.Empty).ToLowerInvariant();

        if (Contains(t, "devops", "sre", "site reliability", "infrastructure", "platform engineer", "cloud engineer"))
            return DevOps;

        if (Contains(t, "security", "cyber", "penetration", "soc analyst", "appsec"))
            return Cybersecurity;

        if (Contains(t, "angular"))
            return FrontendAngular;

        if (Contains(t, "react", "next.js", "nextjs"))
            return FrontendReact;

        if (Contains(t, "frontend", "front-end", "front end", "ui developer"))
            return FrontendReact;

        return BackendDotNet; // الافتراضي
    }

    public static string ResolveLevel(string seniority)
    {
        var s = (seniority ?? string.Empty).ToLowerInvariant();

        if (Contains(s, "senior", "sr.", "lead", "principal", "staff"))
            return "senior";

        if (Contains(s, "junior", "jr.", "entry", "graduate", "intern", "fresh"))
            return "junior";

        return "mid";
    }

    private static bool Contains(string source, params string[] candidates)
        => candidates.Any(source.Contains);
}
