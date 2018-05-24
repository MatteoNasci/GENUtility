using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GENUtility.ByteManipulator;
using NUnit.Framework;
using GENUtility;
[TestFixture]
[TestOf(typeof(BytePacket))]
[Category("Utility")]
public class BytePacketTest
{
    BytePacket packet;
    [SetUp]
    public void GamePacketSetUp()
    {
        packet = new BytePacket(20);
    }
    [TearDown]
    public void GamePacketTeardown()
    {
        packet = null;
    }
    [Test]
    public void Test()
    {

    }
}