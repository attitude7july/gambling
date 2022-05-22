namespace gambling.Application.Common.Helpers;
public static class ExtensionHelper
{

    public static int GetNumber()
    {
        Random random = new Random();
        return random.Next(0, 10);
    }
}
