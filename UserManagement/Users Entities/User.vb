Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class User
    Sub New()
        Salt = GenerateSalt()
        Password = GeneratePassword()
    End Sub
    Public Property Id As Long

    <Required>
    <StringLength(50)>
    <Display(Name:="User Name")>
    Public Property UserName As String = ""

    <Required>
    <MaxLength(32)>
    Public Property Password As Byte()

    <Required>
    <MaxLength(64)>
    Public Property Salt As Byte()

    <Required>
    <StringLength(200)>
    <Display(Name:="Email Address")>
    Public Property EmailAddress As String = ""

    Private Function GenerateSalt() As Byte()
        Dim tmp(63) As Byte
        tmp.FillRandom
        Return tmp
    End Function

    Private Function GeneratePassword() As Byte()
        Dim tmp(2047) As Byte
        tmp.FillRandom
        Return tmp.HashSHA256(Salt)
    End Function


    Public Overrides Function ToString() As String
        Return $"User Name:{UserName}
Email:{EmailAddress}
Password Hash:
{Password.ToHex}
Salt:
{Salt.ToHex}"
    End Function


End Class
