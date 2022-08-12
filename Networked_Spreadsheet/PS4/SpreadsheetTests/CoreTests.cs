/// Written by Bridger Holt for Fall 2018 CS 3500.
/// Not to be shared with anyone.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;

using SpreadsheetUtilities;
using SS;

/// <summary>
/// Contains all the tests for Spreadsheet.
/// </summary>
namespace SpreadsheetTests
{
    /// <summary>
    /// Contains all the current core tests for Spreadsheet.
    /// </summary>
    [TestClass]
    public class CoreTests
    {
        /// <summary>
        /// Very basic test that no exception is thrown, no non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var ss = new Spreadsheet();

            var cells = ss.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(cells.MoveNext());
            Assert.IsFalse(ss.Changed);
        }

        /// <summary>
        /// Very basic test that no exception is thrown, no non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestConstructorFull()
        {
            var ss = new Spreadsheet(s => true, s => s, "default");

            var cells = ss.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(cells.MoveNext());
            Assert.IsFalse(ss.Changed);
        }

        /// <summary>
        /// Tests that the load constructor works properly.
        /// </summary>
        [TestMethod]
        public void TestConstructorLoad()
        {
            var ss = new Spreadsheet(
                "TestConstructorLoad1.xml", s => true, s => s, "default");

            var cells = ss.GetNamesOfAllNonemptyCells();
            var cellSet = new HashSet<string>(cells);

            var expectedCells = new Dictionary<string, string>
            {
                { "A1", new Formula("A3 + A5").ToString() },
                { "A3", double.Parse("100.0").ToString() },
                { "A5", new Formula("30 + D10").ToString() },
                { "D10", double.Parse("50").ToString() },
                { "F10", "hello" },
            };

            // Verify there are no extra cells, and each cell has the appropriate name.
            Assert.IsTrue(cellSet.SetEquals(new HashSet<string>(expectedCells.Keys)));

            foreach (var cellName in cells)
            {
                var expected = expectedCells[cellName];
                var actual = ss.GetCellContents(cellName).ToString();
                Assert.AreEqual(expected, actual);
            }

            Assert.IsFalse(ss.Changed);
        }


        /// <summary>
        /// Tests that GetCellContents returns "" for a non-set cell.
        /// </summary>
        [TestMethod]
        public void TestEmptyCellContents()
        {
            var ss = new Spreadsheet();

            Assert.AreEqual("", ss.GetCellContents("B30"));
        }


        /// <summary>
        /// Tests that GetCellValue returns "" for a non-set cell.
        /// </summary>
        [TestMethod]
        public void TestEmptyCellValue()
        {
            var ss = new Spreadsheet();

            Assert.AreEqual("", ss.GetCellValue("B30"));
        }


        // -- SetContentsOfCell -- \\
        /// <summary>
        /// Very basic test for SetContentsOfCell with a number.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDouble()
        {
            var ss = new Spreadsheet();

            var dependents = ss.SetContentsOfCell("A1", "10.5");
            Assert.AreEqual(1, dependents.Count);
            Assert.IsTrue(dependents.Contains("A1"));
            Assert.AreEqual(10.5, ss.GetCellContents("A1"));

            var cells = ss.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(
                HaveSameValues(cells, new List<string> { "A1" }));
            Assert.IsTrue(ss.Changed);
        }


        /// <summary>
        /// Very basic test for SetContentsOfCell with a formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsFormula()
        {
            var ss = new Spreadsheet();

            var dependents = ss.SetContentsOfCell("A1", "=10.5");
            Assert.AreEqual(1, dependents.Count);
            Assert.IsTrue(dependents.Contains("A1"));
            Assert.AreEqual(new Formula("10.5"), ss.GetCellContents("A1"));

            var cells = ss.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(
                HaveSameValues(cells, new List<string> { "A1" }));
            Assert.IsTrue(ss.Changed);
        }


        /// <summary>
        /// Very basic test for SetContentsOfCell with a string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsString()
        {
            var ss = new Spreadsheet();

            var dependents = ss.SetContentsOfCell("A1", "helllo");
            Assert.AreEqual(1, dependents.Count);
            Assert.IsTrue(dependents.Contains("A1"));
            Assert.AreEqual("helllo", ss.GetCellContents("A1"));

            var cells = ss.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(
                HaveSameValues(cells, new List<string> { "A1" }));
            Assert.IsTrue(ss.Changed);
        }



