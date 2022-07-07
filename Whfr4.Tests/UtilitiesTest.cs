namespace Whfr4.Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void TestBonus()
        {
            Assert.AreEqual(1, Utilities.Bonus(10));
            Assert.AreEqual(1, Utilities.Bonus(11));
            Assert.AreEqual(1, Utilities.Bonus(12));
            Assert.AreEqual(1, Utilities.Bonus(13));
            Assert.AreEqual(2, Utilities.Bonus(20));
            Assert.AreEqual(3, Utilities.Bonus(30));
            Assert.AreEqual(4, Utilities.Bonus(40));
        }

        [TestMethod]
        public void TestDouble()
        {
            Assert.IsTrue(Utilities.IsDouble(11));
            Assert.IsTrue(Utilities.IsDouble(22));
            Assert.IsTrue(Utilities.IsDouble(33));
            Assert.IsTrue(Utilities.IsDouble(44));
            Assert.IsTrue(Utilities.IsDouble(55));
            Assert.IsTrue(Utilities.IsDouble(66));
            Assert.IsTrue(Utilities.IsDouble(77));
            Assert.IsTrue(Utilities.IsDouble(88));
            Assert.IsTrue(Utilities.IsDouble(99));

            Assert.IsFalse(Utilities.IsDouble(10));
            Assert.IsFalse(Utilities.IsDouble(25));
            Assert.IsFalse(Utilities.IsDouble(39));
            Assert.IsFalse(Utilities.IsDouble(47));
        }

        [TestMethod]
        public void TestInvert()
        {
            Assert.AreEqual(12, Utilities.Invert(21));
            Assert.AreEqual(22, Utilities.Invert(22));
            Assert.AreEqual(34, Utilities.Invert(43));
            Assert.AreEqual(76, Utilities.Invert(67));
        }

        [TestMethod]
        public void TestLocalize()
        {
            Assert.AreEqual(Localization.Head, Utilities.Localize(5));
            Assert.AreEqual(Localization.LeftArm, Utilities.Localize(11));
            Assert.AreEqual(Localization.RightArm, Utilities.Localize(37));
            Assert.AreEqual(Localization.Body, Utilities.Localize(65));
            Assert.AreEqual(Localization.LeftLeg, Utilities.Localize(84));
            Assert.AreEqual(Localization.RightLeg, Utilities.Localize(97));
        }
    }
}