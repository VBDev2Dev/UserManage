Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text

Module Extensions

    <Extension>
    Sub FillRandom(data As Byte())

        Dim rng As New RNGCryptoServiceProvider
        rng.GetNonZeroBytes(data)
    End Sub

    <Extension>
    Function Hash(Of T As HashAlgorithm)(value As String, Salt As Byte(), HashType As T) As Byte()
        Dim enc As Encoding = Encoding.Unicode
        Return Salt.Concat(enc.GetBytes(value)).ToArray.Hash(Salt, HashType)
    End Function

    <Extension>
    Function Hash(Of T As HashAlgorithm)(value As Byte(), Salt As Byte(), HashType As T) As Byte()
        Return HashType.ComputeHash(Salt.Concat(value).ToArray)
    End Function
    <Extension>
    Function HashSHA256(Value As String, salt As Byte()) As Byte()
        Return Value.Hash(salt, New SHA256Managed)
    End Function
    <Extension>
    Function HashSHA256(Value As Byte(), salt As Byte()) As Byte()
        Return Value.Hash(salt, New SHA256Managed)
    End Function

    <Extension>
    Function ToHex(bytes As Byte()) As String

        Dim sb As New StringBuilder
        For Each b In bytes
            sb.Append($"{b.ToString("X2")}")
        Next
        Return sb.ToString
    End Function



End Module
