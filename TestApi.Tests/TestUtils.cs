using NUnit.Framework;

namespace TestApi.Tests;

public static class TestUtils
{
    public static TExpected AssertIsType<TExpected>(this object actual, string message = null)
        where TExpected : class
    {
        TExpected result = actual as TExpected;
        Assert.IsNotNull(result, message);
        return result;
    }
}