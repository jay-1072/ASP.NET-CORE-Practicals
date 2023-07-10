using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Practical17.Models;
using System.Data;

namespace Practical17.Data.Repository;

public class StudentRepository : IRepository<StudentModel>
{
    private readonly MyDBContext _myDBContext;
    public StudentRepository(MyDBContext myDBContext)
    {
        _myDBContext = myDBContext;
    }

    public async Task<bool> Add(StudentModel entity)
    {
        bool IsAdded = false;

        try
        {
            await _myDBContext.Students.AddAsync(entity);
            await _myDBContext.SaveChangesAsync();
            IsAdded = true;
        }
        catch(DBConcurrencyException)
        {
            throw;
        }
        return IsAdded;
    }
    public async Task<bool> Update(int id, StudentModel entity)
    {
        bool IsUpdated = false;

        var studentToUpdate = _myDBContext.Students.Find(id);
        if (studentToUpdate != null)
        {
            studentToUpdate.EnrollmentNumber = entity.EnrollmentNumber;
            studentToUpdate.FirstName = entity.FirstName;
            studentToUpdate.LastName = entity.LastName;
            studentToUpdate.Email = entity.Email;
            studentToUpdate.DateOfBirth = entity.DateOfBirth;
            studentToUpdate.Gender = entity.Gender;
            studentToUpdate.Address = entity.Address;

            try
            {
                await _myDBContext.SaveChangesAsync();
                IsUpdated = true;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }

        return IsUpdated;
    }

    public async Task<bool> Delete(int id)
    {
        bool IsDeleted = false;

        var studentToDelete = await _myDBContext.Students.FindAsync(id);
        if(studentToDelete != null)
        {
            _myDBContext.Students.Remove(studentToDelete);
            await _myDBContext.SaveChangesAsync();
            IsDeleted = true;
        }

        return IsDeleted;
    }

    public async Task<StudentModel> Get(int id)
    {
        var student = await _myDBContext.Students.FindAsync(id);
        return student!;
    }

    public async Task<List<StudentModel>> GetAll()
    {
        var students = await _myDBContext.Students.ToListAsync();
        foreach (var student in students)
        {
            if(String.IsNullOrEmpty(student.Address))
            {
                student.Address = "NA";
            }
        }
        return students;
    }
}
