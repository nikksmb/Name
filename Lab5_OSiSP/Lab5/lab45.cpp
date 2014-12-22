// Lab4.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "lab45.h"
#include "../RedBlackTree/RedBlackTreeLibrary.h"

#define MAX_LOADSTRING 100
#define SURNAME_BUTTON 200
#define ADRESS_BUTTON 201
#define TELEPHONE_BUTTON 202
#define RESULT_BUTTON 203
#define SURNAME_EDIT 204
#define ADRESS_EDIT 205
#define TELEPHONE_EDIT 206

typedef void(*INITIALIZE_TREES)();
typedef void(*INITIALIZE_MEMORY)();
typedef void(*FINDSURNAME)(std::string *, HWND);
typedef void(*FINDADRESS)(char[], HWND);
typedef void(*FINDTELEPHONE)(char[], HWND);

// Global Variables:
HINSTANCE hInst;								// current instance
HWND hWnd;
HWND hSurnameLabel;
HWND hTelephoneLabel;
HWND hAdressLabel;
HWND hSurnameEdit;
HWND hTelephoneEdit;
HWND hAdressEdit;
HWND hResultEdit;
HWND hFindSurnameButton;
HWND hFindAdressButton;
HWND hFindTelephoneButton;
HWND hFindAllButton;
HWND hSurnameCheckBox;
HWND hAdressCheckBox;
HWND hTelephoneCheckBox;
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name
HINSTANCE DLL;
char** stringArray;
bool baseLoaded;
FINDSURNAME FindSurnames;
FINDADRESS FindAdresses;
FINDTELEPHONE FindTelephones;
INITIALIZE_TREES Initialize_Trees;
INITIALIZE_MEMORY Initialize_Memory;

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
void				AddText(HWND hEdit, char* text);

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPTSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_OSISP4, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_OSISP4));
	baseLoaded = false;
	DLL = LoadLibrary(L"e:\\5 ñåì\\ÎÑèÑÏ\\lab45\\lab45\\Debug\\RedBlackTree.dll");
	FindSurnames = (FINDSURNAME)GetProcAddress(DLL, "FindSurname");
	FindAdresses = (FINDADRESS)GetProcAddress(DLL, "FindAdress");
	FindTelephones = (FINDTELEPHONE)GetProcAddress(DLL, "FindTelephone");
	Initialize_Memory = (INITIALIZE_MEMORY)GetProcAddress(DLL, "InitializeMemory");
	//Initialize_Trees = (INITIALIZE_TREES)GetProcAddress(DLL, "InitializeTrees");
	Initialize_Memory();
	//Initialize_Trees();
	
	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int)msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_OSISP4));
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW);
	wcex.lpszMenuName = MAKEINTRESOURCE(IDC_OSISP4);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{


	hInst = hInstance; // Store instance handle in our global variable

	hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);


	int x, w, y, h;
	y = 10; h = 20;
	x = 30; w = 90;


	hSurnameLabel = CreateWindow(L"static", L"hSurnameLabel", WS_CHILD | WS_VISIBLE, x, y, w, h, hWnd, NULL, hInstance, NULL);
	SetWindowText(hSurnameLabel, L"Surname:");

	hSurnameEdit = CreateWindowEx(WS_EX_CLIENTEDGE, L"edit", L"hSurnameEdit", WS_CHILD | WS_VISIBLE | ES_LEFT,
		x + 100, y, w + 150, h, hWnd, (HMENU)SURNAME_EDIT, hInstance, NULL);
	SetWindowText(hSurnameEdit, L"");

	hFindSurnameButton = CreateWindow(L"button", L"hFindSurnameButton", WS_CHILD | WS_VISIBLE,
		x + 350, y, w + 90, h, hWnd, (HMENU)SURNAME_BUTTON, hInstance, NULL);
	SetWindowText(hFindSurnameButton, L"Find Surname");

	y = 40;


	hAdressLabel = CreateWindow(L"static", L"hAdressLabel", WS_CHILD | WS_VISIBLE, x, y, w, h, hWnd, NULL, hInstance, NULL);
	SetWindowText(hAdressLabel, L"Adress:");

	hAdressEdit = CreateWindowEx(WS_EX_CLIENTEDGE, L"edit", L"hAdressEdit", WS_CHILD | WS_VISIBLE | ES_LEFT,
		x + 100, y, w + 200, h, hWnd, (HMENU)ADRESS_EDIT, hInstance, NULL);
	SetWindowText(hAdressEdit, L"");

	hFindAdressButton = CreateWindow(L"button", L"hFindAdressButton", WS_CHILD | WS_VISIBLE,
		x + 400, y, w + 90, h, hWnd, (HMENU)ADRESS_BUTTON, hInstance, NULL);
	SetWindowText(hFindAdressButton, L"Find Adress");

	y = 70;


	hTelephoneLabel = CreateWindow(L"static", L"hTelephoneLabel", WS_CHILD | WS_VISIBLE, x, y, w, h, hWnd, NULL, hInstance, NULL);
	SetWindowText(hTelephoneLabel, L"Telephone:");

	hTelephoneEdit = CreateWindowEx(WS_EX_CLIENTEDGE, L"edit", L"hTelephoneEdit", WS_CHILD | WS_VISIBLE | ES_LEFT,
		x + 100, y, w + 50, h, hWnd, (HMENU)TELEPHONE_EDIT, hInstance, NULL);
	SetWindowText(hTelephoneEdit, L"");

	hFindTelephoneButton = CreateWindow(L"button", L"hFindTelephoneButton", WS_CHILD | WS_VISIBLE,
		x + 250, y, w + 90, h, hWnd, (HMENU)TELEPHONE_BUTTON, hInstance, NULL);
	SetWindowText(hFindTelephoneButton, L"Find Telephone");

	y = 100;

	hResultEdit = CreateWindowEx(WS_EX_CLIENTEDGE, L"edit", L"hResultEdit", WS_CHILD | WS_VISIBLE | ES_LEFT | ES_MULTILINE | ES_AUTOVSCROLL,
		x - 20, y, 800, 200, hWnd, (HMENU)TELEPHONE_EDIT, hInstance, NULL);
	SendMessage(hResultEdit, EM_SETLIMITTEXT, -1, NULL);
	SetWindowText(hResultEdit, L"");

	if (!hWnd)
	{
		return FALSE;
	}
	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		char * input;
		std::string * str;
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;

		case SURNAME_BUTTON:
			input = new char[30];
			str = new std::string[8];
			GetWindowTextA(hSurnameEdit, input, 30);
			for (int i = 0; i<30; i++)
			{
				str[1].push_back(input[i]);
				//str[1].append(input[i]);
				//std::strcpy(str[1],input);
				//str[1][i] = input[i];
			}
			GetWindowTextA(hAdressEdit, input, 30);
			for (int i = 0; i<30; i++)
			{
				str[4].push_back(input[i]);
				//str[4][i] = input[i];
			}
			GetWindowTextA(hTelephoneEdit, input, 30);
			for (int i = 0; i<30; i++)
			{
				str[0].push_back(input[i]);
			//	str[0][i] = input[i];
			}
			str[2].clear();
			str[3].clear();
			str[5].clear();
			str[6].clear();
			str[7].clear();
		//	str[0].c_str() = input;
			SetWindowText(hResultEdit, L"");
			FindSurnames(str, hResultEdit);
			break;
		case ADRESS_BUTTON:
			input = new char[30];
			GetWindowTextA(hAdressEdit, input, 30);
			SetWindowText(hResultEdit, L"");
			FindAdresses(input, hResultEdit);
			break;
		case TELEPHONE_BUTTON:
			input = new char[30];
			GetWindowTextA(hTelephoneEdit, input, 30);
			SetWindowText(hResultEdit, L"");
			FindTelephones(input, hResultEdit);
			break;

		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		FreeLibrary(DLL);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}



// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