        // -- SetContentsOfCell overloads InvalidNameException -- \\
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsNameExceptionStartWithDigit()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("1A", "100.0");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsNameExceptionStartWithSymbol()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("@A10", "100.0");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsNameExceptionStartWithSpace()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell(" A10", "100.0");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsNameExceptionEmptyName()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("", "100.0");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsDoubleNameException()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1@", "100.0");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsStringNameException()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1@", "a");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaNameException()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1@", "=100");
        }
        

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsValidateInvalid1()
        {
            var ss = new Spreadsheet(s => s == "A", s => s, "default");
            ss.SetContentsOfCell("a10", "=100");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsValidateInvalid2()
        {
            var ss = new Spreadsheet(s => s == "A", s => s, "default");
            ss.SetContentsOfCell("B10", "=100");
        }


        // -- SetContentsOfCell overloads ArgumentNullException -- \\
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellContentsStringNullArg()
        {
            var ss = new Spreadsheet();
            String s = null;
            ss.SetContentsOfCell("A1", s);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellContentsNullArg()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", null);
        }



        // -- Circular dependency -- \\
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularDependency1()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=B1");
            ss.SetContentsOfCell("B1", "=A1");
        }


        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularDependency2()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=B1");
            ss.SetContentsOfCell("B1", "=C2 + C3");
            ss.SetContentsOfCell("C2", "100.0");
            ss.SetContentsOfCell("C3", "=F5 + 100");
            ss.SetContentsOfCell("F5", "=10.0*B1");
        }


        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularDependency3()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=B1");
            ss.SetContentsOfCell("B1", "=C2 + C3");
            ss.SetContentsOfCell("C2", "100.0");
            ss.SetContentsOfCell("C3", "=F5 + 100");
            ss.SetContentsOfCell("B1", "=C3 * 10");
            ss.SetContentsOfCell("F5", "=10.0*B1");
        }


        /// <summary>
        /// That a cell will not be added if it would cause a circular dependency.
        /// </summary>
        [TestMethod]
        public void TestCircularDependencyUnchanged()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=B1");
            ss.SetContentsOfCell("B1", "=C2 + C3");
            ss.SetContentsOfCell("C2", "100.0");
            ss.SetContentsOfCell("C3", "=F5 + 100");
            ss.SetContentsOfCell("B1", "=C3 * 10");

            try
            {
                ss.SetContentsOfCell("F5", "=10.0*B1");
            }
            catch (CircularException)
            {

            }

            Assert.AreEqual("", ss.GetCellContents("F5"));
        }


        /// <summary>
        /// That Changed remains false if adding a circular dependency.
        /// </summary>
        [TestMethod]
        public void TestCircularDependencyChangedFalse()
        {
            var ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);

            try
            {
                ss.SetContentsOfCell("F5", "=F5");
            }
            catch (CircularException)
            {

            }

            Assert.AreEqual("", ss.GetCellContents("F5"));
            Assert.IsFalse(ss.Changed);
        }



        [TestMethod]
        public void TestNoCircularDependency()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=B1");
            ss.SetContentsOfCell("B1", "=C2 + C3");
            ss.SetContentsOfCell("C2", "100.0");
            ss.SetContentsOfCell("C3", "=F5 + 100");
            ss.SetContentsOfCell("B1", "=C2 * C2");
            ss.SetContentsOfCell("F5", "=10.0*B1");
        }




        /// <summary>
        /// Sets several contents, using double and Formula overloads of SetContentsOfCell.
        /// </summary>
        [TestMethod]
        public void TestComplexDependencies()
        {
            var ss = new Spreadsheet();

            var depF15 = ss.SetContentsOfCell("F15", "=D10 + E2 + B3");
            var depE2 = ss.SetContentsOfCell("E2", "=D10 + A1");
            var depD10 = ss.SetContentsOfCell("D10", "=A1");
            var depA1 = ss.SetContentsOfCell("A1", "10.5");
            var depB3 = ss.SetContentsOfCell("B3", "15.10");

            Assert.IsTrue(depF15.SetEquals(new HashSet<string> { "F15" }));
            Assert.IsTrue(depE2.SetEquals(new HashSet<string> { "E2", "F15", }));
            Assert.IsTrue(depD10.SetEquals(new HashSet<string> { "D10", "F15", "E2" }));
            Assert.IsTrue(depA1.SetEquals(new HashSet<string> { "A1", "D10", "F15", "E2" }));
            Assert.IsTrue(depB3.SetEquals(new HashSet<string> { "B3", "F15" }));

        }



        /// <summary>
        /// Sets a cell's contents twice, ensuring dependencies are changed appropriately.
        /// </summary>
        [TestMethod]
        public void TestSetContentsTwice()
        {
            var ss = new Spreadsheet();

            ss.SetContentsOfCell("A2", "=A1");
            var depPre = ss.SetContentsOfCell("A1", "10.5");

            Assert.IsTrue(depPre.SetEquals(new HashSet<string> { "A2", "A1" }));
            
            ss.SetContentsOfCell("A2", "=100");
            var depPost = ss.SetContentsOfCell("A1", "10.5");

            Assert.IsTrue(depPost.SetEquals(new HashSet<string> { "A1" }));
        }



        // -- GetCellContents -- \\
        [TestMethod]
        public void TestGetCellContentsEmpty1()
        {
            var ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("A1"));
        }
        

        [TestMethod]
        public void TestGetCellContentsEmpty2()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "100");
            ss.SetContentsOfCell("A1", "");
            Assert.AreEqual("", ss.GetCellContents("A1"));
        }



        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsNull()
        {
            var ss = new Spreadsheet();
            ss.GetCellContents(null);
        }


        // -- GetCellValue -- \\
        [TestMethod]
        public void TestGetCellValue()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "=100 * 100");
            ss.SetContentsOfCell("A2", "100.0");
            ss.SetContentsOfCell("A3", "hello");

            Assert.AreEqual(100.0 * 100, ss.GetCellValue("A1"));
            Assert.AreEqual(100.0, ss.GetCellValue("A2"));
            Assert.AreEqual("hello", ss.GetCellValue("A3"));
        }

        [TestMethod]
        public void TestGetCellValueComplex()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "100");
            ss.SetContentsOfCell("A2", "=A1 + 10");
            ss.SetContentsOfCell("A3", "=2 * A2");
            ss.SetContentsOfCell("Z10", "=100 + A3 * A1");

            Assert.AreEqual(100.0 + 2 * (100 + 10) * 100, ss.GetCellValue("Z10"));
        }

        [TestMethod]
        public void TestGetCellValueComplexUpdate()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "100");
            ss.SetContentsOfCell("A2", "=A1 + 10");
            ss.SetContentsOfCell("A3", "=2 * A2");
            ss.SetContentsOfCell("Z10", "=100 + A3 * A1");

            Assert.AreEqual(100.0 + 2 * (100 + 10) * 100, ss.GetCellValue("Z10"));

            ss.SetContentsOfCell("A2", "20");

            Assert.AreEqual(100.0 + 2 * 20 * 100, ss.GetCellValue("Z10"));
        }


        // -- Stress -- \\
        [TestMethod]
        public void TestStress1()
        {
            var ss = new Spreadsheet();

            int last = 100;  // Takes 25s if set to 1000

            for (int i = 1; i < last; i++)
            {
                string cellName = "A" + i;
                string cellContents = "=A" + (i + 1);

                ss.SetContentsOfCell(cellName, cellContents);
            }

            ss.SetContentsOfCell("A" + last, "5");

            for (int i = 1; i <= last; i++)
            {
                Assert.AreEqual(5.0, ss.GetCellValue("A" + i));
            }
        }


        [TestMethod]
        public void TestStress2()
        {
            var ss = new Spreadsheet();

            int last = 1000;

            ss.SetContentsOfCell("A" + last, "5");

            for (int i = last - 1; i >= 1; i--)
            {
                string cellName = "A" + i;
                string cellContents = "=A" + (i + 1);

                ss.SetContentsOfCell(cellName, cellContents);
            }

            ss.SetContentsOfCell("A" + last, "10");


            for (int i = 1; i <= last; i++)
            {
                Assert.AreEqual(10.0, ss.GetCellValue("A" + i));
            }
        }


        // -- XML -- \\
        [TestMethod]
        public void TestSave()
        {
            var filename = "TestSave1.xml";

            var cells = new Dictionary<string, string>
            {
                { "A1", "=" + new Formula("A3 + A5").ToString() },
                { "A3", double.Parse("100.0").ToString() },
                { "A5", "=" + new Formula("30 + D10").ToString() },
                { "D10", double.Parse("50").ToString() },
                { "F10", "hello" },
            };


            var ss = new Spreadsheet();

            foreach (var cell in cells)
            {
                ss.SetContentsOfCell(cell.Key, cell.Value);
            }

            ss.Save(filename);

            Assert.IsFalse(ss.Changed);

            // Verify xml contents by reading.
            var settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            using (var reader = XmlReader.Create(filename, settings))
            {
                Assert.IsTrue(reader.Read());  // <?xml>
                Assert.IsTrue(reader.Read());  // <spreadsheet>
                Assert.IsTrue(reader.NodeType == XmlNodeType.Element);

                Assert.AreEqual("spreadsheet", reader.Name);

                Assert.AreEqual("default", reader.GetAttribute("version"));

                // Look for each element.
                while (cells.Count > 0 && reader.Read())  // <cell>
                {
                    Assert.AreEqual("cell", reader.Name);
                    Assert.IsTrue(reader.Read());  // <name>
                    Assert.AreEqual("name", reader.Name);
                    Assert.IsTrue(reader.Read());  // name

                    string name = reader.Value;
                    bool inDict = cells.TryGetValue(name, out string contents);
                    Assert.IsTrue(inDict);

                    Assert.IsTrue(reader.Read());  // </name>
                    Assert.IsTrue(reader.Read());  // <contents>
                    Assert.AreEqual("contents", reader.Name);
                    Assert.IsTrue(reader.Read());  // contents
                    Assert.AreEqual(contents, reader.Value);

                    cells.Remove(name);

                    Assert.IsTrue(reader.Read());  // </contents>
                    Assert.IsTrue(reader.Read());  // </cell>
                }

                Assert.AreEqual(0, cells.Count);
                Assert.IsTrue(reader.Read());  // </spreadsheet>
            }
        }


        [TestMethod]
        public void TestSaveEmpty()
        {
            var ss = new Spreadsheet();
            ss.Save("TestSaveEmpty1.xml");
        }


        [TestMethod]
        public void TestGetSavedVersion()
        {
            var ss = new Spreadsheet();
            Assert.AreEqual("default", ss.GetSavedVersion("TestGetSavedVersion1.xml"));
        }


        [TestMethod]
        public void TestLoadEmpty()
        {
            var ss = new Spreadsheet("TestLoadEmpty1.xml", s => true, s => s, "default");

            Assert.AreEqual(0, new HashSet<string>(ss.GetNamesOfAllNonemptyCells()).Count);
        }



        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetSavedVersionInvalidFile()
        {
            var ss = new Spreadsheet();
            ss.GetSavedVersion("TestGetSavedVersionInvalidFile1.xml");
        }


        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestIncorrectVersion()
        {
            try
            {
                var ss1 = new Spreadsheet("TestIncorrectVersion1.xml", s => true, s => s, "default");
            }
            catch
            {
                throw new Exception("Cannot test on basis of version because file throws error");
            }

            var ss2 = new Spreadsheet("TestIncorrectVersion1.xml", s => true, s => s, "2.0");
        }


        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadNonExistentFile()
        {
            var ss = new Spreadsheet("ANonExistentFileThatShouldntExist.xml", s => true, s => s, "default");
        }


        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveInvalidFileName()
        {
            var ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "500");
            ss.Save("");
        }


        /// <summary>
        /// Missing an ending cell element.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile1()
        {
            var ss = new Spreadsheet("TestInvalidFile1.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// One cell element has multiple name and contents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile2()
        {
            var ss = new Spreadsheet("TestInvalidFile2.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// A non-element where node should be.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile3()
        {
            var ss = new Spreadsheet("TestInvalidFile3.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// No root node.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile4()
        {
            var ss = new Spreadsheet("TestInvalidFile4.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// Incorrect root node name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile5()
        {
            var ss = new Spreadsheet("TestInvalidFile5.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// A cell node named sell.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile6()
        {
            var ss = new Spreadsheet("TestInvalidFile6.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// A cell with a element n instead of name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile7()
        {
            var ss = new Spreadsheet("TestInvalidFile7.xml", s => true, s => s, "default");
        }


        /// <summary>
        /// Document abruptly ending after a cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestInvalidFile8()
        {
            var ss = new Spreadsheet("TestInvalidFile8.xml", s => true, s => s, "default");
        }




        // -- Private -- \\

        /// <summary>
        /// Whether two enumerables contain the same strings. Order does not matter.
        /// </summary>
        private bool HaveSameValues(
            IEnumerable<string> enum1, IEnumerable<string> enum2)
        {
            var list1 = new List<string>(enum1);
            var list2 = new List<string>(enum2);

            list1.Sort();
            list2.Sort();

            return list1.SequenceEqual(list2);
        }
    }
}
