Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Partial Public Class UsersEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=UsersEntities")
    End Sub

    Public Overridable Property Users As DbSet(Of User)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        modelBuilder.Entity(Of User)() _
            .Property(Function(e) e.Password) _
            .IsFixedLength()

        modelBuilder.Entity(Of User)() _
            .Property(Function(e) e.Salt) _
            .IsFixedLength()
    End Sub
End Class
