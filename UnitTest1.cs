using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsharpFeaturesFinalCheckpoint
{
    [TestClass]
    public class TestsBefore
    {
        readonly string[] GRADES = new string[]
        {
            "A", "B", "C", "D", "F"
        };

        [TestMethod]
        public void GetFirstName_NullableReference()
        {
            var student = new Student()
            {
                FirstName = "John"
            };

            string result = ($"The student is called {student.FirstName}.");

            Assert.AreEqual("The student is called John.", result);
        }

        [TestMethod]
        public async Task GetStudentsAsync_WithFullName()
        {
            string result = "";
            foreach (var student in await GetStudentsAsync())
            {
                result = result + ($"{student.FirstName} {student.LastName} - ");
            }

            Assert.AreEqual("John Doe - Jane Doe - John Smith - ", result);
        }

        static async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            await Task.Delay(2000);
            return new List<Student>()
            {
                new Student() {FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com",Grade = 'A'},
                new Student() {FirstName = "Jane", LastName = "Doe", Email="jane.doe@galvanize.com", Grade = 'B'},
                new Student() {FirstName = "John", LastName = "Smith", Email = "john.smith@galvanize.com", Grade = 'C'}
            };
        }

        [TestMethod]
        public void GetGradeFromEnd_WhenCalledWithValidNumber_ShouldReturnGradeFromEnd()
        {
            string result = getGradeFromEnd(4);

            Assert.AreEqual("B", result);
        }

        private string getGradeFromEnd(int indexFromEnd)
        {
            string item = GRADES[^indexFromEnd];
            return item;
        }

        [TestMethod]
        public void GetGradeRange_WhenCalledWithStartAndEndIndex_ShouldReturnGradeRange()
        {
            string[] result = getGradeRange(1, 3);

            CollectionAssert.AreEqual(new string[] { "B", "C", "D" }, result);
        }

        private string[] getGradeRange(int startIndex, int endIndex)
        {
            var length = endIndex - startIndex + 1;
            var result = GRADES[startIndex.. (startIndex + length)];
            return result;
        }

        [TestMethod]
        public void getPassOrFail_WithPropertyPatternMatching()
        {
            //Arrange
            var passingStudent = new Student("Jane", "Doe", true, 'B');
            var failingStudent = new Student("Jane", "Doe", true, 'D');

            //Act
            bool passResult = getPassOrFail_ByPropertyPatternMatching(passingStudent);
            bool failResult = getPassOrFail_ByPropertyPatternMatching(failingStudent);

            //Assert
            Assert.AreEqual(true, passResult && !failResult);
        }

        [TestMethod]
        public void getPassOrFail_WithTupleMatching()
        {
            //Arrange
            var passingStudent = new Student("Jane", "Doe", true, 'B');
            var failingStudent = new Student("Jane", "Doe", true, 'D');

            //Act
            bool passResult = getPassOrFail_ByTuplePatternMatching(passingStudent);
            bool failResult = getPassOrFail_ByTuplePatternMatching(failingStudent);

            //Assert
            Assert.AreEqual(true, passResult && !failResult);
        }

        [TestMethod]
        public void getPassOrFail_WithPositionalMatching()
        {
            //Arrange
            var passingStudent = new Student("Jane", "Doe", true, 'B');
            var failingStudent = new Student("Jane", "Doe", true, 'D');

            //Act
            bool passResult = getPassOrFail_ByPositionalPatternMatching(passingStudent);
            bool failResult = getPassOrFail_ByPositionalPatternMatching(failingStudent);

            //Assert
            Assert.AreEqual(true, passResult && !failResult);
        }

        public static bool getPassOrFail_ByPropertyPatternMatching(Student student)
        {
            return student switch
            {
                { TuitionPaid: false } => false,
                { Grade: 'D' } => false,
                { Grade: 'F' } => false,
                _ => true
            };
        }

        public static bool getPassOrFail_ByTuplePatternMatching(Student student)
        {
            return (student.TuitionPaid, student.Grade) switch
            {
                (true, 'A') => true,
                (true, 'B') => true,
                (true, 'C') => true,
                _ => false
            };
        }

        public static bool getPassOrFail_ByPositionalPatternMatching(Student student)
        {
            return student switch
            {
                var (_, _, paid, grade) when paid && (grade == 'A' || grade == 'B' || grade == 'C') => true,
                _ => false
            };
        }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool TuitionPaid { get; set; }
        public char Grade { get; set; }

        public Student(string firstName, string lastName, string email, bool tuitionPaid, char grade) =>
            (FirstName, LastName, Email, TuitionPaid, Grade) = (firstName, lastName, email, tuitionPaid, grade);

        public Student(string firstName, string lastName, bool tuitionPaid, char grade) =>
           (FirstName, LastName, TuitionPaid, Grade) = (firstName, lastName, tuitionPaid, grade);

        public Student()
        {
        }

        public void Deconstruct(out string firstName, out string lastName, out bool tuitionPaid, out char grade) =>
            (firstName, lastName, tuitionPaid, grade) = (FirstName, LastName, TuitionPaid, Grade);
    }
}