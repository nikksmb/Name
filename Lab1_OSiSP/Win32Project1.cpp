// Win32Project1.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "Win32Project1.h"
#include <commdlg.h>
#include <locale>
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
BOOL GetColor(HWND hwnd, COLORREF *clrref);
void			SaveEnhFile(HWND hWnd);
void				LoadEnhFile(HWND hWnd);
void			PrintImage(HWND hWnd);

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

HBITMAP bufBitmap1,bufBitmap2,bufBitmapu,bufBitmapd;
int scrbot,scrright,exscrbot,exscrright;
HDC hdc1,hdc2,hdc,hdcu,hdcd,hdcT;
LPENHMETAHEADER lpemh=(ENHMETAHEADER *)malloc(EMR_HEADER);
double ZoomK=1;

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	static RECT r;
//	HDC hdc;
	static int x,y,xEx,yEx;
	static LPPOINT PointEx;
	static int Tmp;
	static int MouseDown=0;
	static int shape,NullBrush;
	static int PenWidth;
	HPEN pen;
	//RECT screen;
	//HBRUSH brush;
	HGDIOBJ brush;
	static COLORREF PenClr,BrushClr;
	static int poly,PolyX,PolyY;
	static int polyAddFlag;
	static int TextFlag,TextLength;
	static char *Text;
	static int ShiftState,CtrlState;
	static int ZoomFlag;
	static int MoveX=0,MoveY=0;
	static int PolyFirst;
	switch (message)
	{
	case WM_CREATE:
		setlocale(LC_ALL,"Russian");
		pen = CreatePen(PS_SOLID,2,RGB(255,255,255));
		hdc=GetDC(hWnd);
		exscrright=scrright = GetDeviceCaps(hdc, HORZRES);
		exscrbot=scrbot = GetDeviceCaps(hdc, VERTRES);
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
		SelectObject(hdc1,pen);
		SelectObject(hdc2,pen);
	//	SelectObject(hdc1,brush);
	//	SelectObject(hdc2,brush);
		r.left = 0;
		r.top = 0;
		r.right = scrright;
		r.bottom = scrbot;
		Rectangle(hdc1,0,0,scrright,scrbot);
		Rectangle(hdc2,0,0,scrright,scrbot);
		Rectangle(hdcu,0,0,scrright,scrbot);
		Rectangle(hdcd,0,0,scrright,scrbot);
		/*FillRect(hdc1, &r, brush);
		FillRect(hdc2, &r, brush);
		FillRect(hdcu, &r, brush);
		FillRect(hdcd, &r, brush);*/
		DeleteObject(brush);
		NullBrush=1;
		/*Rectangle(hdc1,0,0,scrright,scrbot);
		Rectangle(hdc2,0,0,scrright,scrbot);*/
		poly=0;
		polyAddFlag=0;
		BrushClr=RGB(255,255,255);
		break;
	case WM_LBUTTONDOWN:
		if (shape==0)
			break;
		if (poly==0)
		{
			xEx=LOWORD(lParam);
			yEx=HIWORD(lParam);
		}
		else
		{
			polyAddFlag=1;
		}
		if (shape==5)
		{
			if (PolyFirst==0)
			{
				PolyFirst=1;
				PolyX=LOWORD(lParam);
				PolyY=HIWORD(lParam);
			}
		}
		if (shape==7)
		{
			if (TextFlag==0)
			{
				TextFlag=1;
				Text=(char*)malloc(1);
				Text[0] = '\0';
				xEx=LOWORD(lParam);
				yEx=HIWORD(lParam);
				TextLength=0;
			}
			else
			{
				TextFlag=0;
			}
		}
		MouseDown=1;
		BitBlt(hdcd,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		break;
	case WM_RBUTTONDOWN:
		if (shape==5)
		{
			pen = CreatePen(PS_SOLID,PenWidth,PenClr);
			SelectObject(hdc2,pen);
			//BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			MoveToEx(hdc2,xEx,yEx,PointEx);
			LineTo(hdc2,PolyX,PolyY);
			BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(pen);
			PolyFirst=0;
		}
		break;
	case WM_LBUTTONUP:
		if (shape==5)
		{
			if (polyAddFlag==1)
			{
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
				SelectObject(hdc2,pen);
				BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				MoveToEx(hdc2,xEx,yEx,PointEx);
				LineTo(hdc2,LOWORD(lParam),HIWORD(lParam));
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
		//		ReleaseDC(hWnd, hdcT);
				InvalidateRect(hWnd,NULL,FALSE);
				DeleteObject(pen);
			}
			xEx=LOWORD(lParam);
			yEx=HIWORD(lParam);
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
			MoveX=0;
			MoveY=0;
			poly=0;
			switch (shape)
			{
			case 1:
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
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
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
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
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
				if (NullBrush==0)
				{
					brush = CreateSolidBrush(BrushClr);
					SelectObject(hdc2,brush);
				}
				else
				{
					brush = GetStockObject(NULL_BRUSH);
					SelectObject(hdc2,brush);
				}
			//	brush = CreateSolidBrush();
				SelectObject(hdc2,pen);
				BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				//SelectObject(hdc2, GetStockObject(NULL_BRUSH));
				Rectangle(hdc2,xEx,yEx,x,y);
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
			//	ReleaseDC(hWnd, hdcT);
				InvalidateRect(hWnd,NULL,FALSE);
				DeleteObject(brush);
				DeleteObject(pen);
				break;
			case 4:
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
				if (NullBrush==0)
					brush = CreateSolidBrush(BrushClr);
				else
					brush = GetStockObject(NULL_BRUSH);
				SelectObject(hdc2,brush);
				SelectObject(hdc2,pen);
				BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				//SelectObject(hdc2, GetStockObject(NULL_BRUSH));
				Ellipse(hdc2,xEx,yEx,x,y);
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
			//	ReleaseDC(hWnd, hdcT);
				InvalidateRect(hWnd,NULL,FALSE);
				DeleteObject(brush);
				DeleteObject(pen);
				break;
			case 5:
				poly = 1;
				pen = CreatePen(PS_SOLID,PenWidth,PenClr);
				SelectObject(hdc2,pen);
				BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				MoveToEx(hdc2,xEx,yEx,PointEx);
				LineTo(hdc2,x,y);
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
			//	ReleaseDC(hWnd, hdcT);
				InvalidateRect(hWnd,NULL,FALSE);
				DeleteObject(pen);
				polyAddFlag=0;
				break;
			case 6:
				pen = CreatePen(PS_SOLID,1,RGB(255,255,255));
				brush = CreateSolidBrush(RGB(255,255,255));
				SelectObject(hdc2,brush);
				SelectObject(hdc2,pen);
				//BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				Rectangle(hdc2,x-5,y-5,x+5,y+5);
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
			//	ReleaseDC(hWnd, hdcT);
				InvalidateRect(hWnd,NULL,FALSE);
				DeleteObject(brush);
				DeleteObject(pen);
				break;
			}
		}
		break;
/*	case WM_MOUSELEAVE:
		MouseDown=0;
		break;*/
	case WM_KEYDOWN:
        if (LOWORD(wParam) == VK_SHIFT)
			ShiftState=1;
	    if (LOWORD(wParam == VK_CONTROL))
			CtrlState=1;
		break;
	case WM_KEYUP:
		ShiftState=0;
		if (CtrlState==1)
		{
			ZoomFlag=0;
			ZoomK=1;
			InvalidateRect(hWnd, NULL, false);
		}
		CtrlState=0;
		break;
	case WM_MOUSEWHEEL:
		exscrright=scrright*ZoomK;
		exscrbot=scrbot*ZoomK;
		if (HIWORD(wParam) > WHEEL_DELTA)
		{
			if (CtrlState==1)
			{
				ZoomK = ZoomK/1.1;
				ZoomFlag=1;
			}
			else
			{
				if (ShiftState==1)
				{
					if (MoveX+5>scrbot/2)
					{
						break;
					}
					ScrollDC(hdc1, 5, 0, &r, NULL, NULL, NULL);
					MoveX=MoveX+5;
					BitBlt(hdc, 0, 0, scrright, scrbot, hdc1, scrright, scrbot,/*scrhor/3, scrvert/3,*/ SRCCOPY);
				}
				else
				{
					if (MoveY+5>scrright/2)
					{
						break;
					}
					ScrollDC(hdc1, 0, 5, &r, NULL, NULL, NULL);
					MoveY=MoveY+5;
					BitBlt(hdc, 0, 0, scrright, scrbot, hdc1, scrright, scrbot,/*scrhor/3, scrvert/3,*/ SRCCOPY);
				}
			}
		}
		else
		{
			if (CtrlState==1)
			{
				ZoomK = ZoomK*1.1;
				ZoomFlag = 1;
			}
			else
			{
				if (ShiftState==1)
				{
					if (MoveX-5<0)
						break;
					ScrollDC(hdc1, -5, 0, &r, NULL, NULL, NULL);
					MoveX=MoveX-5;
					BitBlt(hdc, 0, 0, scrright, scrbot, hdc1, scrright, scrbot,/*scrhor/3, scrvert/3,*/ SRCCOPY);
				}
				else
				{
					if (MoveY-5<0)
						break;
					ScrollDC(hdc1, 0, -5, &r, NULL, NULL, NULL);
					MoveY=MoveY-5;
					BitBlt(hdc, 0, 0, scrright, scrbot, hdc1, scrright, scrbot,/*scrhor/3, scrvert/3,*/ SRCCOPY);
				}
			}
		}
		InvalidateRect(hWnd, NULL, false);
		break;
	case WM_CHAR:
		if ((CtrlState==1) && (wParam == 26))
		{
			BitBlt(hdc1,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
			shape=0;
			TextFlag=0;
			InvalidateRect(hWnd,NULL,FALSE);
			break;
		}
		if ((TextFlag) && (shape==7))
		{
			TextLength=strlen(Text);
			if (wParam != 8)
			{
				if (wParam!=13)
				{
					SetTextColor(hdc2,PenClr);
					//if (BrushClr==RGB(0,0,0))
					//{
					//	SetBkColor(hdc2,RGB(255,255,255));
					//}
					//else
					//{
					SetBkColor(hdc2,BrushClr);
					//}
					Text = (char*)realloc(Text, TextLength + 2);
					TextLength = TextLength + 2;
					Text[TextLength - 2] = wParam;
					Text[TextLength - 1] = '\0';
					TextOutA(hdc2, xEx/*+scrhor/3*/, yEx/*+scrvert/3*/, Text, strlen(Text));
					BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
					InvalidateRect(hWnd, NULL, false);
				}
				else
				{	
					TextFlag=0;
				}
			}
			else
			{
				Text[TextLength-1]='\0';
				BitBlt(hdc2,0,0,scrright,scrbot,hdcd,0,0,SRCCOPY);
				TextOutA(hdc2, xEx/*+scrhor/3*/, yEx/*+scrvert/3*/, Text, strlen(Text));
				BitBlt(hdc1,0,0,scrright,scrbot,hdc2,0,0,SRCCOPY);
				InvalidateRect(hWnd, NULL, false);
			}
		}
		break;
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		poly=0;
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
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_SHAPES_LINE:
			shape=2;
			TextFlag=0;
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			break;
		case ID_SHAPES_RECTANGLE:
			shape=3;
			TextFlag=0;
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			break;
		case ID_SHAPES_ELLIPSE:
			shape=4;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_SHAPES_POLYLINE:
			//poly=1;
			shape=5;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_WIDTHOFLINE_1:
//			DeleteObject(pen);
			PenWidth=1;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_WIDTHOFLINE_2:
	//		DeleteObject(pen);
			PenWidth=2;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_WIDTHOFLINE_3:
	//		DeleteObject(pen);
			PenWidth=3;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_WIDTHOFLINE_4:
	//		DeleteObject(pen);
			PenWidth=4;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_WIDTHOFLINE_5:
	//		DeleteObject(pen);
			PenWidth=5;
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			break;
		case ID_WIDTHOFLINE_6:
	//		DeleteObject(pen);
			PenWidth=6;
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_COLOR_SELECTCOLOR:
			GetColor(hWnd, &PenClr);
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_COLOR_SELECTBRUSHCOLOR:
			GetColor(hWnd, &BrushClr);
			NullBrush=0;
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_COLOR_WITHOUTBRUSH:
			NullBrush=1;
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_EDIT_ERASER:
			shape=6;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_FILE_S:
			SaveEnhFile(hWnd);
			TextFlag=0;
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			break;
		case ID_FILE_LOADFILE:
			LoadEnhFile(hWnd);
			/*pen = CreatePen(PS_SOLID,1,RGB(255,255,255));
			brush = CreateSolidBrush(RGB(255,255,255));
			SelectObject(hdc1,brush);
			SelectObject(hdc1,pen);
			SelectObject(hdc2,brush);
			SelectObject(hdc2,pen);
			Rectangle(hdc1,0,0,scrright,scrbot);
			Rectangle(hdc2,0,0,scrright,scrbot);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(brush);
			DeleteObject(pen);*/
			TextFlag=0;
			MoveX=0;
			MoveY=0;
			PolyFirst=0;
			break;
		case ID_FILE_PRINTTOFILE:
			PrintImage(hWnd);
			TextFlag=0;
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			break;
		case ID_TEXT_PRINTTEXT:
			shape=7;
			MoveX=0;
			MoveY=0;
			TextFlag=0;
			PolyFirst=0;
			break;
		case ID_EDIT_CLEARALL:
			pen = CreatePen(PS_SOLID,1,RGB(255,255,255));
			brush = CreateSolidBrush(RGB(255,255,255));
			SelectObject(hdc1,brush);
			SelectObject(hdc1,pen);
			SelectObject(hdc2,brush);
			SelectObject(hdc2,pen);
			Rectangle(hdc1,0,0,scrright,scrbot);
			Rectangle(hdc2,0,0,scrright,scrbot);
			InvalidateRect(hWnd,NULL,FALSE);
			DeleteObject(brush);
			DeleteObject(pen);
			PolyFirst=0;
			MoveX=0;
			MoveY=0;
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		if (ZoomFlag==1)
		{
		//	StretchBlt(hdc1, 0,0, scrright, scrbot, hdc1, 0,0,/*scrright, scrbot,*/ scrright*ZoomK, scrbot*ZoomK, SRCCOPY);
			StretchBlt(hdc, 0,0, scrright, scrbot, hdc1, 0,0,/*scrright, scrbot,*/ scrright*ZoomK, scrbot*ZoomK, SRCCOPY);
		}
		else
		{
			BitBlt(hdc,0,0,scrright,scrbot, hdc1,0,0,SRCCOPY);
		}
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

BOOL GetColor(HWND hwnd, COLORREF *clrref)
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

void PrintImage(HWND hWnd)
{
	PRINTDLG PrintDialog;
	memset(&PrintDialog, 0, sizeof(PRINTDLG));
	PrintDialog.lStructSize = sizeof(PRINTDLG);
	PrintDialog.hwndOwner = hWnd;
	PrintDialog.hDevMode = NULL;
	PrintDialog.hDevNames = NULL;
	PrintDialog.nCopies = 1;
	PrintDialog.Flags = PD_RETURNDC;
	PrintDialog.nMinPage = 1;
	PrintDialog.nMaxPage = 0xFFFF;
	PrintDlg(&PrintDialog);
	DOCINFO docInfo;
	docInfo.cbSize = sizeof(docInfo);
	docInfo.lpszDocName = L"Printing";
	docInfo.lpszOutput = 0;
	docInfo.lpszDatatype = 0;
	docInfo.fwType = 0;
	StartDoc(PrintDialog.hDC, &docInfo); //start print job
	RECT printRect;
	GetClientRect(hWnd, &printRect);
	StretchBlt(PrintDialog.hDC, 0, 0, scrright*5, scrbot*5, hdc1, 0, 0, scrright, scrbot,SRCCOPY);
	EndDoc(PrintDialog.hDC);
}

void LoadEnhFile(HWND hWnd)
{
    OPENFILENAME LFile;
	int t;
    WCHAR FileName[MAX_PATH], FilePath[MAX_PATH];
    FileName[0] = '\0';
    FilePath[0] = '\0';
    LFile.lStructSize = sizeof(OPENFILENAME);
    LFile.hwndOwner = hWnd;
    LFile.lpstrFilter = L"EMF(*.emf)\0";
    LFile.lpstrCustomFilter = 0;
    LFile.lpstrFile = FilePath;
    LFile.nMaxFile = MAX_PATH * sizeof(WCHAR);
    LFile.lpstrFileTitle = FileName;
    LFile.nMaxFileTitle = MAX_PATH * sizeof(WCHAR);
    LFile.lpstrInitialDir = 0;
    LFile.lpstrDefExt = L"emf";
    LFile.lpstrTitle = L"Load file from:";
    LFile.Flags = OFN_PATHMUSTEXIST;
    if (GetOpenFileName(&LFile))
	{
		UINT Buffer=(UINT)malloc(EMR_HEADER);
	//	LPENHMETAHEADER lpemh=(ENHMETAHEADER *)malloc(EMR_HEADER);
		RECTL ClRect;
		HENHMETAFILE enhFile = GetEnhMetaFile(LFile.lpstrFile);
		int iWidthMM, iHeightMM, iWidthPels, iHeightPels;
		iWidthMM = GetDeviceCaps(hdc1, HORZSIZE); 
		iHeightMM = GetDeviceCaps(hdc1, VERTSIZE); 
		iWidthPels = GetDeviceCaps(hdc1, HORZRES); 
		iHeightPels = GetDeviceCaps(hdc1, VERTRES);
		//if (t=GetEnhMetaFileHeader(enhFile,Buffer,lpemh))
		//{
		//}
		//else
		//	exit;
		//emh=lpemh;//
		ClRect=lpemh->rclFrame;
		RECT client;
		//client.bottom=ClRect.bottom/**iHeightPels*//(100*iHeightMM);
		//client.top=0;//ClRect.top/**iHeightPels*//(100*iHeightMM);
		//client.right=ClRect.right/**iWidthPels*//(100*iWidthMM);
		//client.left=0;//ClRect.left/**iWidthPels*//(100*iWidthMM);
		GetClientRect(hWnd, &client);
		HPEN pen = CreatePen(PS_SOLID,1,RGB(255,255,255));
		HBRUSH brush = CreateSolidBrush(RGB(255,255,255));
		SelectObject(hdc1,brush);
		SelectObject(hdc1,pen);
		SelectObject(hdc2,brush);
		SelectObject(hdc2,pen);
		Rectangle(hdc1,0,0,scrright,scrbot);
		Rectangle(hdc2,0,0,scrright,scrbot);
		InvalidateRect(hWnd,NULL,FALSE);
		DeleteObject(brush);
		DeleteObject(pen);
		PlayEnhMetaFile(hdc1, enhFile, &client);
		PlayEnhMetaFile(hdc2, enhFile, &client);
		DeleteEnhMetaFile(enhFile);
	//	free(lpemh);
		InvalidateRect(hWnd, NULL, false);
	}
}
	
void SaveEnhFile(HWND hWnd)
{
    OPENFILENAME SFile;
    WCHAR FileName[MAX_PATH], FilePath[MAX_PATH];
    FileName[0] = '\0';
    FilePath[0] = '\0';
    SFile.lStructSize = sizeof(OPENFILENAME);
    SFile.hwndOwner = hWnd;
    SFile.lpstrFilter = L"EMF(*.emf)\0";
    SFile.lpstrCustomFilter = 0;
    SFile.lpstrFile = FilePath;
    SFile.nMaxFile = MAX_PATH * sizeof(WCHAR);
    SFile.lpstrFileTitle = FileName;
    SFile.nMaxFileTitle = MAX_PATH * sizeof(WCHAR);
    SFile.lpstrInitialDir = 0;
    SFile.lpstrDefExt = L"emf";
    SFile.lpstrTitle = L"Save file as:";
    SFile.Flags = OFN_PATHMUSTEXIST | OFN_OVERWRITEPROMPT;
    GetSaveFileName (&SFile);
	    //Creating an Enhanced Metafile
    HDC hdcRef = GetDC(hWnd);
    int iWidthMM, iHeightMM, iWidthPels, iHeightPels;
    RECT EnhClient;
    iWidthMM = GetDeviceCaps(hdcRef, HORZSIZE); 
    iHeightMM = GetDeviceCaps(hdcRef, VERTSIZE); 
    iWidthPels = GetDeviceCaps(hdcRef, HORZRES); 
    iHeightPels = GetDeviceCaps(hdcRef, VERTRES);
    GetClientRect(hWnd, &EnhClient);
	EnhClient.left = (EnhClient.left * iWidthMM * 100) / iWidthPels;
    EnhClient.top = (EnhClient.top * iHeightMM * 100) / iHeightPels;
    EnhClient.right = (EnhClient.right * iWidthMM * 100) / iWidthPels; 
    EnhClient.bottom = (EnhClient.bottom * iHeightMM * 100) / iHeightPels;
    HDC hdcEmf = CreateEnhMetaFile(hdcRef, SFile.lpstrFile, &EnhClient, 0);
	BitBlt(hdcEmf, 0, 0, scrright, scrbot, hdc1, 0, 0, SRCCOPY);
    CloseEnhMetaFile(hdcEmf);
    ReleaseDC(hWnd, hdcRef);
}