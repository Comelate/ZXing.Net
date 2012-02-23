/*
 * Copyright 2008 ZXing authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ZXing.Common.ReedSolomon
{
   /// <summary>
   /// <author>Sean Owen</author>
   /// </summary>
   [TestFixture]
   public sealed class ReedSolomonEncoderQRCodeTestCase : AbstractReedSolomonTestCase
   {

      /// <summary>
      /// Tests example given in ISO 18004, Annex I
      /// </summary>
      [Test]
      public void testISO18004Example()
      {
         int[] dataBytes = {
      0x10, 0x20, 0x0C, 0x56, 0x61, 0x80, 0xEC, 0x11,
      0xEC, 0x11, 0xEC, 0x11, 0xEC, 0x11, 0xEC, 0x11 };
         int[] expectedECBytes = {
      0xA5, 0x24, 0xD4, 0xC1, 0xED, 0x36, 0xC7, 0x87,
      0x2C, 0x55 };
         doTestQRCodeEncoding(dataBytes, expectedECBytes);
      }

      [Test]
      public void testQRCodeVersusDecoder()
      {
         Random random = getRandom();
         ReedSolomonEncoder encoder = new ReedSolomonEncoder(GenericGF.QR_CODE_FIELD_256);
         ReedSolomonDecoder decoder = new ReedSolomonDecoder(GenericGF.QR_CODE_FIELD_256);
         for (int i = 0; i < 100; i++)
         {
            int size = 1 + random.Next(1000);
            int[] toEncode = new int[size];
            int ecBytes = 1 + random.Next(2 * (1 + size / 8));
            ecBytes = Math.Min(ecBytes, size - 1);
            int dataBytes = size - ecBytes;
            for (int j = 0; j < dataBytes; j++)
            {
               toEncode[j] = random.Next(256);
            }
            int[] original = new int[dataBytes];
            Array.Copy(toEncode, 0, original, 0, dataBytes);
            encoder.encode(toEncode, ecBytes);
            Assert.IsTrue(decoder.decode(toEncode, ecBytes));
            assertArraysEqual(original, 0, toEncode, 0, dataBytes);
         }
      }

      private void assertArraysEqual(int[] original, int index, int[] toEncode, int index2, int length)
      {
         for (var x = index; x < length; x++)
         {
            if (original[x] != toEncode[x])
               Assert.Fail("not equal");
         }
      }

      // Need more tests I am sure
   }
}
