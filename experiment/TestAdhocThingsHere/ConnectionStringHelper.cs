


using System.Configuration;


namespace TestAdhocThingsHere
{
    public static class ConnectionStrings
    {

    public static string AdventureWorks => ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString; 

    public static string AnotherDB => ConfigurationManager.ConnectionStrings["AnotherDB"].ConnectionString; 
    }
}


