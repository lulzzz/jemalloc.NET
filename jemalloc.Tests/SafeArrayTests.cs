﻿using System;
using System.Numerics;

using Xunit;
namespace jemalloc.Tests
{
    public class SafeArrayTests : jemallocTest
    {
        [Fact(DisplayName = "Can construct SafeArray")]
        public void CanConstructSafeArray()
        {
            SafeArray<int> a = new SafeArray<int>(500);
            a[1] = 1000;
            Assert.Equal(1000, a[1]);
            a.Acquire();
            Assert.Equal(1000, a[1]);
            a.Acquire();
            a.Release();
            Assert.Equal(1000, a[1]);
            a.Release();
            Assert.Equal(1000, a[1]);
            a.Close();
            Assert.True(a.IsClosed);
            Assert.Throws<ObjectDisposedException>(() => a[1] == 1000);
            //int r = a[(2,3)]
        }

        [Fact(DisplayName = "Can convert to Vector")]
        public void CanConvertToVector()
        {
            SafeArray<uint> a = new SafeArray<uint>(8, 1, 11, 94, 5, 0, 0, 0, 8);
            Vector<uint> v = a.AcquireAsSingleVector();
            Assert.Equal(a[0], v[0]);
            Assert.Equal(a[3], v[3]);
            Assert.Equal(a[7], v[7]);
            SafeArray<uint> a2 = new SafeArray<uint>(12, 11, 112, 594, 65, 0, 0, 0, 8, 14, 90, 2, 8);
            Vector<uint> v2 = a2.AcquireSliceAsVector(2);
            Assert.Equal(594u, v2[0]);
        }

        [Fact]
        public void CanVectorizedMultiply()
        {
            SafeArray<uint> a = new SafeArray<uint>(8, 1, 2, 3, 4, 5, 6, 7, 8);
            SafeArray<int> b = new SafeArray<int>(8, 111, 22, 345, 40888, 3 , 777, 99, 6);
            a.VectorMultiply(2);
            Assert.Equal(2u, a[0]);
            Assert.Equal(8u, a[3]);
            Assert.Equal(16u, a[7]);
            b.VectorMultiply(6);
            Assert.Equal(666, b[0]);

        }
    }
}
