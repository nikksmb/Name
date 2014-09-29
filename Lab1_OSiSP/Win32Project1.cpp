// Win32Project1.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "Win32Project1.h"
#include <commdlg.h>
#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name



// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
BOOL GetPenColor(HWND hwnd, COLORREF *clrref);

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
	LoadString(hInstance, IDC_WIN32PROJECT1, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WIN32PROJECT1));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
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

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WIN32PROJECT1));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_WIN32PROJECT1);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

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
   HWND hWnd;

   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

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
	RECT r;
//	HDC hdc;
	static HDC hdcT;
	static int x,y,xEx,yEx;
	static LPPOINT PointEx;
	static int Tmp;
	static int MouseDown=0;
	static int shape;
	static int PenWidth;
	HPEN pen;
	HBITMAP bufBitmap1,bufBitmap2,bufBitmapu,bufBitmapd;
	//RECT screen;
	HBRUSH brush;
	static COLORREF clr;
	static HDC hdc1,hdc2,hdc,hdcu,hdcd;
	static int scrbot,scrright;
	switch (message)
	{
	case WM_CREATE:
		pen = CreatePen(PS_SOLID,1,RGB(0,0,0));
		hdc=GetDC(hWnd);
		scrright = GetDeviceCaps(hdc, HORZRES);
		scrbot = GetDeviceCaps(hdc, VERTRES);
		hdc1=CreateCompatibleDC(hdc);
		hdc2=CreateCompatibleDC(hdc);
		hdcu=CreateCompatibleDC(hdc);
		hdcd=CreateCompatibleDC(hdc);
		brush=(HBRUSH)CreateSolidBrush(RGB(255, 255, 255));
		bufBitmap1=CreateCompatibleBitmap(hdc,scrright,scrbot);
		bufBitmap2=CreateCompatibleBitmap(hdc,scrright,scrbot);
		bufBitmapu=CreateCompatibleBitmap(hdc,scrright,scrbot);
		bufBitmapd=CreateCompatibleBitmap(hdc,scrright,scrbot);
		SelectObject(hdc1,bufBitmap1);
		SelectObject(hdc2,bufBitmap2);
		SelectObject(hdcu,bufBitmapu);
		SelectObject(hdcd,bufBitmapd);
	//	SelectObject(hdc1,brush);
	//	SelectObject(hdc2,brush);
		r.left = 0;
		r.top = 0;
		r.right = scrright;
		r.bottom = scrbot;
		FillRect(hdc1, &r, brush);
		FillRect(hdc2, &r, brush);
		FillRect(hdcu, &r, brush);
		FillRect(hdcd, &r, brush);
		DeleteObject(brush);
		/*Rectangle(hdc1,0,0,scrright,scrbot);
		Rectangle(hdc2,0,0,scrright,scrbot);*/
		SelectObject(hdc1,pen);
		SelectObject(hdc2,pen);
		break;
	case WM_LBUTTONDOWN:
		if (shape==0)
			break;
		xEx=LOWORD(lParam);
		yEx=HIWORD(lParam);
		MouseDown=1;
		BitBlt(hdcd,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		break;
	case WM_LBUTTONUP:
		if (shape==2)
		{

		}
		MouseDown=0;
		BitBlt(hdcu,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		break;
	case WM_MOUSEMOVE:
		x=LOWORD(lParam);
		y=HIWORD(lParam);
		if (MouseDown)
		{
		//	hdcT=GetDC(hWnd);
		switch (shape)
		{
		case 1:
			pen = CreatePen(PS_SOLID,PenWidth,clr);
			SelectObject(hdc2,pen);
			MoveToEx(hdc2,xEx,yEx,PointEx);
			LineTo(hdc2,x,y);
			xEx=x;
			yEx=y;
			BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		//	ReleaseDC(hWnd, hdcT);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(pen);
			break;
		case 2:
			pen = CreatePen(PS_SOLID,PenWidth,clr);
			SelectObject(hdc2,pen);
			BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			MoveToEx(hdc2,xEx,yEx,PointEx);
			LineTo(hdc2,x,y);
			BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		//	ReleaseDC(hWnd, hdcT);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(pen);
			break;
		case 3:
			pen = CreatePen(PS_SOLID,PenWidth,clr);
		//	brush = CreateSolidBrush();
			SelectObject(hdc2,pen);
			BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			SelectObject(hdc2, GetStockObject(NULL_BRUSH));
			Rectangle(hdc2,xEx,yEx,x,y);
			BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		//	ReleaseDC(hWnd, hdcT);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(pen);
			break;
		case 4:
			pen = CreatePen(PS_SOLID,PenWidth,clr);
		//	brush = CreateSolidBrush();
			SelectObject(hdc2,pen);
			BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			SelectObject(hdc2, GetStockObject(NULL_BRUSH));
			Ellipse(hdc2,xEx,yEx,x,y);
			BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		//	ReleaseDC(hWnd, hdcT);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(pen);
			break;
		}
		}
		break;
/*	case WM_MOUSELEAVE:
		MouseDown=0;
		break;*/
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		case ID_SHAPES_PENCIL:
			shape=1;
			break;
		case ID_SHAPES_LINE:
			shape=2;
			break;
		case ID_SHAPES_RECTANGLE:
			shape=3;
			break;
		case ID_SHAPES_ELLIPSE:
			shape=4;
			break;
		case ID_WIDTHOFLINE_1:
//			DeleteObject(pen);
			PenWidth=1;
			break;
		case ID_WIDTHOFLINE_2:
	//		DeleteObject(pen);
			PenWidth=2;
			break;
		case ID_WIDTHOFLINE_3:
	//		DeleteObject(pen);
			PenWidth=3;
			break;
		case ID_WIDTHOFLINE_4:
	//		DeleteObject(pen);
			PenWidth=4;
			break;
		case ID_WIDTHOFLINE_5:
	//		DeleteObject(pen);
			PenWidth=5;
			break;
		case ID_WIDTHOFLINE_6:
	//		DeleteObject(pen);
			PenWidth=6;
			break;
		case ID_COLOR_SELECTCOLOR:
			GetPenColor(hWnd, &clr);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		BitBlt(hdc,0,0,scrright,scrbot, hdc1,0,0,SRCCOPY);
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

BOOL GetPenColor(HWND hwnd, COLORREF *clrref)
{
	CHOOSECOLOR cc;
	COLORREF aclrCust[16];
	int i;
	for (i = 0; i < 16; i++)
		aclrCust[i] = RGB(255, 255, 255);
	memset(&cc, 0, sizeof(CHOOSECOLOR));
	cc.lStructSize = sizeof(CHOOSECOLOR);
	cc.hwndOwner = hwnd;
	cc.rgbResult = RGB(0, 0, 0);
	cc.lpCustColors = aclrCust;
	cc.Flags = 0;
	if (ChooseColor(&cc))
	{
		*clrref = cc.rgbResult;
		return TRUE;
	}
	else
		return FALSE;
}
