using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

/// <summary>
/// Summary description for DateTimeFormatter
/// </summary>
public static class DateTimeFormatter
{
    #region initializer

    private static readonly Dictionary<DateFormat, string> FormatDictionary = new Dictionary<DateFormat, string>
                                                                              {
                                                                                  {DateFormat.ddMMyyyy, "dd/MM/yyyy"},
                                                                                  {DateFormat.MMddyyyy, "MM/dd/yyyy"}
                                                                              };// {DateFormat.ddMMMyyyy, "dd/MMM/yyyy"},
    #endregion

    public static DateTime ToDate(string strDate, DateFormat inputdateformat)
    {
        try
        {
            var di = new DateTimeFormatInfo { ShortDatePattern = FormatDictionary[inputdateformat] };
            var dt = DateTime.Parse(strDate, di);
            return dt;
        }
        catch (Exception)
        {
            throw new Exception(strDate + " is not a valid date with format " + inputdateformat.ToString());
        }

    }
    public static DateTime ToDateEndTime(string strDate, DateFormat inputdateformat)
    {

        DateTime dt = ToDate(strDate, inputdateformat).Date;
        dt = dt.AddDays(1);
        dt = dt.AddMilliseconds(-1);
        return dt;

    }
    public static DateTime ToDateStartTime(string strDate, DateFormat inputdateformat)
    {
        //DateTime dt = ToDate(strDate, inputdateformat).Date;
        ////dt = dt.AddDays(1);
        //dt = dt.AddMilliseconds(-1);
        //return dt;
        return ToDate(strDate, inputdateformat);
    }


}
public enum DateFormat
{
    ddMMyyyy = 0,
    MMddyyyy = 2
    //ddMMMyyyy = 1,

}