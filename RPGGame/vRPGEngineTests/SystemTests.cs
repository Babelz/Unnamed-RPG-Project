using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vRPGEngine;
using vRPGEngine.ECS;

namespace vRPGEngineTests
{
    [TestClass]
    public class SystemTests
    {
        public class TestComponent : Component<TestComponent>
        {
        }

        public class TestComponentManager : ComponentManager<TestComponent>
        {
            private TestComponentManager()
                : base()
            {
            }
        }

        [TestMethod]
        public void ReferenceFromGenericInterface()
        {
            var a = SystemManager<EntityManager>.Instance;
            var b = SystemManager<TestComponent>.Instance;
            
            Assert.AreNotEqual(a, b);    
        }

        [TestMethod]
        public void Reference()
        {
            var a = EntityManager.Instance;
            var b = TestComponentManager.Instance;

            Assert.IsFalse(ReferenceEquals(a, b));
        }
    }
}
