using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using MyMonitor;
using NUnit.Framework;



namespace MyMonitorTests
{
    public class MonitoringTest
    {
        private static Fixture fixture = new Fixture();
        private IMonitoring monitorTest;
        private DateTime startTime;
        private DateTime timeNow;

        [SetUp]
        public void Setup()
        {
            monitorTest = new Monitoring();
            startTime = fixture.Create<DateTime>();
            timeNow = DateTime.Now;
        }

        [Test]
        public void GIVEN_Monitoring_WHEN_CkeckTimeLifeNow_method_is_invoked_THEN_correct_value_is_returned()
        {
            var actual = monitorTest.CkeckTimeLifeNow(startTime);
            Assert.That(actual, Is.EqualTo(timeNow.Minute - startTime.Minute));
        }

        [Test]
        public void GIVEN_Monitoring_WHEN_LogInfo_method_is_invoked_THEN_no_exception_is_thrown()
        {
            Assert.DoesNotThrow(() => monitorTest.LogInfo(startTime, "Closed"));
        }

        [Test]
        public void GIVEN_Monitoring_WHEN_CheckLifeProcess_method_is_invoked_THEN_no_exception_is_thrown()
        {
            Assert.DoesNotThrow(() => monitorTest.CheckLifeProcess());
        }
    }
}