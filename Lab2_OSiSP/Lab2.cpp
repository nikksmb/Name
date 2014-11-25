// Lab2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>
#include <Windows.h>
#include <iostream>
#include <thread>
#include <queue>

int N = 5;
FILE * logFile;
CRITICAL_SECTION logCriticalSection, queueCriticalSection, boolCriticalSection;

typedef int function();
int task1();
int task2();
int task3();
int task4();
int fin();

void printLog(char * text)
{
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,text);
		fprintf(stdout, text);
		LeaveCriticalSection(&logCriticalSection);
}

int task1()
{
	try
	{
		Sleep(2500);
		puts("task 1");
	}
	catch (...)
	{
		printLog("Error during work of function task1().\n");
	}
	return 0;
}

int task2()
{
	try
	{
		Sleep(9000);
		puts("task 2");
	}
	catch (...)
	{
		printLog("Error during work of function task2().\n");
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
		printLog("Error during work of function task3().\n");
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
		printLog("Error during work of function task4().\n");
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
		InitializeCriticalSection(&boolCriticalSection);

		threadsHandle = (HANDLE*)malloc(sizeof(HANDLE)*(maxCountOfThreads+1));
		threadsSemaphore = (HANDLE*)malloc(sizeof(HANDLE)*(maxCountOfThreads+1));
		isThreadReady = (bool*)malloc(sizeof(bool)*(maxCountOfThreads+1));

		masterThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)masterThreadFunction, (LPVOID)(maxCountOfThreads), 0, NULL);
		masterThreadSemaphore = CreateSemaphore(NULL, maxCountOfThreads, maxCountOfThreads, NULL);
		masterThreadSemaphoreComplete = CreateSemaphore(NULL, 0, 1, NULL);
		
		
		printLog("Master thread has been created.\n");

		for (int i = 0; i < maxCountOfThreads; i++)
		{
			threadsHandle[i] = CreateThread(NULL, 0, threadFunction, (LPVOID)i, 0, NULL);
			threadsSemaphore[i] = CreateSemaphore(NULL, 0, 1, NULL);
			isThreadReady[i] = true;
		}
	}

	~ThreadPool()
	{
		printLog("Starting finish\n");
		for (int i = 0; i < maxCountOfThreads; i++)
		{
			addTask(&fin);
		} 
		WaitForMultipleObjects(maxCountOfThreads,threadsHandle,true,10000);
		free(threadsHandle);
		for (int i = 0; i < maxCountOfThreads; i++)
        {
			CloseHandle(threadsSemaphore[i]);
	    }
		free(threadsSemaphore);
	//	free(&queueTasks);
		EnterCriticalSection(&boolCriticalSection);
		masterThreadExists = false;
		LeaveCriticalSection(&boolCriticalSection);
		WaitForSingleObject(masterThreadSemaphoreComplete, 5000);
		CloseHandle(masterThread);
	}

	void addTask(function * f)
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
	static HANDLE masterThreadSemaphoreComplete;
	static bool * isThreadReady;
	static bool masterThreadExists;

	static DWORD WINAPI masterThreadFunction(LPVOID lParam)
	{
		int numberOfThread = maxCountOfThreads;
		EnterCriticalSection(&boolCriticalSection);
		masterThreadExists = true;
		LeaveCriticalSection(&boolCriticalSection);
		bool masterThread = true;
		bool allBusy;
		while (masterThread)
		{
			if (WaitForSingleObject(masterThreadSemaphore, 1000) == WAIT_OBJECT_0)
			{
				allBusy = false;
				EnterCriticalSection(&boolCriticalSection);
				for (int i = 0; i < maxCountOfThreads; i++)
				{
					if (isThreadReady[i])
					{
						EnterCriticalSection(&queueCriticalSection);
						if (!queueTasks.empty())
						{
							ReleaseSemaphore(threadsSemaphore[i],1,NULL);
						}
						LeaveCriticalSection(&queueCriticalSection);
						break;
					}
					else
					{
						if (i == maxCountOfThreads - 1)
						{
							allBusy = true;
						}
					}

				}
				LeaveCriticalSection(&boolCriticalSection);
				EnterCriticalSection(&boolCriticalSection);
				if (allBusy && masterThreadExists)
				{
					EnterCriticalSection(&queueCriticalSection);
					if (!queueTasks.empty())
					{
						if(queueTasks.front() != &fin)
						{
							printLog("All threads are busy, task will be ignored \n");
							queueTasks.pop();
						}
					}
					LeaveCriticalSection(&queueCriticalSection);
				}
				LeaveCriticalSection(&boolCriticalSection);
			}
			EnterCriticalSection(&boolCriticalSection);
			if (!masterThreadExists)
			{
				masterThread = false;
			}
			LeaveCriticalSection(&boolCriticalSection);
		}
		printLog("Master thread terminated \n");
		ReleaseSemaphore(masterThreadSemaphoreComplete, 1, NULL);
		return 0;
	}

	static DWORD WINAPI threadFunction(LPVOID lParam)
	{
		int numberOfThread = (int) lParam; 
		bool isThreadExists = true;
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Thread %d started.\n", numberOfThread + 1);
		fprintf(stdout,"Thread %d started.\n", numberOfThread + 1);
		LeaveCriticalSection(&logCriticalSection);
		while (isThreadExists)
		{
			if (WaitForSingleObject(threadsSemaphore[numberOfThread], 1000) == WAIT_OBJECT_0)
			{
				EnterCriticalSection(&boolCriticalSection);
				isThreadReady[numberOfThread] = false;
				LeaveCriticalSection(&boolCriticalSection);
				function * task;
				EnterCriticalSection(&queueCriticalSection);
	            task = queueTasks.front();
	            queueTasks.pop();
	            LeaveCriticalSection(&queueCriticalSection);
				EnterCriticalSection(&logCriticalSection);
				fprintf(logFile,"Thread %d started task.\n", numberOfThread + 1);
				fprintf(stdout,"Thread %d started task.\n", numberOfThread + 1);
				LeaveCriticalSection(&logCriticalSection);
	            if ((*task)())
				{
					isThreadExists = false;
				}
				EnterCriticalSection(&logCriticalSection);
				fprintf(logFile,"Thread %d completed task.\n", numberOfThread + 1);
				fprintf(stdout,"Thread %d completed task.\n", numberOfThread + 1);
				LeaveCriticalSection(&logCriticalSection);
				EnterCriticalSection(&boolCriticalSection);
				isThreadReady[numberOfThread] = true;
				LeaveCriticalSection(&boolCriticalSection);
				ReleaseSemaphore(masterThreadSemaphore, 1, NULL);
			}
		}
		EnterCriticalSection(&logCriticalSection);
		fprintf(logFile,"Thread %d terminated.\n", numberOfThread + 1);
		fprintf(stdout,"Thread %d terminated.\n", numberOfThread + 1);
		LeaveCriticalSection(&logCriticalSection);
		EnterCriticalSection(&boolCriticalSection);
		isThreadReady[numberOfThread] = false;
		LeaveCriticalSection(&boolCriticalSection);
		ReleaseSemaphore(masterThreadSemaphore, 1, NULL);
		return 0;
	}

};

//static
int ThreadPool::maxCountOfThreads;
HANDLE * ThreadPool::threadsHandle;
HANDLE ThreadPool::masterThread;
std::queue <function*> ThreadPool::queueTasks;
HANDLE ThreadPool::masterThreadSemaphore;
HANDLE ThreadPool::masterThreadSemaphoreComplete;
HANDLE * ThreadPool::threadsSemaphore;
bool * ThreadPool::isThreadReady;
bool ThreadPool::masterThreadExists;


int _tmain(int argc, _TCHAR* argv[])
{
	printf("Enter count of threads\n");
	scanf_s("%d",&N);
	fopen_s(&logFile,"log.txt", "w");
	ThreadPool * threadpool = new ThreadPool(N);
	int taskNumber = 0;
	while(taskNumber!=13)
	{
		std::cin >> taskNumber;
		if (taskNumber == 1)
			threadpool->addTask(&task1);
		if (taskNumber == 2)
			threadpool->addTask(&task2);
		if (taskNumber == 3)
			threadpool->addTask(&task3);
		if (taskNumber == 4)
			threadpool->addTask(&task4);
	}
	delete threadpool;
	fprintf(logFile, "Program terminated.\n");
	fclose(logFile);
	return 0;
}

