using System;

namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        int age = today.Year - dob.Year;

        //! In case they havent had their birthday yet, we dont give them the extra year
        if (dob > today.AddYears(-age)) age--;
        return age;
    }
}
