using PhoneNumbers;

namespace BeautySaloon.API.Helpers;

public static class PhoneNumberHelper
{
    public static string ConvertToE164PhoneNumber(string phoneNumber)
    {
        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        var regions = phoneNumberUtil.GetSupportedRegions();

        foreach (var region in regions)
        {
            PhoneNumber number = phoneNumberUtil.Parse(phoneNumber, region);

            var isValid = phoneNumberUtil.IsValidNumberForRegion(number, region);

            if (isValid)
            {
                return phoneNumberUtil.Format(number, PhoneNumberFormat.E164);
            }
        }

        return "";
    }
}
