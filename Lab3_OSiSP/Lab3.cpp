// Lab3.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>


int _tmain(int argc, _TCHAR* argv[])
{
	WCHAR * mapName = (WCHAR * )"mapfile";
	HANDLE mapfile = OpenFileMapping(FILE_MAP_ALL_ACCESS, false, mapName);
	if (mapfile == NULL)
	{
		mapfile = CreateFileMapping(NULL, NULL, PAGE_READWRITE, 0, 256, mapName);
		void * addressMap = MapViewOfFile(mapfile, FILE_MAP_ALL_ACCESS, 0, 0, 255);
		char * text = (char *)malloc(256);
		gets_s(text, 256);
		CopyMemory(addressMap, text, 256);
        UnmapViewOfFile(addressMap);
	}
	else
	{
		void * addressMap = MapViewOfFile(mapfile, FILE_MAP_ALL_ACCESS, 0, 0, 255);
		char * text = (char *)malloc(256);
		CopyMemory(text, addressMap, 256);
		printf_s("%s\n", text);
        UnmapViewOfFile(addressMap);
	}
	Sleep(20000);
	return 0;
}

