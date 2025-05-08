using System.Text.RegularExpressions;
using UnityEngine;

public static class ValidateString
{
    public static void ValidateID(string input)
    {
        var pattern = @"^[a-zA-Z0-9.@_-]{3,20}$";

        if (Regex.IsMatch(input, pattern))
        {
            Debug.Log("아이디 유효함");
        }
        else
        {
            Debug.Log("아이디 유효하지 않음");
        }
    }
    
    public static void ValidatePassword(string input)
    {
        var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,30}$";

        if (Regex.IsMatch(input, pattern))
        {
            Debug.Log("비밀번호 유효함");
        }
        else
        {
            Debug.Log("비밀번호 유효하지 않음");
        }
    }
}
