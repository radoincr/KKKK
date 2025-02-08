namespace Service.MyToolServices;
public class ArabicNumber
{
    private static readonly string[] aname = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر" };
    private static readonly string[] aname10 = { "", "عشر", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
    private static readonly string[] aname100 = { "", "مئة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
    private static readonly string[] aname1000 = { "", "ألف", "ألفان", "آلاف" };
    private static readonly string[] aname1000000 = { "", "مليون", "مليونان", "ملايين" };
    
    public string arabicNumber(double num)
    {
        if (num == 0) return "صفر";

        int num6 = (int)(num / 1_000_000);
        int num4 = (int)((num % 1_000_000) / 1000);
        int num3 = (int)((num % 1000) / 100);
        int num2 = (int)((num % 100) / 10);
        int num1 = (int)(num % 10);

        string result = "";

        if (num6 > 0)
        {
            result += (num6 == 1 ? aname1000000[1] : num6 == 2 ? aname1000000[2] : arabicNumber(num6) + " " + aname1000000[3]) + " ";
        }

        if (num4 > 0)
        {
            result += (num4 == 1 ? aname1000[1] : num4 == 2 ? aname1000[2] : arabicNumber(num4) + " " + aname1000[3]) + " ";
        }

        if (num3 > 0)
        {
            result += aname100[num3] + " ";
        }

        if (num2 == 1 && num1 > 0)
        {
            result += aname[10 + num1] + " ";
        }
        else
        {
            if (num1 > 0)
            {
                result += aname[num1] + " ";
            }
            if (num2 > 0)
            {
                if (num1 > 0)
                    result += "و ";
                result += aname10[num2] + " ";
            }
        }

        return result.Trim();
    }
}