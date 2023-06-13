#include "resource.h"
#include <windows.h>

struct TAGDESCR
{
	const wchar_t* Name;
	VARTYPE Type;
	DWORD Rights; 
	int Min;
	int Max;
	DWORD Flags;
	const wchar_t* Descr;
};

// Flags values
#define GBS_FUNC_MASK	0x0000ffff
#define GBS_ALG_MASK	0x000000ff
#define GBS_SAW			0x00000001
#define GBS_SIN			0x00000002
#define GBS_TRI			0x00000003
#define GBS_SQR			0x00000004
#define GBS_RND			0x00000005
#define GBS_TXTRND		0x00000006
#define GBS_TXTCOLOR	0x00000007
#define GBS_TXTWEEK		0x00000008
#define GBS_TXTNUMBER	0x00000009
#define GBS_CURTIME		0x0000000a
#define GBS_RNDTIME		0x0000000b
#define GBS_REG			0x0000000c
#define GBS_BAND		0x0000000d
#define GBS_ECOLOR		0x0000000e
#define GBS_ENUMBER		0x0000000f
#define GBS_EWEEK		0x00000010
#define GBS_ATTR_MASK	0x0000ff00
#define GBS_OPTFREQ		0x00000100
#define GBS_PROP_MASK	0x00ff0000
#define GBS_FREQ		0x00010000
#define GBS_EU			0x00020000

static const wchar_t* TagWeekday[] =
{
	L"Sunday",
	L"Monday",
	L"Tuesday",
	L"Wednesday",
	L"Thursday",
	L"Friday",
	L"Saturday"
};

static const wchar_t* TagColor[] =
{
	L"White",
	L"Black",
	L"Red",
	L"Blue",
	L"Yellow",
	L"Green",
	L"Brown",
	L"Pink"
};

static const wchar_t* TagNumber[] =
{
	L"Zero",
	L"One",
	L"Two",
	L"Three",
	L"Four",
	L"Five",
	L"Six",
	L"Seven",
	L"Eight",
	L"Nine"
};

static const TAGDESCR TagDescr[] =
{
	{L"options.sawfreq",		VT_R8,		3, 0, 0, GBS_SAW|GBS_OPTFREQ|GBS_EU,	L"Saw waves frequency"},
	{L"options.sinfreq",		VT_R8,		3, 0, 0, GBS_SIN|GBS_OPTFREQ|GBS_EU,	L"Harmonic waves frequency"},
	{L"options.trianglefreq",	VT_R8,		3, 0, 0, GBS_TRI|GBS_OPTFREQ|GBS_EU,	L"Tiangle waves frequency"},
	{L"options.sqaurefreq",		VT_R8,		3, 0, 0, GBS_SQR|GBS_OPTFREQ|GBS_EU,	L"Square waves frequency"},
	{L"numeric.saw.uint8",		VT_UI1,		1, 0, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.int8",		VT_I2,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.uint16",		VT_UI2,		1, 0, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.int16",		VT_I2,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.uint32",		VT_UI4,		1, 0, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.int32",		VT_I4,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.uint64",		VT_UI8,		1, 0, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.int64",		VT_I8,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.float",		VT_R4,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.saw.double",		VT_R8,		1, -100, 100, GBS_SAW|GBS_FREQ,		L"Saw wave"},
	{L"numeric.sin.uint8",		VT_UI1,		1, 0, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.int8",		VT_I1,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.uint16",		VT_UI2,		1, 0, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.int16",		VT_I2,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.uint32",		VT_UI4,		1, 0, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.int32",		VT_I4,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.uint64",		VT_UI8,		1, 0, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.int64",		VT_I8,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.float",		VT_R4,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.sin.double",		VT_R8,		1, -100, 100, GBS_SIN|GBS_FREQ,		L"Harmonic wave"},
	{L"numeric.triangle.uint8",	VT_UI1,		1, 0, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.int8",	VT_I1,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.uint16",VT_UI2,		1, 0, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.int16",	VT_I2,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.uint32",VT_UI4,		1, 0, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.int32",	VT_I4,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.uint64",VT_UI8,		1, 0, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.int64",	VT_I8,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.float",	VT_R4,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.triangle.double",VT_R8,		1, -100, 100, GBS_TRI|GBS_FREQ,		L"Triangle wave"},
	{L"numeric.square.uint8",	VT_UI1,		1, 0, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.int8",	VT_I2,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.uint16",	VT_UI2,		1, 0, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.int16",	VT_I2,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.uint32",	VT_UI4,		1, 0, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.int32",	VT_I4,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.uint64",	VT_UI8,		1, 0, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.int64",	VT_I8,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.float",	VT_R4,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.double",	VT_R8,		1, -100, 100, GBS_SQR|GBS_FREQ,		L"Square wave"},
	{L"numeric.square.bool",	VT_BOOL,	1, 0, 0, GBS_SQR|GBS_FREQ,			L"Square wave"},
	{L"numeric.random.uint8",	VT_UI1,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.int8",	VT_I1,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.uint16",	VT_UI2,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.int16",	VT_I2,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.uint32",	VT_UI4,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.int32",	VT_I4,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.uint64",	VT_UI8,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.int64",	VT_I8,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.float",	VT_R4,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.double",	VT_R8,		1, 0, 0, GBS_RND,					L"Random number"},
	{L"numeric.random.bool",	VT_BOOL,	1, 0, 0, GBS_RND,					L"Random number"},
	{L"textual.color",			VT_BSTR,	1, 0, 0, GBS_TXTCOLOR,				L"Random color"},
	{L"textual.number",			VT_BSTR,	1, 0, 0, GBS_TXTNUMBER,				L"Random number (textual)"},
	{L"textual.random",			VT_BSTR,	1, 0, 0, GBS_TXTRND,				L"Random string"},
	{L"textual.weekday",		VT_BSTR,	1, 0, 0, GBS_TXTWEEK,				L"Random weekday"},
	{L"time.current",			VT_DATE,	1, 0, 0, GBS_CURTIME,				L"Current date and time (UTC)"},
	{L"time.random",			VT_DATE,	1, 0, 0, GBS_RNDTIME,				L"Random date and time"},
	{L"bandwidth",				VT_UI4,		1, 0, 0, GBS_BAND,					L"Server bandwidth"},
	{L"enum.color",				VT_I4,		3, 0, 0, GBS_ECOLOR,				L"Colors enumeration"},
	{L"enum.number",			VT_I4,		3, 0, 0, GBS_ENUMBER,				L"Numbers enumeration"},
	{L"enum.weekday",			VT_I4,		3, 0, 0, GBS_EWEEK,					L"Weekdays enumeration"},
	{L"storage.numeric.reg01",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg02",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg03",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg04",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg05",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg06",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg07",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.numeric.reg08",	VT_R8,		3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg01",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg02",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg03",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg04",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg05",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg06",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg07",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.bool.reg08",		VT_BOOL,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg01",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg02",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg03",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg04",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg05",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg06",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg07",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.string.reg08",	VT_BSTR,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg01",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg02",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg03",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg04",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg05",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg06",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg07",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
	{L"storage.time.reg08",		VT_DATE,	3, 0, 0, GBS_REG,					L"Storage register"},
};