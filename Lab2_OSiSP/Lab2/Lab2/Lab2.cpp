// Lab2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>
#include <Windows.h>
#include <iostream>
#include <thread>
#include <queue>

const int N = 5;
FILE * logFile;
CRITICAL_SECTION logCriticalSection, queueCriticalSection;

typedef int function();
int task1();
int task2();
int task3();
int task4();
int fin();

int task1()
{
	try
	{
		Sleep(2500);
		puts("task 1");
	}
	catch (...)
	{
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Error during work of function task1().\n");
		LeaveCriticalSection(&logCriticalSection);
	}
	return 0;
}

int task2()
{
	try
	{
		Sleep(4500);
		puts("task 2");
	}
	catch (...)
	{
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Error during work of function task2().\n");
		LeaveCriticalSection(&logCriticalSection);
	}
	return 0;
}

int fin()
{
	return 1;
}

int task3()
{
	try
	{
		Sleep(500);
		puts("task 3");
	}
	catch (...)
	{
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Error during work of function task3().\n");
		LeaveCriticalSection(&logCriticalSection);
	}
	return 0;
}

int task4()
{
	try
	{
		Sleep(3700);
		puts("task 4");
	}
	catch (...)
	{
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Error during work of function task4().\n");
		LeaveCriticalSection(&logCriticalSection);
	}
	return 0;
}

class ThreadPool
{
public: 
	ThreadPool(int countOfThreads)
	{
		maxCountOfThreads = countOfThreads;

		InitializeCriticalSection(&logCriticalSection);
		InitializeCriticalSection(&queueCriticalSection);

		threadsHandle = (HANDLE*)malloc(sizeof(HANDLE)*(maxCountOfThreads-1));
		threadsSemaphore = (HANDLE*)malloc(sizeof(HANDLE)*(maxCountOfThreads-1));
		isThreadReady = (bool*)malloc(sizeof(bool)*(maxCountOfThreads-1));

		masterThread = CreateThread(NULL, 0, masterThreadFunction, (LPVOID)(maxCountOfThreads - 1), 0, NULL);
		masterThreadSemaphore = CreateSemaphore(NULL, maxCountOfThreads, maxCountOfThreads, NULL);
		
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile, "Master thread has been created.\n");
		LeaveCriticalSection(&logCriticalSection);

		for (int i = 0; i < maxCountOfThreads-1; i++)
		{
			threadsHandle[i] = CreateThread(NULL, 0, threadFunction, (LPVOID)i, 0, NULL);
			threadsSemaphore[i] = CreateSemaphore(NULL, 0, 1, NULL);
			isThreadReady[i] = true;
		}
	}

	~ThreadPool()
	{
		//wait for multiple, wait for master
	}

	void addTask(function f)
	{
		EnterCriticalSection(&queueCriticalSection);
        queueTasks.push(f);
		ReleaseSemaphore(masterThreadSemaphore, 1, NULL);
        LeaveCriticalSection(&queueCriticalSection);
	}

private:
	static int maxCountOfThreads;
	static HANDLE * threadsHandle;
	static HANDLE masterThread;
	static std::queue <function*> queueTasks;
	static HANDLE * threadsSemaphore;
	static HANDLE masterThreadSemaphore;
	static bool * isThreadReady;

	static DWORD WINAPI masterThreadFunction(LPVOID lParam)
	{
		int numberOfThread = (int) lParam;
		bool isThreadExists = true;
		while (isThreadExists)
		{
			if (WaitForSingleObject(masterThreadSemaphore, 1000) == WAIT_OBJECT_0)
			{
				for (int i = 1; i<maxCountOfThreads; i++)
				{

				}
			}
		}
	}

	static DWORD WINAPI threadFunction(LPVOID lParam)
	{
		int numberOfThread = (int) lParam;
		bool isThreadExists = true;
		while (isThreadExists)
		{
			if (WaitForSingleObject(threadsSemaphore[numberOfThread], 1000) == WAIT_OBJECT_0)
			{
				isThreadReady[numberOfThread] = false;
				function * task;
				EnterCriticalSection(&queueCriticalSection);
	            task = queueTasks.front();
	            queueTasks.pop();
	            LeaveCriticalSection(&queueCriticalSection);
	            if ((*task)())
				{
					isThreadExists = false;
				}
				EnterCriticalSection(&logCriticalSection);
				fprintf(logFile,"Thread %d completed task.\n", numberOfThread);
				LeaveCriticalSection(&logCriticalSection);
				isThreadReady[numberOfThread] = true;
				ReleaseSemaphore(masterThreadSemaphore, 1, NULL);

			}
		}
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Thread %d terminated.\n", numberOfThread);
		LeaveCriticalSection(&logCriticalSection);
		return 0;
	}

};

//static
int ThreadPool::maxCountOfThreads;
HANDLE * ThreadPool::threadsHandle;
HANDLE ThreadPool::masterThread;
std::queue <function*> ThreadPool::queueTasks;
HANDLE ThreadPool::masterThreadSemaphore;
HANDLE * ThreadPool::threadsSemaphore;
bool * ThreadPool::isThreadReady;


int _tmain(int argc, _TCHAR* argv[])
{
	logFile = fopen("log.txt", "w");
	ThreadPool * threadpool = new ThreadPool(N);
	int taskNumber = 0;
	do
	{
			taskNumber = getch();
		if (taskNumber == '1')
			threadpool->addTask(&task1);
		if (taskNumber == '2')
			threadpool->addTask(&task2);
		if (taskNumber == '3')
			threadpool->addTask(&task3);
		if (taskNumber == '4')
			threadpool->addTask(&task4);
	} while (taskNumber!=13);
	
	delete threadpool;
	fprintf(logFile, "Program terminated.\n");
	fclose(logFile);
	return 0;
}

