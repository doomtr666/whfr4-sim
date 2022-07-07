namespace Whfr4.Tests
{
    [TestClass]
    public class ConditionsTest
    {
        [TestMethod]
        public void Test()
        {
            Conditions conditions = new Conditions();
            conditions.Add(Condition.Stunned);
            Assert.AreEqual(1, conditions.Get(Condition.Stunned));
        }
    }
}
