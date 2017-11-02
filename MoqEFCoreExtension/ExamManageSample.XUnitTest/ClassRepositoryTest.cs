using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using MoqEFCoreExtension;
using ExamManageSample.Models;
using ExamManageSample.Models.DataModels;
using Xunit;

namespace ExamManageSample.XUnitTest
{
    /// <summary>
    /// ClassRepository������
    /// </summary>
    [Trait("TestManage", "ClassRepository����")]
    public class ClassRepositoryTest
    {
        /// <summary>
        /// DB Mock����
        /// </summary>
        Mock<IDBModel> _dbMock;
        /// <summary>
        /// �����Զ���
        /// </summary>
        IClassRepository _classRepository;
        public ClassRepositoryTest()
        {
            _dbMock = new Mock<IDBModel>();
            _classRepository = new ClassRepository(_dbMock.Object);
        }
        /// <summary>
        /// ��ѯ�༶����
        /// </summary>
        [Fact]
        public void GetClasses_Default_ReturnCount()
        {
            var data = new List<Classes> {
                new Classes { Id = 1, ClassName = "����1" },
                new Classes { Id = 2, ClassName = "����2" }
            }; 
        
            var mockSet = new Mock<DbSet<Classes>>().SetUpList(data);      
            _dbMock.Setup(c => c.Classes).Returns(mockSet.Object);
            var clses = _classRepository.GetClasses();
            Assert.Equal(2, clses.Count);
        }

        /// <summary>
        /// AddClass�쳣����
        /// </summary>
        [Fact]
        public void AddClass_AddNull_ThrowException()
        {
            _dbMock.Setup(db => db.Classes.Add(null)).Throws(new Exception("AddClass�쳣"));
            var ext = Assert.Throws<Exception>(() => _classRepository.AddClass(null));
            Assert.Contains("AddClass�쳣", ext.Message);
        }
        /// <summary>
        /// AddClass�������
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void AddClass_Default_ReturnTrue(int result)
        {
            _dbMock.Setup(db => db.Classes.Add(new Classes()));
            _dbMock.Setup(db => db.SaveChanges()).Returns(value: result);
            var backResult = _classRepository.AddClass(new Classes());
            Assert.Equal(result == 1, backResult);
        }
        /// <summary>
        /// ModifyClass�쳣����
        /// </summary>
        [Fact]
        public void ModifyClass_NotFind_ThrowException()
        {

            _dbMock.Setup(db => db.Classes.Find()).Returns(value: null);
            var ext = Assert.Throws<Exception>(() => _classRepository.ModifyClass(new Classes { Id = 111 }));
            Assert.Contains("��ѯ����IDΪ111�İ༶", ext.Message);
        }
        /// <summary>
        /// ModifyClass��������
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ModifyClass_Default_ReturnTrue(int result)
        {
            var cls = new Classes { Id = 111 };
            _dbMock.Setup(db => db.Classes.Find(cls.Id)).Returns(value: new Classes());
            _dbMock.Setup(db => db.SaveChanges()).Returns(value: result);
            var backResult = _classRepository.ModifyClass(cls);
            Assert.Equal(result == 1, backResult);
        }

        /// <summary>
        /// RemoveClass�쳣����
        /// </summary>
        [Fact]
        public void RemoveClass_NotFind_ThrowException()
        {
            _dbMock.Setup(db => db.Classes.Find()).Returns(value: null);
            var ext = Assert.Throws<Exception>(() => _classRepository.RemoveClass(111));
            Assert.Contains("��ѯ����IDΪ111�İ༶", ext.Message);
        }

        /// <summary>
        /// RemoveClass����
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void RemoveClass_Default_ReturnClass(int result)
        {
            var cls = new Classes { Id = 111 };
            _dbMock.Setup(db => db.Classes.Find(cls.Id)).Returns(value: new Classes());
            _dbMock.Setup(db => db.SaveChanges()).Returns(value: result);
            var backResult = _classRepository.RemoveClass(111);
            Assert.Equal(result == 1, backResult);
        }

    }

}
