namespace RepoLinter;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class GitIgnore
{
    private List<string> _ignorePatterns;
    private List<string> _exceptionPatterns;

    public GitIgnore(List<string> ignoreRules)
    {
        _ignorePatterns = new List<string>();
        _exceptionPatterns = new List<string>();

        foreach (var rule in ignoreRules)
        {
            ParseIgnoreRule(rule);
        }
        
    }

    private void ParseIgnoreRule(string rule)
    {
        // Skip empty lines and comments
        if (string.IsNullOrWhiteSpace(rule) || rule.StartsWith("#"))
            return;

        // Handle exceptions (lines starting with !)
        if (rule.StartsWith("!"))
        {
            _exceptionPatterns.Add(rule.Substring(1));
        }
        else
        {
            _ignorePatterns.Add(rule);
        }
    }

    public bool ShouldIgnore(string path)
    {
        // Check if the path matches any exception pattern
        if (_exceptionPatterns.Any(pattern => MatchesPattern(path, pattern)))
            return false;

        // Check if the path matches any ignore pattern
        foreach (var pattern in _ignorePatterns)
        {
            if (path.Contains(pattern))
            {
                return true;
            }
        }
        return _ignorePatterns.Any(pattern => MatchesPattern(path, pattern));
    }

    private bool MatchesPattern(string path, string pattern)
    {
        // Convert pattern to regex
        string regexPattern = Regex.Escape(pattern)
            .Replace(@"\*", ".*") // Replace * with .*
            .Replace(@"\?", ".");  // Replace ? with .

        // Handle directory-specific patterns (e.g., /folder)
        if (pattern.StartsWith("/"))
        {
            regexPattern = "^.*" + regexPattern + "$";
        }
        else
        {
            regexPattern = "^" + regexPattern + "$";
        }

        return Regex.IsMatch(path, regexPattern, RegexOptions.IgnoreCase);
    }
}
