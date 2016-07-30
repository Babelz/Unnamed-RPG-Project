using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vRPGEngine;
using System.Collections.Generic;

namespace vRPGEngineTests
{
    [TestClass]
    public class AllocatorTests
    {
        public sealed class Foo
        {
            public int bar;
        }

        [TestMethod]
        public void PoolResize()
        {
            var allocator = new PoolAllocator<Foo>(128, () => { return new Foo(); });

            var foos = new List<Foo>();

            for (var i = 0; i < 128; i++) foos.Add(allocator.Allocate());

            Assert.IsTrue(allocator.Size > 128);
        }

        [TestMethod]
        public void PoolRelease()
        {
            var allocator = new PoolAllocator<Foo>(128, () => { return new Foo(); });

            var foos = new List<Foo>();

            for (var i = 0; i < 127; i++) foos.Add(allocator.Allocate());

            foos.ForEach(f => allocator.Deallocate(f));

            Assert.AreEqual(127, allocator.Size - 1);
            Assert.AreEqual(127, allocator.Released);
        }
    }
}
