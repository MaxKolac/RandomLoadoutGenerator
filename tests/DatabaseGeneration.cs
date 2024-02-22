using RandomLoadoutGenerator.Database;

namespace RandomLoadoutGeneratorTests
{
    public static class DatabaseGeneration
    {
        //Use this unit-test to execute the Purge method to generate a fresh copy
        //of the database populated with all the weapons
        //[Fact]
        public static void GenerateDatabaseAndPopulateIt()
        {
            DatabaseContext.Purge();
        }
    }
}
