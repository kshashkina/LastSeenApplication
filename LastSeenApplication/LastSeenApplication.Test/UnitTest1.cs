using System;
using LastSeenApplication;
using NUnit.Framework;

namespace LastSeenApplicationTests
{
    public class Tests
    {
        [Test]
        public void GetTimeAgoString_ShouldReturnJustNow_WhenDifferenceIsLessThan30Seconds()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromSeconds(20);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("just now", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnLessThanAMinuteAgo_WhenDifferenceIsLessThan60Seconds()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromSeconds(40);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("less than a minute ago", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnCoupleOfMinutesAgo_WhenDifferenceIsLessThan59Minutes()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromMinutes(30);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("couple of minutes ago", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnAnHourAgo_WhenDifferenceIsLessThan119Minutes()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromMinutes(60);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("an hour ago", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnToday_WhenDifferenceIsLessThan23Hours()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromHours(12);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("today", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnYesterday_WhenDifferenceIsLessThan47Hours()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromHours(24);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("yesterday", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnThisWeek_WhenDifferenceIsLessThan7Days()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromDays(3);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("this week", timeAgo);
        }

        [Test]
        public void GetTimeAgoString_ShouldReturnLongTimeAgo_WhenDifferenceIsMoreThan7Days()
        {
            // Arrange
            TimeSpan difference = TimeSpan.FromDays(8);

            // Act
            string timeAgo = Program.GetTimeAgoString(difference);

            // Assert
            Assert.AreEqual("long time ago", timeAgo);
        }
        private StringWriter consoleOutput;

        [SetUp]
        public void Setup()
        {
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            consoleOutput.Dispose();
            Console.SetOut(Console.Out);
        }

        [Test]
        public void RetrieveUserData_ShouldPrintOnlineMessage_WhenUserIsOnline()
        {
            // Arrange
            var user = new User() { nickname = "User1", lastSeenDate = null };

            // Act
            Program.RetrieveUserData(user);

            // Assert
            string expectedOutput = "User1 is online." + Environment.NewLine;
            Assert.AreEqual(expectedOutput, consoleOutput.ToString());
        }

        [Test]
        public void RetrieveUserData_ShouldPrintOfflineMessage_WhenUserIsOffline()
        {
            // Arrange
            var user = new User() { nickname = "User2", lastSeenDate = DateTime.Now.AddMinutes(-5) };

            // Act
            Program.RetrieveUserData(user);

            // Assert
            string expectedOutput = "User2 was online couple of minutes ago" + Environment.NewLine;
            Assert.AreEqual(expectedOutput, consoleOutput.ToString());
        }
    }
}
