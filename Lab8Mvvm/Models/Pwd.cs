using System;

namespace Lab8Mvvm.Models;

public sealed class Pwd : BusinessObject
{
    private readonly uint b = 37;
    private readonly uint c = 81;
    private readonly uint d = 13;
    private readonly uint e = 29;
    private readonly uint f = 0xDEADBEEF;

    private uint pwd;

    public Pwd() : this("Sarıtaş Yusuf Alper")
    {
    }

    public Pwd(string owner)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Owner cannot be empty.", nameof(owner));

        Owner = owner.Trim();
        CalculatePassword();
    }

    private void CalculatePassword()
    {
        uint sqrtC = (uint)Math.Sqrt(c);
        pwd = 0;

        foreach (char key in Owner)
        {
            uint term = unchecked(f - d * b - e * sqrtC - (uint)key);
            pwd ^= term;
        }
    }

    public uint GetPwd() => pwd;
    public string GetOwner() => Owner;
    public override uint Password => pwd;
}
