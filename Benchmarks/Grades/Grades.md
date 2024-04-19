# Grades

Inspired by the original [Twitter post](https://twitter.com/ozanyrc/status/1778921269670342776) around fixing nested if statements. This benchmarking is mostly about the obscure and interesting ways users posted to solve the problem.

This is for fun. No serious opinions on production performance, absolute correctness, readability, or even this flawed benchmarking process - but I can't help but dig into the esoteric ways this has been solved because I'm a nerd 😁

Each method will have the C# code, and the resulting IL via ILSpy (but the ILSpy version is static, because I've written the code as static for benchmarking).

1. [Methods](#Methods)
1. [Benchmarks](#Benchmarks)

## Methods

### Original

[Via ozanyrc](https://twitter.com/ozanyrc/status/1778921269670342776)
	
```csharp
public string CheckGrade(int score)
{
    if (score >= 90)
    {
        return "A";
    }
    else
    {
        if (score >= 80)
        {
            return "B";
        }
        else
        {
            if (score >= 70)
            {
                return "C";
            }
            else
            {
                if (score >= 60)
                {
                    return "D";
                }
                else
                {
                    return "F";
                }
            }
        }
    }
}
```

```csharp
.method public hidebysig static 
	string Original (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2822
	// Header size: 1
	// Code size: 50 (0x32)
	.maxstack 8

	// if (score >= 90)
	IL_0000: ldarg.0
	IL_0001: ldc.i4.s 90
	IL_0003: blt.s IL_000b

	// return "A";
	IL_0005: ldstr "A"
	IL_000a: ret

	// if (score >= 80)
	IL_000b: ldarg.0
	IL_000c: ldc.i4.s 80
	IL_000e: blt.s IL_0016

	// return "B";
	IL_0010: ldstr "B"
	IL_0015: ret

	// if (score >= 70)
	IL_0016: ldarg.0
	IL_0017: ldc.i4.s 70
	IL_0019: blt.s IL_0021

	// return "C";
	IL_001b: ldstr "C"
	IL_0020: ret

	// if (score >= 60)
	IL_0021: ldarg.0
	IL_0022: ldc.i4.s 60
	IL_0024: blt.s IL_002c

	// return "D";
	IL_0026: ldstr "D"
	IL_002b: ret

	// return "F";
	IL_002c: ldstr "F"
	IL_0031: ret
} // end of method GradeMethods::Original

```


### AgileJebrim1

[Via AgileJebrim](https://twitter.com/AgileJebrim/status/1779699293697311177)

Adapted from C/C++. The C# versions miss a bit of nuance, because the other examples in this also post use [ispc](https://ispc.github.io/), a variant of C for SPMD (single program, multiple data).

```csharp
public char CheckGrade(int score)
{
    var grade = 'A';
    grade = score < 90 ? 'B' : grade;
    grade = score < 80 ? 'C' : grade;
    grade = score < 70 ? 'D' : grade;
    grade = score < 60 ? 'F' : grade;
            
    return grade;
}
```

```csharp
.method public hidebysig static 
	char AgileJebrim1 (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2858
	// Header size: 12
	// Code size: 49 (0x31)
	.maxstack 2
	.locals init (
		[0] char grade
	)

	// char c = 'A';
	IL_0000: ldc.i4.s 65
	IL_0002: stloc.0
	// c = ((score < 90) ? 'B' : c);
	IL_0003: ldarg.0
	IL_0004: ldc.i4.s 90
	IL_0006: blt.s IL_000b

	IL_0008: ldloc.0
	IL_0009: br.s IL_000d

	// c = ((score < 80) ? 'C' : c);
	IL_000b: ldc.i4.s 66

	IL_000d: stloc.0
	IL_000e: ldarg.0
	IL_000f: ldc.i4.s 80
	IL_0011: blt.s IL_0016

	IL_0013: ldloc.0
	IL_0014: br.s IL_0018

	// c = ((score < 70) ? 'D' : c);
	IL_0016: ldc.i4.s 67

	IL_0018: stloc.0
	IL_0019: ldarg.0
	IL_001a: ldc.i4.s 70
	IL_001c: blt.s IL_0021

	IL_001e: ldloc.0
	IL_001f: br.s IL_0023

	// return (score < 60) ? 'F' : c;
	IL_0021: ldc.i4.s 68

	IL_0023: stloc.0
	IL_0024: ldarg.0
	IL_0025: ldc.i4.s 60
	IL_0027: blt.s IL_002c

	IL_0029: ldloc.0
	// (no C# code)
	IL_002a: br.s IL_002e

	IL_002c: ldc.i4.s 70

	IL_002e: stloc.0
	IL_002f: ldloc.0
	IL_0030: ret
} // end of method GradeMethods::AgileJebrim1

```

### AgileJebrim2

[Via AgileJebrim](https://twitter.com/AgileJebrim/status/1779699293697311177)

Adapted from C/C++.

```csharp
public char CheckGrade(int score)
{
    var grade = 'A';
    if (score < 90) grade = 'B';
    if (score < 80) grade = 'C';
    if (score < 70) grade = 'D';
    if (score < 60) grade = 'F';

    return grade;
}
```

```csharp
.method public hidebysig static 
	char AgileJebrim2 (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2898
	// Header size: 12
	// Code size: 37 (0x25)
	.maxstack 2
	.locals init (
		[0] char grade
	)

	// char result = 'A';
	IL_0000: ldc.i4.s 65
	IL_0002: stloc.0
	// if (score < 90)
	IL_0003: ldarg.0
	IL_0004: ldc.i4.s 90
	IL_0006: bge.s IL_000b

	// result = 'B';
	IL_0008: ldc.i4.s 66
	IL_000a: stloc.0

	// if (score < 80)
	IL_000b: ldarg.0
	IL_000c: ldc.i4.s 80
	IL_000e: bge.s IL_0013

	// result = 'C';
	IL_0010: ldc.i4.s 67
	IL_0012: stloc.0

	// if (score < 70)
	IL_0013: ldarg.0
	IL_0014: ldc.i4.s 70
	IL_0016: bge.s IL_001b

	// result = 'D';
	IL_0018: ldc.i4.s 68
	IL_001a: stloc.0

	// if (score < 60)
	IL_001b: ldarg.0
	IL_001c: ldc.i4.s 60
	IL_001e: bge.s IL_0023

	// result = 'F';
	IL_0020: ldc.i4.s 70
	IL_0022: stloc.0

	// return result;
	IL_0023: ldloc.0
	IL_0024: ret
} // end of method GradeMethods::AgileJebrim2

```

### AgileJebrim3

[Via AgileJebrim](https://twitter.com/AgileJebrim/status/1779699293697311177)

Adapted from C/C++. 

```csharp
public char CheckGrade(int score)
{           
    if (score < 60) return 'F';
    if (score < 70) return 'D';
    if (score < 80) return 'C';
    if (score < 90) return 'B';

    return 'A';
}
```

```csharp
.method public hidebysig static 
	char AgileJebrim3 (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x28c9
	// Header size: 1
	// Code size: 35 (0x23)
	.maxstack 8

	// if (score < 60)
	IL_0000: ldarg.0
	IL_0001: ldc.i4.s 60
	IL_0003: bge.s IL_0008

	// return 'F';
	IL_0005: ldc.i4.s 70
	IL_0007: ret

	// if (score < 70)
	IL_0008: ldarg.0
	IL_0009: ldc.i4.s 70
	IL_000b: bge.s IL_0010

	// return 'D';
	IL_000d: ldc.i4.s 68
	IL_000f: ret

	// if (score < 80)
	IL_0010: ldarg.0
	IL_0011: ldc.i4.s 80
	IL_0013: bge.s IL_0018

	// return 'C';
	IL_0015: ldc.i4.s 67
	IL_0017: ret

	// if (score < 90)
	IL_0018: ldarg.0
	IL_0019: ldc.i4.s 90
	IL_001b: bge.s IL_0020

	// return 'B';
	IL_001d: ldc.i4.s 66
	IL_001f: ret

	// return 'A';
	IL_0020: ldc.i4.s 65
	IL_0022: ret
} // end of method GradeMethods::AgileJebrim3

```


### amohr1

[Via amohr](https://twitter.com/amohr/status/1779788415527256172)

Adapted from C/C++.

```csharp
public char CheckGrade(int score)
{
    return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDDDDDDCCCCCCCCCCBBBBBBBBBBAAAAAAAAAAA"[score];
}
```

```csharp
.method public hidebysig static 
	char amohr1 (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x28ed
	// Header size: 1
	// Code size: 12 (0xc)
	.maxstack 8

	// return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDDDDDDCCCCCCCCCCBBBBBBBBBBAAAAAAAAAAA"[score];
	IL_0000: ldstr "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDDDDDDCCCCCCCCCCBBBBBBBBBBAAAAAAAAAAA"
	IL_0005: ldarg.0
	IL_0006: call instance char [System.Runtime]System.String::get_Chars(int32)
	IL_000b: ret
} // end of method GradeMethods::amohr1

```

### amohr2

[Via amohr](https://twitter.com/amohr/status/1780099051146621019)

Adapted from C/C++.

```csharp
public char CheckGrade(int score)
{
    return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDCCCCCBBBBBAAAAAAF"[score >> 1];
}
```

```csharp
.method public hidebysig static 
	char amohr2 (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x28fa
	// Header size: 1
	// Code size: 14 (0xe)
	.maxstack 8

	// return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDCCCCCBBBBBAAAAAAF"[score >> 1];
	IL_0000: ldstr "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDCCCCCBBBBBAAAAAAF"
	IL_0005: ldarg.0
	IL_0006: ldc.i4.1
	IL_0007: shr
	IL_0008: call instance char [System.Runtime]System.String::get_Chars(int32)
	IL_000d: ret
} // end of method GradeMethods::amohr2

```

### FreyaHolmer

[Via FreyaHolmer](https://twitter.com/FreyaHolmer/status/1779793317275808238)

```csharp
public char CheckGrade(int score)
{
    return "FFFFFFDCBAA"[score/10];
}
```

```csharp
.method public hidebysig static 
	char FreyaHolmer (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2909
	// Header size: 1
	// Code size: 15 (0xf)
	.maxstack 8

	// return "FFFFFFDCBAA"[score / 10];
	IL_0000: ldstr "FFFFFFDCBAA"
	IL_0005: ldarg.0
	IL_0006: ldc.i4.s 10
	IL_0008: div
	IL_0009: call instance char [System.Runtime]System.String::get_Chars(int32)
	IL_000e: ret
} // end of method GradeMethods::FreyaHolmer

```

### iquilezles

[Via iquilezles](https://twitter.com/iquilezles/status/1779732569032114466)


```csharp
public char CheckGrade(int score)
{
    return "FFFFFFDCBA"[((score << 4) + (score << 3) + score + 54) >> 8];
}

```

```csharp
.method public hidebysig static 
	char iquilezles (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2919
	// Header size: 1
	// Code size: 25 (0x19)
	.maxstack 8

	// return "FFFFFFDCBA"[(score << 4) + (score << 3) + score + 54 >> 8];
	IL_0000: ldstr "FFFFFFDCBA"
	IL_0005: ldarg.0
	IL_0006: ldc.i4.4
	IL_0007: shl
	IL_0008: ldarg.0
	IL_0009: ldc.i4.3
	IL_000a: shl
	IL_000b: add
	IL_000c: ldarg.0
	IL_000d: add
	IL_000e: ldc.i4.s 54
	IL_0010: add
	IL_0011: ldc.i4.8
	IL_0012: shr
	IL_0013: call instance char [System.Runtime]System.String::get_Chars(int32)
	IL_0018: ret
} // end of method GradeMethods::iquilezles

```

### nthnblair

[Via nthnblair](https://twitter.com/nthnblair/status/1779979327251341820)

Adapted from C/C++.

```csharp
public char CheckGrade(int score)
{
    int x = (((score << 4) + (score << 3) + score + 54) >> 8) - 6;
    return (char)(68 - (x & (~(x >> 4))) + ((x >> 4) & 2));
}
```

```csharp
.method public hidebysig static 
	char nthnblair (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2934
	// Header size: 12
	// Code size: 34 (0x22)
	.maxstack 4
	.locals init (
		[0] int32 x
	)

	// int num = ((score << 4) + (score << 3) + score + 54 >> 8) - 6;
	IL_0000: ldarg.0
	IL_0001: ldc.i4.4
	IL_0002: shl
	IL_0003: ldarg.0
	IL_0004: ldc.i4.3
	IL_0005: shl
	IL_0006: add
	IL_0007: ldarg.0
	IL_0008: add
	IL_0009: ldc.i4.s 54
	IL_000b: add
	IL_000c: ldc.i4.8
	IL_000d: shr
	IL_000e: ldc.i4.6
	IL_000f: sub
	IL_0010: stloc.0
	// return (char)(68 - (num & ~(num >> 4)) + ((num >> 4) & 2));
	IL_0011: ldc.i4.s 68
	IL_0013: ldloc.0
	IL_0014: ldloc.0
	IL_0015: ldc.i4.4
	IL_0016: shr
	IL_0017: not
	IL_0018: and
	IL_0019: sub
	IL_001a: ldloc.0
	IL_001b: ldc.i4.4
	IL_001c: shr
	IL_001d: ldc.i4.2
	IL_001e: and
	IL_001f: add
	IL_0020: conv.u2
	IL_0021: ret
} // end of method GradeMethods::nthnblair

```

### xsphi

[Via xsphi](https://twitter.com/xsphi/status/1779701103099318386)

Adapted from C/C++. It seems `printf()` with `%c` only works in the ASCII range so the C# version has `% 128`.

```csharp
public char CheckGrade(int score)
{           
    return (char)((((74 - score / 10) | 1179009280) >> (((score - 60) >> 2) & 24)) % 128);
}
```

```csharp
.method public hidebysig static 
	char xsphi (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2962
	// Header size: 1
	// Code size: 34 (0x22)
	.maxstack 8

	// return (char)((((74 - score / 10) | 0x46464100) >> ((score - 60 >> 2) & 0x18)) % 128);
	IL_0000: ldc.i4.s 74
	IL_0002: ldarg.0
	IL_0003: ldc.i4.s 10
	IL_0005: div
	IL_0006: sub
	IL_0007: ldc.i4 1179009280
	IL_000c: or
	IL_000d: ldarg.0
	IL_000e: ldc.i4.s 60
	IL_0010: sub
	IL_0011: ldc.i4.2
	IL_0012: shr
	IL_0013: ldc.i4.s 24
	IL_0015: and
	// (no C# code)
	IL_0016: ldc.i4.s 31
	IL_0018: and
	IL_0019: shr
	IL_001a: ldc.i4 128
	IL_001f: rem
	IL_0020: conv.u2
	IL_0021: ret
} // end of method GradeMethods::xsphi

```


### Switch Expression

My favourite and the way I'm most comfortable with.

```csharp
public string CheckGrade(int score) => score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "F"
};
```

```csharp
.method public hidebysig static 
	string SwitchExpression (
		int32 score
	) cil managed 
{
	// Method begins at RVA 0x2988
	// Header size: 12
	// Code size: 64 (0x40)
	.maxstack 2
	.locals init (
		[0] string
	)

	// if (score >= 70)
	IL_0000: ldarg.0
	IL_0001: ldc.i4.s 70
	IL_0003: blt.s IL_0011

	// if (score < 90)
	IL_0005: ldarg.0
	IL_0006: ldc.i4.s 90
	IL_0008: bge.s IL_0018

	// if (score >= 80)
	IL_000a: ldarg.0
	IL_000b: ldc.i4.s 80
	IL_000d: bge.s IL_0020

	// if (score >= 60)
	IL_000f: br.s IL_0028

	IL_0011: ldarg.0
	IL_0012: ldc.i4.s 60
	IL_0014: bge.s IL_0030

	// return "A";
	IL_0016: br.s IL_0038

	IL_0018: ldstr "A"
	IL_001d: stloc.0
	// return "B";
	IL_001e: br.s IL_003e

	IL_0020: ldstr "B"
	IL_0025: stloc.0
	// return "C";
	IL_0026: br.s IL_003e

	IL_0028: ldstr "C"
	IL_002d: stloc.0
	// return "D";
	IL_002e: br.s IL_003e

	IL_0030: ldstr "D"
	IL_0035: stloc.0
	// return "F";
	IL_0036: br.s IL_003e

	IL_0038: ldstr "F"
	IL_003d: stloc.0

	// (no C# code)
	IL_003e: ldloc.0
	IL_003f: ret
} // end of method GradeMethods::SwitchExpression

```

## Benchmarks

### For an individual score

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4291/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
```

| Method           | score |      Mean |     Error |    StdDev |    Median |    Ratio | RatioSD | Allocated | Alloc Ratio |
| ---------------- | ----: | --------: | --------: | --------: | --------: | -------: | ------: | --------: | ----------: |
| Original         |    50 | 0.3680 ns | 0.0688 ns | 0.1110 ns | 0.3172 ns | baseline |         |         - |          NA |
| AgileJebrim1     |    50 | 0.3003 ns | 0.0356 ns | 0.0350 ns | 0.2889 ns |   -12.3% |   29.9% |         - |          NA |
| AgileJebrim2     |    50 | 0.0942 ns | 0.0229 ns | 0.0191 ns | 0.0877 ns |   -71.4% |   32.0% |         - |          NA |
| AgileJebrim3     |    50 | 0.0029 ns | 0.0104 ns | 0.0093 ns | 0.0000 ns |   -99.4% |  286.8% |         - |          NA |
| amohr1           |    50 | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |  -100.0% |      NA |         - |          NA |
| amohr2           |    50 | 0.2795 ns | 0.0348 ns | 0.0372 ns | 0.2709 ns |   -20.6% |   36.0% |         - |          NA |
| FreyaHolmer      |    50 | 0.0308 ns | 0.0246 ns | 0.0561 ns | 0.0000 ns |   -91.5% |  194.3% |         - |          NA |
| iquilezles       |    50 | 0.3963 ns | 0.0321 ns | 0.0578 ns | 0.3776 ns |   +17.8% |   29.7% |         - |          NA |
| nthnblair        |    50 | 0.2937 ns | 0.0280 ns | 0.0248 ns | 0.2921 ns |   -10.5% |   27.0% |         - |          NA |
| xsphi            |    50 | 0.6032 ns | 0.0399 ns | 0.0892 ns | 0.5738 ns |   +79.2% |   25.7% |         - |          NA |
| SwitchExpression |    50 | 0.8528 ns | 0.0788 ns | 0.1294 ns | 0.8376 ns |  +155.0% |   33.6% |         - |          NA |
|                  |       |           |           |           |           |          |         |           |             |
| Original         |    60 | 0.3453 ns | 0.0674 ns | 0.0899 ns | 0.3237 ns | baseline |         |         - |          NA |
| AgileJebrim1     |    60 | 0.2795 ns | 0.0196 ns | 0.0164 ns | 0.2791 ns |      -6% |   25.6% |         - |          NA |
| AgileJebrim2     |    60 | 0.0783 ns | 0.0284 ns | 0.0554 ns | 0.0690 ns |     -71% |   75.8% |         - |          NA |
| AgileJebrim3     |    60 | 0.0318 ns | 0.0310 ns | 0.0345 ns | 0.0144 ns |     -90% |  112.0% |         - |          NA |
| amohr1           |    60 | 0.2778 ns | 0.0362 ns | 0.0615 ns | 0.2509 ns |      -8% |   34.9% |         - |          NA |
| amohr2           |    60 | 0.1590 ns | 0.0153 ns | 0.0127 ns | 0.1611 ns |     -47% |   25.6% |         - |          NA |
| FreyaHolmer      |    60 | 0.3035 ns | 0.0266 ns | 0.0235 ns | 0.3027 ns |      -0% |   28.4% |         - |          NA |
| iquilezles       |    60 | 0.3595 ns | 0.0343 ns | 0.0381 ns | 0.3435 ns |     +12% |   24.6% |         - |          NA |
| nthnblair        |    60 | 0.2678 ns | 0.0230 ns | 0.0204 ns | 0.2762 ns |     -12% |   27.4% |         - |          NA |
| xsphi            |    60 | 0.6405 ns | 0.0426 ns | 0.0507 ns | 0.6273 ns |     +96% |   27.8% |         - |          NA |
| SwitchExpression |    60 | 0.5448 ns | 0.0691 ns | 0.0646 ns | 0.5483 ns |     +76% |   35.7% |         - |          NA |
|                  |       |           |           |           |           |          |         |           |             |
| Original         |    70 | 0.1242 ns | 0.0569 ns | 0.0505 ns | 0.1095 ns | baseline |         |         - |          NA |
| AgileJebrim1     |    70 | 0.2998 ns | 0.0362 ns | 0.0417 ns | 0.2999 ns |    +178% |   38.1% |         - |          NA |
| AgileJebrim2     |    70 | 0.0508 ns | 0.0274 ns | 0.0410 ns | 0.0391 ns |     -49% |  137.8% |         - |          NA |
| AgileJebrim3     |    70 | 0.2658 ns | 0.0365 ns | 0.0535 ns | 0.2599 ns |    +123% |   41.8% |         - |          NA |
| amohr1           |    70 | 0.2517 ns | 0.0355 ns | 0.0436 ns | 0.2378 ns |    +123% |   35.1% |         - |          NA |
| amohr2           |    70 | 0.2626 ns | 0.0343 ns | 0.0367 ns | 0.2502 ns |    +135% |   36.9% |         - |          NA |
| FreyaHolmer      |    70 | 0.4043 ns | 0.0384 ns | 0.0964 ns | 0.3750 ns |    +310% |   43.6% |         - |          NA |
| iquilezles       |    70 | 0.2889 ns | 0.0367 ns | 0.0539 ns | 0.2674 ns |    +185% |   32.0% |         - |          NA |
| nthnblair        |    70 | 0.3221 ns | 0.0374 ns | 0.1055 ns | 0.2877 ns |    +224% |   53.0% |         - |          NA |
| xsphi            |    70 | 0.6687 ns | 0.0435 ns | 0.0879 ns | 0.6500 ns |    +471% |   32.9% |         - |          NA |
| SwitchExpression |    70 | 0.5503 ns | 0.0705 ns | 0.0812 ns | 0.5254 ns |    +410% |   39.3% |         - |          NA |
|                  |       |           |           |           |           |          |         |           |             |
| Original         |    80 | 0.2859 ns | 0.0347 ns | 0.0290 ns | 0.2796 ns | baseline |         |         - |          NA |
| AgileJebrim1     |    80 | 0.3375 ns | 0.0377 ns | 0.0850 ns | 0.3014 ns |     +41% |   25.9% |         - |          NA |
| AgileJebrim2     |    80 | 0.0355 ns | 0.0252 ns | 0.0223 ns | 0.0413 ns |     -87% |   58.3% |         - |          NA |
| AgileJebrim3     |    80 | 0.2387 ns | 0.0157 ns | 0.0131 ns | 0.2374 ns |     -16% |   11.9% |         - |          NA |
| amohr1           |    80 | 0.2053 ns | 0.0187 ns | 0.0175 ns | 0.1987 ns |     -27% |   11.8% |         - |          NA |
| amohr2           |    80 | 0.2348 ns | 0.0114 ns | 0.0095 ns | 0.2358 ns |     -17% |   10.2% |         - |          NA |
| FreyaHolmer      |    80 | 0.3287 ns | 0.0162 ns | 0.0135 ns | 0.3258 ns |     +16% |   10.5% |         - |          NA |
| iquilezles       |    80 | 0.3298 ns | 0.0189 ns | 0.0177 ns | 0.3277 ns |     +17% |   13.1% |         - |          NA |
| nthnblair        |    80 | 0.2711 ns | 0.0098 ns | 0.0087 ns | 0.2702 ns |      -4% |   11.0% |         - |          NA |
| xsphi            |    80 | 0.6249 ns | 0.0416 ns | 0.0428 ns | 0.6192 ns |    +124% |   12.8% |         - |          NA |
| SwitchExpression |    80 | 0.4340 ns | 0.0258 ns | 0.0215 ns | 0.4310 ns |     +53% |    9.6% |         - |          NA |
|                  |       |           |           |           |           |          |         |           |             |
| Original         |    90 | 0.3149 ns | 0.0218 ns | 0.0194 ns | 0.3137 ns | baseline |         |         - |          NA |
| AgileJebrim1     |    90 | 0.2839 ns | 0.0116 ns | 0.0103 ns | 0.2846 ns |      -9% |    8.2% |         - |          NA |
| AgileJebrim2     |    90 | 0.0603 ns | 0.0172 ns | 0.0161 ns | 0.0555 ns |     -81% |   26.0% |         - |          NA |
| AgileJebrim3     |    90 | 0.2493 ns | 0.0148 ns | 0.0138 ns | 0.2484 ns |     -20% |    7.7% |         - |          NA |
| amohr1           |    90 | 0.3671 ns | 0.0361 ns | 0.0738 ns | 0.3509 ns |     +30% |   25.2% |         - |          NA |
| amohr2           |    90 | 0.2433 ns | 0.0156 ns | 0.0130 ns | 0.2449 ns |     -22% |    7.2% |         - |          NA |
| FreyaHolmer      |    90 | 0.3341 ns | 0.0157 ns | 0.0139 ns | 0.3371 ns |      +6% |    6.7% |         - |          NA |
| iquilezles       |    90 | 0.3215 ns | 0.0204 ns | 0.0181 ns | 0.3154 ns |      +2% |    8.2% |         - |          NA |
| nthnblair        |    90 | 0.2488 ns | 0.0209 ns | 0.0196 ns | 0.2412 ns |     -21% |    9.4% |         - |          NA |
| xsphi            |    90 | 0.6218 ns | 0.0393 ns | 0.0349 ns | 0.6063 ns |     +98% |    9.6% |         - |          NA |
| SwitchExpression |    90 | 0.4245 ns | 0.0211 ns | 0.0177 ns | 0.4278 ns |     +36% |    6.7% |         - |          NA |

### Looping through many random scores

Has loop overhead too. I assume timings will also depend on the distribution of scores too within the random set.

| Method           | No. of scores |          Mean |         Error |        StdDev |        Median |    Ratio | RatioSD | Allocated | Alloc Ratio |
| ---------------- | ------------: | ------------: | ------------: | ------------: | ------------: | -------: | ------: | --------: | ----------: |
| Original         |           100 |      38.65 ns |      0.423 ns |      0.353 ns |      38.59 ns | baseline |         |         - |          NA |
| AgileJebrim1     |           100 |      38.89 ns |      0.686 ns |      0.642 ns |      38.91 ns |      +1% |    1.8% |         - |          NA |
| AgileJebrim2     |           100 |      39.08 ns |      0.596 ns |      0.529 ns |      38.84 ns |      +1% |    2.1% |         - |          NA |
| AgileJebrim3     |           100 |      39.38 ns |      0.530 ns |      0.496 ns |      39.23 ns |      +2% |    1.8% |         - |          NA |
| amohr1           |           100 |      52.73 ns |      1.074 ns |      1.005 ns |      52.76 ns |     +36% |    2.2% |         - |          NA |
| amohr2           |           100 |      55.07 ns |      0.971 ns |      0.908 ns |      54.95 ns |     +43% |    1.8% |         - |          NA |
| FreyaHolmer      |           100 |      98.28 ns |      1.570 ns |      1.468 ns |      97.92 ns |    +154% |    1.7% |         - |          NA |
| iquilezles       |           100 |      82.50 ns |      1.542 ns |      1.443 ns |      82.10 ns |    +113% |    1.9% |         - |          NA |
| nthnblair        |           100 |      41.01 ns |      0.846 ns |      0.905 ns |      41.06 ns |      +6% |    2.5% |         - |          NA |
| xsphi            |           100 |      38.53 ns |      0.338 ns |      0.299 ns |      38.58 ns |      -0% |    1.3% |         - |          NA |
| SwitchExpression |           100 |      39.11 ns |      0.707 ns |      0.661 ns |      39.24 ns |      +1% |    2.0% |         - |          NA |
|                  |               |               |               |               |               |          |         |           |             |
| Original         |        100000 |  23,482.96 ns |    317.061 ns |    281.067 ns |  23,365.05 ns | baseline |         |         - |          NA |
| AgileJebrim1     |        100000 |  23,501.67 ns |    422.242 ns |    374.307 ns |  23,419.52 ns |      +0% |    1.5% |         - |          NA |
| AgileJebrim2     |        100000 |  23,641.14 ns |    472.756 ns |    562.783 ns |  23,623.05 ns |      +0% |    2.6% |         - |          NA |
| AgileJebrim3     |        100000 |  24,609.60 ns |    489.159 ns |  1,330.793 ns |  24,155.50 ns |      +2% |    4.7% |         - |          NA |
| amohr1           |        100000 |  48,115.36 ns |    931.482 ns |  1,394.198 ns |  48,074.59 ns |    +103% |    2.3% |         - |          NA |
| amohr2           |        100000 |  47,138.44 ns |    907.736 ns |    758.000 ns |  47,095.05 ns |    +101% |    1.9% |         - |          NA |
| FreyaHolmer      |        100000 |  94,876.05 ns |  1,815.093 ns |  1,609.033 ns |  94,912.68 ns |    +304% |    1.5% |         - |          NA |
| iquilezles       |        100000 |  78,286.16 ns |  1,529.539 ns |  2,144.200 ns |  77,462.46 ns |    +237% |    2.9% |         - |          NA |
| nthnblair        |        100000 |  25,049.40 ns |    500.286 ns |  1,273.387 ns |  24,902.00 ns |      +8% |    6.3% |         - |          NA |
| xsphi            |        100000 |  24,763.69 ns |    459.065 ns |  1,117.426 ns |  24,466.72 ns |      +7% |    5.3% |         - |          NA |
| SwitchExpression |        100000 |  24,053.18 ns |    460.477 ns |    614.723 ns |  23,926.56 ns |      +3% |    3.5% |         - |          NA |
|                  |               |               |               |               |               |          |         |           |             |
| Original         |       1000000 | 246,810.88 ns |  4,848.635 ns | 11,984.625 ns | 243,528.31 ns | baseline |         |         - |          NA |
| AgileJebrim1     |       1000000 | 240,626.27 ns |  4,577.578 ns |  4,700.836 ns | 240,331.79 ns |      -3% |    6.4% |         - |          NA |
| AgileJebrim2     |       1000000 | 242,684.72 ns |  4,825.116 ns |  8,193.413 ns | 240,967.87 ns |      -2% |    5.6% |         - |          NA |
| AgileJebrim3     |       1000000 | 244,711.63 ns |  4,869.819 ns |  8,656.097 ns | 241,423.55 ns |      -2% |    6.8% |         - |          NA |
| amohr1           |       1000000 | 487,972.61 ns |  8,242.155 ns |  6,882.573 ns | 485,930.08 ns |     +95% |    6.1% |         - |          NA |
| amohr2           |       1000000 | 512,273.99 ns | 10,196.677 ns | 20,829.109 ns | 506,680.27 ns |    +106% |    6.5% |         - |          NA |
| FreyaHolmer      |       1000000 | 999,286.61 ns | 19,839.991 ns | 45,982.197 ns | 989,788.82 ns |    +304% |    5.9% |         - |          NA |
| iquilezles       |       1000000 | 807,265.25 ns | 16,127.205 ns | 25,579.457 ns | 806,156.45 ns |    +224% |    6.0% |       1 B |          NA |
| nthnblair        |       1000000 | 245,396.44 ns |  4,868.947 ns |  9,496.506 ns | 241,739.34 ns |      -1% |    6.5% |         - |          NA |
| xsphi            |       1000000 | 243,827.25 ns |  4,851.874 ns | 10,019.962 ns | 240,789.77 ns |      -2% |    5.4% |         - |          NA |
| SwitchExpression |       1000000 | 244,043.04 ns |  4,358.940 ns |  6,913.741 ns | 242,722.71 ns |      -2% |    6.4% |         - |          NA |